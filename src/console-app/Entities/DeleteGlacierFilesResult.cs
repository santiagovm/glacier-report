using System.Collections.Generic;
using System.Linq;

namespace Glacier.Tools.Entities
{
    internal class DeleteGlacierFilesResult
    {
        public DeleteGlacierFilesResult(IEnumerable<GlacierDeleteFileResult> deletedFiles)
        {
            DeletedFiles = deletedFiles;
        }

        public IEnumerable<GlacierDeleteFileResult> DeletedFiles { get; private set; }

        public int DeletedFilesCount
        {
            get { return DeletedFiles.Count(f => f.DeleteFailed == false); }
        }

        public long DeletedFilesSizeBytes 
        {
            get
            {
                return DeletedFiles.Where(f => f.DeleteFailed == false)
                                   .Select(f => f.FileSizeBytes)
                                   .Sum();
            }
        }

        public int DeletionFailuresCount
        {
            get { return DeletedFiles.Count(f => f.DeleteFailed); }
        }
    }
}
