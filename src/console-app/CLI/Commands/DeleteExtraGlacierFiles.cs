using System;
using System.Collections.Generic;
using Amazon;
using Amazon.Glacier.Transfer;
using Glacier.Tools.Entities;

namespace Glacier.Tools.CLI.Commands
{
    internal class DeleteExtraGlacierFiles : ICommand
    {
        readonly string _vaultInventoryFile;
        readonly string _nasPath;
        readonly TimeSpan _glacierOlderThan;

        public DeleteExtraGlacierFiles(string vaultInventoryFile, string nasPath, TimeSpan glacierOlderThan)
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

            IEnumerable<GlacierFile> glacierExtraAgeFiles = FileMatcher.GetGlacierExtraFiles(glacierFiles,
                                                                                             localFiles,
                                                                                             _glacierOlderThan);

            var deleteFileResults = new List<GlacierDeleteFileResult>();

            using (var mgr = new ArchiveTransferManager(RegionEndpoint.USWest2))
            {
                foreach (GlacierFile g in glacierExtraAgeFiles)
                {
                    string deleteFailure = null;

                    bool isOldEnough = g.AgeDays > 90;

                    if (isOldEnough)
                    {
                        try
                        {
                            mgr.DeleteArchive(vaultName: "backup-home",
                                              archiveId: g.ArchiveId);
                        }
                        catch (Exception ex)
                        {
                            deleteFailure = ex.Message;
                        }
                    }
                    else
                    {
                        deleteFailure = string.Format("File is not old enough to be deleted. Age: [{0}]", g.AgeDays);
                    }

                    var deleteFileResult = new GlacierDeleteFileResult(g.NormalizedFilePath,
                                                                       deleteFailure,
                                                                       g.SizeBytes);

                    deleteFileResults.Add(deleteFileResult);
                }
            }

            var result = new DeleteGlacierFilesResult(deleteFileResults);

            ConsoleView.Show(result);
        }

        #endregion
    }
}
