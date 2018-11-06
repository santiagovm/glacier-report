using System;
using System.Collections.Generic;
using Glacier.Tools.CLI.Commands;
using NDesk.Options;

namespace Glacier.Tools.CLI
{
    internal class CliParser
    {
        public static ICommand Parse(IEnumerable<string> args)
        {
            string backupLocation = null;

            bool showHelp = false;

            string vaultInventoryFile = null;

            string nasPath = null;

            bool showGlacierExtra = false;

            TimeSpan glacierAge = TimeSpan.Zero;

            bool deleteGlacierFiles = false;

            bool requestGlacierInventory =false;

            bool downloadGlacierInventory = false;

            string jobId = null;

            var p = new OptionSet
                        {
                            {
                                "s|summary=", "backup location summary. Options: [{glacier | nas}]",
                                x => backupLocation = x
                            },
                            {
                                "v|vaultInventoryFile=", "glacier vault inventory file location",
                                x => vaultInventoryFile = x
                            },
                            {
                                "n|nasPath=", "NAS path",
                                x => nasPath = x
                            },
                            {
                                "ge|glacierExtra", "show files in glacier that are not in the nas",
                                x => showGlacierExtra = x != null
                            },
                            {
                                "ga|glacierAge=", "show files in glacier that are older than {age} (use timespan notation)",
                                x => glacierAge = TimeSpan.Parse(x)
                            },
                            {
                                "gd|glacierDelete", "delete files in glacier",
                                x => deleteGlacierFiles = x != null
                            },
                            {
                                "rgi|requestGlacierInventory", "request glacier inventory",
                                x => requestGlacierInventory = x != null
                            },
                            {
                                "dgi|downloadGlacierInventory", "download glacier inventory",
                                x => downloadGlacierInventory = x != null
                            },
                            {
                                "j|jobId=", "inventory retrieval job id",
                                x => jobId = x
                            },
                            {
                                "h|help", "show this message and exit",
                                x => showHelp = x != null
                            }
                        };

            try
            {
                List<string> extra = p.Parse(args);

                if (extra.Count > 0)
                {
                    ConsoleView.Show(p, extra);
                    return null;
                }
            }
            catch (OptionException e)
            {
                ConsoleView.Show(e);
                return null;
            }

            if (showHelp)
            {
                ConsoleView.Show(p);
                return null;
            }

            if (requestGlacierInventory)
            {
                return new RequestGlacierInventory();
            }

            if (downloadGlacierInventory)
            {
                if (string.IsNullOrWhiteSpace(jobId))
                {
                    ConsoleView.Show(p, "Job Id is missing Use option -j");
                    return null;
                }

                if (string.IsNullOrWhiteSpace(vaultInventoryFile))
                {
                    ConsoleView.Show(p, "Vault inventory file is missing Use option -v");
                    return null;
                }

                return new DownloadGlacierInventory(jobId, vaultInventoryFile);
            }

            if (String.Compare(backupLocation, "glacier", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (string.IsNullOrWhiteSpace(vaultInventoryFile))
                {
                    ConsoleView.Show(p, "Vault inventory file is missing Use option -v");
                    return null;
                }

                return new GetGlacierSummary(vaultInventoryFile);
            }

            if (String.Compare(backupLocation, "nas", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (string.IsNullOrWhiteSpace(nasPath))
                {
                    ConsoleView.Show(p, "NAS path is missing Use option -n");
                    return null;
                }

                return new GetNasSummary(nasPath);
            }

            if (showGlacierExtra)
            {
                if (string.IsNullOrWhiteSpace(vaultInventoryFile))
                {
                    ConsoleView.Show(p, "Vault inventory file is missing Use option -v");
                    return null;
                }

                if (string.IsNullOrWhiteSpace(nasPath))
                {
                    ConsoleView.Show(p, "NAS path is missing Use option -n");
                    return null;
                }

                if ( glacierAge == TimeSpan.Zero)
                {
                    return new GetGlacierExtra(vaultInventoryFile, nasPath);
                }

                if (deleteGlacierFiles)
                {
                    return new DeleteExtraGlacierFiles(vaultInventoryFile,
                                                       nasPath,
                                                       glacierOlderThan: glacierAge);
                }

                return new GetGlacierExtraAge(vaultInventoryFile, nasPath, glacierAge);
            }

            ConsoleView.Show(p, "Invalid option");
            return null;
        }
    }
}
