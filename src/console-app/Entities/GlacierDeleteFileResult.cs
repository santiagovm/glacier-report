namespace Glacier.Tools.Entities
{
    internal class GlacierDeleteFileResult
    {
        public GlacierDeleteFileResult(string normalizedFilePath, string deleteFailure, long fileSizeBytes)
        {
            FileSizeBytes = fileSizeBytes;
            DeleteFailure = deleteFailure;
            NormalizedFilePath = normalizedFilePath;
        }

        public string NormalizedFilePath { get; private set; }

        public string DeleteFailure { get; private set; }

        public long FileSizeBytes { get; private set; }

        public bool DeleteFailed 
        {
            get { return string.IsNullOrWhiteSpace(DeleteFailure) == false; }
        }
    }
}
