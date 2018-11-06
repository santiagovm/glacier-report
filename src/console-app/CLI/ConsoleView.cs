using System;
using System.Collections.Generic;
using System.Linq;
using Glacier.Tools.Entities;
using NDesk.Options;

namespace Glacier.Tools.CLI
{
    internal class ConsoleView
    {
        public static void Show(string message)
        {
            ShowInfo(new[] { message });
        }

        public static void Show(Exception ex)
        {
            ShowError(new[]
                          {
                              "",
                              "*** ERROR ***",
                              ex.Message,
                              "",
                              ex.StackTrace
                          });
        }

        public static void Show(OptionException ex)
        {
            ShowError(new[]
                          {
                              "",
                              "*** ERROR ***",
                              "Try --help for more information.",
                              "",
                              ex.Message
                          });
        }

        public static void Show(OptionSet optionSet)
        {
            Console.WriteLine();
            Console.WriteLine("Usage: console-app [OPTIONS]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            optionSet.WriteOptionDescriptions(Console.Out);
        }

        public static void Show(OptionSet optionSet, string message)
        {
            Console.WriteLine();
            Console.WriteLine("{0}", message);
            Console.WriteLine();
            Console.WriteLine("Usage: console-app [OPTIONS]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            optionSet.WriteOptionDescriptions(Console.Out);
        }

        public static void Show(OptionSet optionSet, IEnumerable<string> extra)
        {
            string message = extra.Aggregate(seed: "Invalid option\r\n",
                                             func: (current, s) => string.Format("{0} {1}", current, s));

            Show(optionSet, message);
        }

        public static void Show(VaultInventory inventory)
        {
            ShowInfo(new[]
                         {
                             "",
                             string.Format("         Vault ARN: {0}", inventory.VaultArn),
                             string.Format("    Inventory Date: {0}", inventory.InventoryDate),
                             string.Format("Archive List Count: {0:n0}", inventory.ArchiveList.Count())
                         });
        }

        public static void Show(NasFiles nasFiles)
        {
            ShowInfo(new[]
                         {
                             "",
                             string.Format("   NAS path: {0}", nasFiles.Path),
                             string.Format("Files Count: {0:n0}", nasFiles.Files.Count())
                         });
        }

        static void ShowError(IEnumerable<string> messages)
        {
            Show(messages, ConsoleColor.Red);
        }

        static void ShowInfo(IEnumerable<string> messages)
        {
            Show(messages, ConsoleColor.DarkGreen);
        }

        static void Show(IEnumerable<string> messages, ConsoleColor consoleColor)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = consoleColor;

            foreach (string m in messages)
            {
                Console.WriteLine(m);
            }

            Console.ForegroundColor = defaultColor;
        }

        public static void Show(GlacierExtraResult result)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkGreen;

            Console.WriteLine();
            Console.WriteLine("  Glacier Extra Files Count: {0:n0}", result.GlacierExtraFilesCount);
            Console.WriteLine("        Glacier Files Count: {0:n0}", result.GlacierFilesCount);
            Console.WriteLine("   Glacier Extra Files Size: {0}", result.GlacierExtraFilesSizeBytes.ToFriendlyFileSize());
            Console.WriteLine("Glacier Extra Files $/month: {0}", result.GlacierExtraFilesSizeBytes.ToCost());
            Console.WriteLine("            NAS Files Count: {0:n0}", result.NasFilesCount);
            Console.WriteLine();
            Console.WriteLine("Glacier Extra Files:");
            Console.WriteLine();

            IEnumerable<GlacierFile> extraFiles = result.GlacierExtraFiles;

            foreach (GlacierFile g in extraFiles)
            {
                Console.WriteLine("{0}", g.NormalizedFilePath);
            }

            Console.WriteLine();
            Console.WriteLine("  Glacier Extra Files Count: {0:n0}", result.GlacierExtraFilesCount);
            Console.WriteLine("        Glacier Files Count: {0:n0}", result.GlacierFilesCount);
            Console.WriteLine("   Glacier Extra Files Size: {0}", result.GlacierExtraFilesSizeBytes.ToFriendlyFileSize());
            Console.WriteLine("Glacier Extra Files $/month: {0}", result.GlacierExtraFilesSizeBytes.ToCost());
            Console.WriteLine("            NAS Files Count: {0:n0}", result.NasFilesCount);

            Console.ForegroundColor = defaultColor;
        }

        public static void Show(GlacierExtraAgeResult result)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkGreen;

            Console.WriteLine();
            Console.WriteLine("  Files Count: {0:n0}", result.FilesCount);
            Console.WriteLine("   Files Size: {0}", result.FilesSizeBytes.ToFriendlyFileSize());
            Console.WriteLine("Files $/month: {0}", result.FilesSizeBytes.ToCost());
            Console.WriteLine();

            IEnumerable<GlacierFile> extraFiles = result.Files;

            Console.WriteLine(" Age |   Date   |                  Path");
            Console.WriteLine("-".PadRight(totalWidth: 78, paddingChar: '-'));

            foreach (GlacierFile g in extraFiles)
            {
                Console.WriteLine("{0,4} | {1:MM/dd/yy} | {2}",
                                  g.AgeDays,
                                  g.DateArchived,
                                  g.NormalizedFilePath.FixedWidth(60));
            }

            Console.WriteLine();
            Console.WriteLine("  Files Count: {0:n0}", result.FilesCount);
            Console.WriteLine("   Files Size: {0}", result.FilesSizeBytes.ToFriendlyFileSize());
            Console.WriteLine("Files $/month: {0}", result.FilesSizeBytes.ToCost());

            Console.ForegroundColor = defaultColor;
        }


        public static void Show(DeleteGlacierFilesResult result)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkGreen;

            Console.WriteLine();
            Console.WriteLine("    Deleted Files Count: {0:n0}", result.DeletedFilesCount);
            Console.WriteLine("     Deleted Files Size: {0}", result.DeletedFilesSizeBytes.ToFriendlyFileSize());
            Console.WriteLine("  Deleted Files $/month: {0}", result.DeletedFilesSizeBytes.ToCost());
            Console.WriteLine("Deletion Failures Count: {0:n0}", result.DeletionFailuresCount);
            
            Console.WriteLine();
            Console.WriteLine("Path | Result | Failure Details");
            Console.WriteLine("-".PadRight(totalWidth: 78, paddingChar: '-'));

            IEnumerable<GlacierDeleteFileResult> files = result.DeletedFiles;

            foreach (GlacierDeleteFileResult g in files)
            {
                Console.WriteLine("{1,-6} | {0} | {2}",
                                  g.NormalizedFilePath,
                                  g.DeleteFailed ? "FAILED" : "OK",
                                  g.DeleteFailure);
            }

            Console.WriteLine();
            Console.WriteLine("    Deleted Files Count: {0:n0}", result.DeletedFilesCount);
            Console.WriteLine("     Deleted Files Size: {0}", result.DeletedFilesSizeBytes.ToFriendlyFileSize());
            Console.WriteLine("  Deleted Files $/month: {0}", result.DeletedFilesSizeBytes.ToCost());
            Console.WriteLine("Deletion Failures Count: {0:n0}", result.DeletionFailuresCount);

            Console.ForegroundColor = defaultColor;
        }
    }
}
