using System;
using Glacier.Tools.Entities;

namespace Glacier.Tools.CLI.Commands
{
    internal class GetGlacierSummary : ICommand
    {
        readonly string _vaultInventoryFile;

        public GetGlacierSummary(string vaultInventoryFile)
        {
            if (string.IsNullOrWhiteSpace(vaultInventoryFile))
            {
                throw new ArgumentNullException("vaultInventoryFile");
            }

            _vaultInventoryFile = vaultInventoryFile;
        }

        #region Implementation of ICommand

        public void Execute()
        {
            VaultInventory inventory = VaultInventoryParser.Parse(_vaultInventoryFile);

            ConsoleView.Show(inventory);
        }

        #endregion
    }
}
