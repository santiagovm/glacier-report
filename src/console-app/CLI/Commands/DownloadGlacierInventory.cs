using System;
using System.IO;
using Amazon;
using Amazon.Glacier.Transfer;

namespace Glacier.Tools.CLI.Commands
{
    internal class DownloadGlacierInventory : ICommand
    {
        readonly string _jobId;
        readonly string _vaultInventoryFile;
        
        public DownloadGlacierInventory(string jobId, string vaultInventoryFile)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                throw new ArgumentNullException("jobId");
            }

            if (string.IsNullOrWhiteSpace(vaultInventoryFile))
            {
                throw new ArgumentNullException("vaultInventoryFile");
            }

            _jobId = jobId;
            _vaultInventoryFile = vaultInventoryFile;
        }

        #region Implementation of ICommand

        public void Execute()
        {
            bool fileExists = File.Exists(_vaultInventoryFile);

            if (fileExists)
            {
                string errorMessage = string.Format("Can't download inventory. File already exists, use a different name [{0}]",
                                                    _vaultInventoryFile);

                throw new InvalidOperationException(errorMessage);
            }

            using (var mgr = new ArchiveTransferManager(RegionEndpoint.USWest2))
            {
                mgr.DownloadJob(vaultName: "backup-home",
                                jobId: _jobId,
                                filePath: _vaultInventoryFile);
            }

            string result = string.Format("Inventory saved at [{0}]", _vaultInventoryFile);

            ConsoleView.Show(result);
        }

        #endregion
    }
}
