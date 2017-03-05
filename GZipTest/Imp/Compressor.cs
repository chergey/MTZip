using System;
using System.IO;
using System.IO.Compression;
using TestTask.Enums;
using TestTask.Interfaces;

namespace TestTask.Imp
{
    public class Compressor : ICompressor
    {
        private readonly FiFo.Chunk _chunk;
        private readonly Activity _activity;
        public Compressor(FiFo.Chunk chunk, Activity activity)
        {
            _chunk = chunk;
            _activity = activity;
        }

        public byte[] Do()
        {
            return _activity == Activity.Compress ? CompressChunk() : DecompressChunk();
        }

        public byte[] CompressChunk()
        {
            using (var ms = new MemoryStream())
            {
                using (var gs = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    gs.Write(_chunk.Data, 0, _chunk.Data.Length);
                    var data = ms.ToArray();
                    if (data.Length == 0)
                    {
                        data = GZipHeader.GetDefaultHeader();
                    }

                    GZipHeader.WriteSegmentSizeToExtraField(ref data);

                    return data;
                }
            }
        }

        public byte[] DecompressChunk()
        {
            using (var tstream = new MemoryStream())
            {
                using (var ms = new MemoryStream(_chunk.Data))
                {
                    using (var gs = new GZipStream(ms, CompressionMode.Decompress))
                    {
                        byte[] buffer = new byte[16 * 1024];
                        int bytesRead;

                        while ((bytesRead = gs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            tstream.Write(buffer, 0, bytesRead);
                        }
                        return tstream.ToArray();
                    }
                }
            }
        }
    }
}