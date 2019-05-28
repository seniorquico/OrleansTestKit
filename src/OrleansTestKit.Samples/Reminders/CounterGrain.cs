using System;
using System.Threading.Tasks;
using Orleans.Runtime;

namespace Orleans.TestKit.Samples.Reminders
{
    public sealed class CounterGrain : Grain, ICounterGrain, IRemindable
    {
        private const string IncrementReminderName = "Increment";

        private int value;

        public Task<int> GetValueAsync() =>
            Task.FromResult(this.value);

        public override Task OnActivateAsync() =>
            this.RegisterOrUpdateReminder(IncrementReminderName, TimeSpan.FromDays(1), TimeSpan.FromDays(1));

        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            var reminder = await this.GetReminder(reminderName);
            switch (reminder.ReminderName)
            {
                case IncrementReminderName:
                    this.value++;
                    break;

                default:
                    await this.UnregisterReminder(reminder);
                    break;
            }
        }
    }
}
