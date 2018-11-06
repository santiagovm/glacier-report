using System.Collections.Generic;
using System.Linq;

namespace Glacier.Tools.Entities
{
    internal class GlacierExtraResult
    {
        public GlacierExtraResult(int glacierFilesCount, 
                                  int nasFilesCount, 
                                  int glacierExtraFilesCount, 
                                  IEnumerable<GlacierFile> glacierExtraFiles)
        {
            GlacierExtraFiles = glacierExtraFiles;
            GlacierExtraFilesCount = glacierExtraFilesCount;
            NasFilesCount = nasFilesCount;
            GlacierFilesCount = glacierFilesCount;
        }

        public int GlacierFilesCount { get; private set; }

        public int NasFilesCount { get; private set; }

        public int GlacierExtraFilesCount { get; private set; }

        public IEnumerable<GlacierFile> GlacierExtraFiles { get; private set; }

        public long GlacierExtraFilesSizeBytes 
        {
            get { return GlacierExtraFiles.Select(g => g.SizeBytes).Sum(); }
        }
    }
}
