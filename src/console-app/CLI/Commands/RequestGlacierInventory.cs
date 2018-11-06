using Amazon;
using Amazon.Glacier;
using Amazon.Glacier.Model;

namespace Glacier.Tools.CLI.Commands
{
    internal class RequestGlacierInventory : ICommand
    {
        #region Implementation of ICommand

        public void Execute()
        {
            var initJobRequest = new InitiateJobRequest
                                     {
                                         VaultName = "backup-home",

                                         JobParameters = new JobParameters
                                                             {
                                                                 Type = "inventory-retrieval"
                                                             }
                                     };

            using (AmazonGlacierClient client = new AmazonGlacierClient(RegionEndpoint.USWest2))
            {
                InitiateJobResponse initJobResponse = client.InitiateJob(initJobRequest);

                string jobId = initJobResponse.JobId;

                ConsoleView.Show("Inventory requested");
                ConsoleView.Show(string.Format("JobId: [{0}]", jobId));
            }
        }

        #endregion
    }
}
