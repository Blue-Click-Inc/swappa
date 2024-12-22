using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using Newtonsoft.Json;

namespace Swappa.Server.Filters
{
    public class HangfireLogAttribute : JobFilterAttribute, IClientFilter, IServerFilter, IElectStateFilter, IApplyStateFilter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        // IClient
        public void OnCreated(CreatedContext context)
        {
            Logger.InfoFormat($"Background job created based on method: {context.Job.Method.Name}");
        }

        //IClient
        public void OnCreating(CreatingContext context)
        {
            Logger.InfoFormat($"Creating background job based on method: {context.Job.Method?.Name}");
        }

        // IServer
        public void OnPerforming(PerformingContext context)
        {
            Logger.InfoFormat($"Starting job: {context.BackgroundJob.Id}...");
        }

        // IServer
        public void OnPerformed(PerformedContext context)
        {
            Logger.InfoFormat($"Job {context.BackgroundJob.Id} completed successfully.");
        }

        // IApplyState
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            Logger.InfoFormat($"Job {context.BackgroundJob.Id} state changed from {context.OldStateName} to {context.NewState.Name}");
        }

        // IApplyState
        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            Logger.InfoFormat($"Job {context.BackgroundJob.Id} state {context.OldStateName} was unapplied.");
        }

        // IElectState
        public void OnStateElection(ElectStateContext context)
        {
            var failedState = context.CandidateState as FailedState;
            if (failedState != null)
            {
                var json = JsonConvert.SerializeObject(failedState.Exception, Formatting.Indented);
                Logger.ErrorFormat($"Job {context.BackgroundJob.Id} failed due to an exception", json);
            }
        }
    }
}
