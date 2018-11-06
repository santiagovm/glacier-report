using System;
using System.Collections.Generic;
using System.Linq;
using Glacier.Tools.Entities;

namespace Glacier.Tools
{
    public class FileMatcher
    {
        public static IEnumerable<GlacierFile> GetGlacierExtraFiles(IEnumerable<GlacierFile> glacierFiles, 
                                                                    IEnumerable<File> nasFiles)
        {
            IEnumerable<GlacierFile> glacierExtraFiles =
                glacierFiles.Where(g => nasFiles.All(n => n.NormalizedFilePath != g.NormalizedFilePath))
                            .Select(g => g);

            return glacierExtraFiles;
        }

        public static IEnumerable<GlacierFile> GetGlacierExtraFiles(IEnumerable<GlacierFile> glacierFiles, 
                                                                    IEnumerable<File> nasFiles, 
                                                                    TimeSpan glacierOlderThan)
        {
            IEnumerable<GlacierFile> glacierExtraFiles = GetGlacierExtraFiles(glacierFiles, nasFiles);

            DateTimeOffset now = DateTimeOffset.Now;

            IEnumerable<GlacierFile> glacierExtraAgeFiles = 
                glacierExtraFiles.Where(g => now.Subtract(g.DateArchived) > glacierOlderThan)
                                 .OrderByDescending(g => g.AgeDays)
                                 .Select(g => g);

            return glacierExtraAgeFiles;
        }
    }
}
