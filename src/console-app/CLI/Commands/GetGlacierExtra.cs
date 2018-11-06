using System;
using System.Collections.Generic;
using System.Linq;
using Glacier.Tools.Entities;

namespace Glacier.Tools.CLI.Commands
{
    internal class GetGlacierExtra : ICommand
    {
        readonly string _vaultInventoryFile;
        readonly string _nasPath;

        public GetGlacierExtra(string vaultInventoryFile, string nasPath)
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
        }

        #region Implementation of ICommand

        public void Execute()
        {
            VaultInventory vaultInventory = VaultInventoryParser.Parse(_vaultInventoryFile);

            NasFiles nasFiles = NasBackup.GetFiles(_nasPath);

            IEnumerable<GlacierFile> glacierFiles = vaultInventory.ArchiveList;

            IEnumerable<File> localFiles = nasFiles.Files;

            GlacierFile[] glacierExtraFiles = FileMatcher.GetGlacierExtraFiles(glacierFiles, localFiles)
                                                         .ToArray();

            int glacierFilesCount = vaultInventory.ArchiveList.Count();

            int nasFilesCount = nasFiles.Files.Count();

            int glacierExtraFilesCount = glacierExtraFiles.Count();

            var result = new GlacierExtraResult(glacierFilesCount,
                                                nasFilesCount,
                                                glacierExtraFilesCount,
                                                glacierExtraFiles);

            ConsoleView.Show(result);
        }

        #endregion
    }
}
