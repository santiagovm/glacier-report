using System.Collections.Generic;
using System.Linq;

namespace Glacier.Tools.Entities
{
    internal class GlacierExtraAgeResult
    {
        public GlacierExtraAgeResult(int glacierExtraFilesCount, IEnumerable<GlacierFile> glacierExtraFiles)
        {
            Files = glacierExtraFiles;
            FilesCount = glacierExtraFilesCount;
        }

        public int FilesCount { get; private set; }

        public IEnumerable<GlacierFile> Files { get; private set; }

        public long FilesSizeBytes 
        {
            get { return Files.Select(f => f.SizeBytes).Sum(); }
        }
    }
}
