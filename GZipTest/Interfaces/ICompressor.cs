namespace TestTask.Interfaces
{
    /// <summary>
    /// Compressor interface
    /// </summary>
    public interface ICompressor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        byte[] Do();

        /// <summary>
        /// compress chunk
        /// </summary>
        /// <returns></returns>
        byte[] CompressChunk();

        /// <summary>
        /// decompress chunk
        /// </summary>
        /// <returns></returns>
        byte[] DecompressChunk();
    }
}