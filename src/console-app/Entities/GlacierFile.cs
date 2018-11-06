using System;

namespace Glacier.Tools.Entities
{
    public class GlacierFile : File
    {
        public GlacierFile(string filePath, 
                           DateTimeOffset dateModified, 
                           long sizeBytes, 
                           DateTimeOffset dateArchived, 
                           string archiveId) 
            : base(filePath, dateModified, sizeBytes)
        {
            ArchiveId = archiveId;
            DateArchived = dateArchived;
        }

        public DateTimeOffset DateArchived { get; private set; }

        public string ArchiveId { get; private set; }

        public int AgeDays 
        {
            get { return (int) DateTimeOffset.Now.Subtract(DateArchived).TotalDays; }
        }
    }
}
