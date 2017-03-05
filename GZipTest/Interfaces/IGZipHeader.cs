using TestTask.Enums;
using TestTask.Imp;

namespace TestTask.Interfaces
{
    public interface IGZipHeader
    {
        /// <summary>
        /// Check if gzip format
        /// </summary>
        /// <returns></returns>
        bool IsGzipFormat();

        /// <summary>
        /// Get segment size
        /// </summary>
        /// <returns></returns>
        int GetSegmentSize();

        /// <summary>
        /// Check if header has a specified flag
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        bool HasFlag(GZipHeaderFlags flag);

        /// <summary>
        /// Check if SubfieldId is correct
        /// </summary>
        /// <returns></returns>
        bool IsSubfieldIDsCorrect();
    }
}