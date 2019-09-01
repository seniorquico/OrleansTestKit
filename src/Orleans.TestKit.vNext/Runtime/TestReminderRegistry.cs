using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans.Runtime;
using Orleans.Timers;

namespace Orleans.TestKit.Runtime
{
    internal sealed class TestReminderRegistry : IReminderRegistry
    {
        public Task<IGrainReminder> GetReminder(string reminderName) =>
            Task.FromResult<IGrainReminder>(null);

        public Task<List<IGrainReminder>> GetReminders() =>
            Task.FromResult(new List<IGrainReminder>());

        public Task<IGrainReminder> RegisterOrUpdateReminder(string reminderName, TimeSpan dueTime, TimeSpan period) =>
            Task.FromResult<IGrainReminder>(new TestReminder());

        public Task UnregisterReminder(IGrainReminder reminder) =>
            Task.CompletedTask;
    }
}
