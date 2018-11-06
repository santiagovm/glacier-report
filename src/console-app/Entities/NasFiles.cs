using System.Collections.Generic;

namespace Glacier.Tools.Entities
{
    internal class NasFiles
    {
        public NasFiles(string path, IEnumerable<File> files)
        {
            Files = files;
            Path = path;
        }

        public string Path { get; private set; }

        public IEnumerable<File> Files { get; private set; }
    }
}
