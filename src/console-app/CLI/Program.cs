using System;

namespace Glacier.Tools.CLI
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ICommand cmd = CliParser.Parse(args);

                if (cmd == null)
                {
                    return;
                }

                ConsoleView.Show("executing...");

                cmd.Execute();
            }
            catch (Exception ex)
            {
                ConsoleView.Show(ex);
            }
        }
    }
}
