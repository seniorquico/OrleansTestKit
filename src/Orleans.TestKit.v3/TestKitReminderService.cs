using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans.Runtime;

namespace Orleans.TestKit
{
    public sealed class TestKitReminderService : IReminderService
    {
        public TestKitReminderService(GrainReference grainRef)
        {
            this.grainRef = grainRef;
        }

        public Task<IGrainReminder> GetReminder(GrainReference grainRef, string reminderName)
        {
        }

        public Task<List<IGrainReminder>> GetReminders(GrainReference grainRef) => throw new NotImplementedException();

        public Task<IGrainReminder> RegisterOrUpdateReminder(GrainReference grainRef, string reminderName, TimeSpan dueTime, TimeSpan period) => throw new NotImplementedException();

        public Task Start() => throw new NotImplementedException();

        public Task Stop() => throw new NotImplementedException();

        public Task UnregisterReminder(IGrainReminder reminder) => throw new NotImplementedException();
    }
}
