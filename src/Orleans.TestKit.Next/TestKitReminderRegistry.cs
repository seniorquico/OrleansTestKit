using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Orleans.Runtime;
using Orleans.Timers;

namespace Orleans.TestKit
{
    internal sealed class TestKitReminderRegistry : IReminderRegistry
    {
        private readonly HashSet<Reminder> reminders = new HashSet<Reminder>();

        public Task<IGrainReminder?> GetReminder(string? reminderName) =>
            string.IsNullOrEmpty(reminderName)
                ? Task.FromException<IGrainReminder?>(new ArgumentNullException(nameof(reminderName)))
                : Task.FromResult<IGrainReminder?>(this.reminders.FirstOrDefault(reminder => reminder.ReminderName == reminderName));

        public Task<List<IGrainReminder>> GetReminders()
        {
            var reminders = this.reminders.ToList<IGrainReminder>();
            reminders.Sort((a, b) => string.CompareOrdinal(a.ReminderName, b.ReminderName));
            return Task.FromResult(reminders);
        }

        public Task<IGrainReminder> RegisterOrUpdateReminder(string? reminderName, TimeSpan dueTime, TimeSpan period)
        {
            if (string.IsNullOrEmpty(reminderName))
            {
                return Task.FromException<IGrainReminder>(new ArgumentNullException(nameof(reminderName)));
            }

            Reminder? reminder = this.reminders.FirstOrDefault(reminder => reminder.ReminderName == reminderName);
            if (reminder != null)
            {
                this.reminders.Remove(reminder);
            }

            reminder = new Reminder(reminderName);
            this.reminders.Add(reminder);
            return Task.FromResult<IGrainReminder>(reminder);
        }

        public Task UnregisterReminder(IGrainReminder? reminder)
        {
            if (reminder == null)
            {
                return Task.FromException(new ArgumentNullException(nameof(Reminder)));
            }

            Reminder? ourReminder = reminder as Reminder;
            if (ourReminder != null)
            {
                this.reminders.Remove(ourReminder);
            }

            return Task.CompletedTask;
        }

        [DebuggerStepThrough]
        private sealed class Reminder : IGrainReminder
        {
            public Reminder(string reminderName) =>
                this.ReminderName = reminderName;

            public string ReminderName { get; }
        }
    }
}
