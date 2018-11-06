using System;
using Glacier.Tools.Entities;

namespace Glacier.Tools.CLI.Commands
{
    internal class GetNasSummary : ICommand
    {
        readonly string _nasPath;

        public GetNasSummary(string nasPath)
        {
            if (string.IsNullOrWhiteSpace(nasPath))
            {
                throw new ArgumentNullException("nasPath");
            }

            _nasPath = nasPath;
        }

        #region Implementation of ICommand

        public void Execute()
        {
            NasFiles nasFiles = NasBackup.GetFiles(_nasPath);

            ConsoleView.Show(nasFiles);
        }

        #endregion
    }
}
