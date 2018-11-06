using System.Collections.Generic;
using System.IO;
using System.Linq;
using Glacier.Tools.Entities;
using File = Glacier.Tools.Entities.File;

namespace Glacier.Tools
{
    internal class NasBackup
    {
        public static NasFiles GetFiles(string path)
        {
            var info = new DirectoryInfo(path);

            FileInfo[] fileInfoList = info.GetFiles(searchPattern: "*", searchOption: SearchOption.AllDirectories);

            var files = new List<File>(capacity: fileInfoList.Length);

            files.AddRange(fileInfoList.Select(fi => new File(fi.FullName,
                                                              fi.LastWriteTimeUtc,
                                                              sizeBytes: fi.Length))
                                       .OrderBy(f => f.FilePath));

            return new NasFiles(path, files);
        }
    }
}
