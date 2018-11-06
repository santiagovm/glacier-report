using System;

namespace Glacier.Tools.Entities
{
    public class File
    {
        public File(string filePath, DateTimeOffset dateModified, long sizeBytes)
        {
            SizeBytes = sizeBytes;
            DateModified = dateModified;
            FilePath = filePath;
        }

        public string FilePath { get; private set; }

        public DateTimeOffset DateModified { get; private set; }

        public long SizeBytes { get; private set; }

        public string NormalizedFilePath
        {
            get
            {
                int startIndex = FilePath.IndexOf("backup", StringComparison.OrdinalIgnoreCase);

                string filePath;

                if (startIndex > -1)
                {
                    filePath = FilePath.Substring(startIndex);
                }
                else
                {
                    filePath = FilePath;
                }

                return filePath.Replace(oldChar: '\\', newChar: '/')
                               .Replace(oldValue: "+AF8-", newValue: "_");
            }
        }

        public bool IsBackupFile
        {
            get { return FilePath.IndexOf("backup", StringComparison.OrdinalIgnoreCase) > -1; }
        }
    }
}
