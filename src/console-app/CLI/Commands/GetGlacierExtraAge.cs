using System;
using System.Collections.Generic;
using System.Linq;
using Glacier.Tools.Entities;

namespace Glacier.Tools.CLI.Commands
{
    internal class GetGlacierExtraAge : ICommand
    {
        readonly string _vaultInventoryFile;
        readonly string _nasPath;
        readonly TimeSpan _glacierOlderThan;

        public GetGlacierExtraAge(string vaultInventoryFile, string nasPath, TimeSpan glacierOlderThan)
        {
            if (string.IsNullOrWhiteSpace(vaultInventoryFile))
            {
                throw new ArgumentNullException("vaultInventoryFile");
            }

            if (string.IsNullOrWhiteSpace(nasPath))
            {
                throw new ArgumentNullException("nasPath");
            }

            _vaultInventoryFile = vaultInventoryFile;
            _nasPath = nasPath;
            _glacierOlderThan = glacierOlderThan;
        }

        #region Implementation of ICommand

        public void Execute()
        {
            VaultInventory vaultInventory = VaultInventoryParser.Parse(_vaultInventoryFile);

            NasFiles nasFiles = NasBackup.GetFiles(_nasPath);

            IEnumerable<GlacierFile> glacierFiles = vaultInventory.ArchiveList;

            IEnumerable<File> localFiles = nasFiles.Files;

            GlacierFile[] glacierExtraAgeFiles = FileMatcher.GetGlacierExtraFiles(glacierFiles,
                                                                                  localFiles,
                                                                                  _glacierOlderThan)
                                                            .ToArray();

            int glacierExtraFilesCount = glacierExtraAgeFiles.Count();

            var result = new GlacierExtraAgeResult(glacierExtraFilesCount, glacierExtraAgeFiles);

            ConsoleView.Show(result);
        }

        #endregion
    }
}
