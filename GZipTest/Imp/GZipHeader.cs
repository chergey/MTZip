using System;
using System.IO;
using TestTask.Enums;
using TestTask.Interfaces;

namespace TestTask.Imp
{
    public class GZipHeader : IGZipHeader
    {
        private readonly byte []_header;
        private enum Sizes
        {
            HeaderSize = 10,
            XlenByteSize = 2,
            S1Index = HeaderSize + 2,
            S2Index = HeaderSize + 3,
            LenIndex = HeaderSize + 4,
            Info = HeaderSize + 6,
            ID1 = 0x1f,
            ID2 = 0x8b,
            CM = 8,
            XFL = 4,
            SI = 0x01,

        }


        private enum Indexes
        {
            ID1 = 0,
            ID2 = 1,
            CM = 2,
            Flags = 3,
            XFL = 8,
        }

        public GZipHeader(byte[] header)
        {
            _header = header;
        }

        public bool IsGzipFormat()
        {
            return _header[(int)Indexes.ID1] == 0x1f && _header[(int)Indexes.ID2] == 0x8b;
        }

        /// <exception cref="InvalidDataException">Not a Gzip format!</exception>
        public int GetSegmentSize()
        {
            if (!IsGzipFormat())
            {
                throw new InvalidDataException("Not a Gzip format!");
            }

            if (!HasFlag(GZipHeaderFlags.Fextra) | !IsSubfieldIDsCorrect())
                throw new InvalidDataException("Wrong algorithm!");

            return BitConverter.ToInt32(_header, (int) Sizes.Info);
        }

        public bool HasFlag(GZipHeaderFlags flag)
        {
            return (_header[(int)Indexes.Flags] & (byte)flag) != 0;
        }

        public bool IsSubfieldIDsCorrect()
        {
            return (int)Sizes.SI == _header[(int)Sizes.S1Index] && (int)Sizes.SI == _header[(int)Sizes.S2Index];
        }

        public static byte[] GetDefaultHeader()
        {
            byte[] result = new byte[(int)Sizes.HeaderSize];
            result[(int)Indexes.ID1] = (int)Sizes.ID1;
            result[(int)Indexes.ID2] = (int)Sizes.ID2;
            result[(int)Indexes.CM] = (int)Sizes.CM;
            result[(int)Indexes.XFL] = (int)Sizes.XFL;
            return result;
        }

        public static void WriteSegmentSizeToExtraField(ref byte[] data)
        {
            data[(int)Indexes.Flags] |= (byte)GZipHeaderFlags.Fextra;
            AddExtraField(ref data);
            WriteExtraFieldInformation(ref data);
        }

        private static void AddExtraField(ref byte[] data)
        {
            Array.Resize(ref data, data.Length + (int)Sizes.HeaderSize);
            int dataSize = data.Length - (int)Sizes.HeaderSize * 2;
            Array.Copy(data, (int)Sizes.HeaderSize, data, (int)Sizes.HeaderSize * 2, dataSize);
        }

        private static void WriteExtraFieldInformation(ref byte[] data)
        {
            byte[] xlen = BitConverter.GetBytes((int)Sizes.HeaderSize - (int)Sizes.XlenByteSize);
            xlen.CopyTo(data, (int)Sizes.HeaderSize);
            data[(int)Sizes.S1Index] = (int)Sizes.SI;
            data[(int)Sizes.S2Index] = (int)Sizes.SI;
            byte[] segmentSizeInfo = BitConverter.GetBytes(data.Length);
            byte[] len = BitConverter.GetBytes((short)segmentSizeInfo.Length);
            len.CopyTo(data, (int)Sizes.LenIndex);
            segmentSizeInfo.CopyTo(data, (int)Sizes.Info);
        }
    }
}
