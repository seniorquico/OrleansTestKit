using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans.Runtime;
using Orleans.Timers;

namespace Orleans.TestKit
{
    public sealed class TestKitReminderRegistry : IReminderRegistry
    {
        private const uint MaxTimeSpan = 0xfffffffe;

        private const uint MinPeriod = 60000;

        private static readonly Task<IGrainReminder> NullCompletedTask = Task.FromResult<IGrainReminder>(null);

        private readonly Dictionary<string, IGrainReminder> reminders = new Dictionary<string, IGrainReminder>();

        public Task<IGrainReminder> GetReminder(string reminderName)
        {
            if (reminderName == null)
            {
                throw new ArgumentNullException(nameof(reminderName));
            }

            if (reminderName.Length == 0)
            {
                throw new ArgumentException("The reminder name must not be empty.", nameof(reminderName));
            }

            IGrainReminder reminder;
            lock (this)
            {
                if (!this.reminders.TryGetValue(reminderName, out reminder))
                {
                    reminder = null;
                }
            }

            return reminder == null
                ? NullCompletedTask
                : Task.FromResult(reminder);
        }

        public Task<List<IGrainReminder>> GetReminders()
        {
            List<IGrainReminder> reminders;
            lock (this)
            {
                reminders = this.reminders.Values.ToList();
            }

            reminders.Sort((a, b) => a.ReminderName.CompareTo(b.ReminderName));
            return Task.FromResult(reminders);
        }

        public Task<IGrainReminder> RegisterOrUpdateReminder(string reminderName, TimeSpan dueTime, TimeSpan period)
        {
            if (reminderName == null)
            {
                throw new ArgumentNullException(nameof(reminderName));
            }

            if (reminderName.Length == 0)
            {
                throw new ArgumentException("The reminder name must not be empty.", nameof(reminderName));
            }

            var dueTimeMilliseconds = (long)dueTime.TotalMilliseconds;
            if (dueTimeMilliseconds < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(dueTime), "The due time must not be negative.");
            }

            if (dueTimeMilliseconds > MaxTimeSpan)
            {
                throw new ArgumentOutOfRangeException(nameof(dueTime), $"The due time must be less than or equal to {MaxTimeSpan}ms.");
            }

            var periodMilliseconds = (long)period.TotalMilliseconds;
            if (periodMilliseconds < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(period), "The period must not be nagative.");
            }

            if (periodMilliseconds > MaxTimeSpan)
            {
                throw new ArgumentOutOfRangeException(nameof(period), $"The period must be less than or equal to {MaxTimeSpan}ms.");
            }

            if (periodMilliseconds < MinPeriod)
            {
                throw new ArgumentOutOfRangeException(nameof(period), $"The period must be greater than or equal to {MinPeriod}ms.");
            }

            IGrainReminder reminder;
            lock (this)
            {
                this.reminders[reminderName] = reminder = new TestKitReminder
                {
                    DueTime = dueTime,
                    ETag = Guid.NewGuid(),
                    Period = period,
                    ReminderName = reminderName,
                };
            }

            return Task.FromResult(reminder);
        }

        public Task UnregisterReminder(IGrainReminder reminder)
        {
            if (reminder == null)
            {
                throw new ArgumentNullException(nameof(reminder));
            }

            if (!(reminder is TestKitReminder theirReminder) || string.IsNullOrEmpty(theirReminder.ReminderName))
            {
                return Task.CompletedTask;
            }

            lock (this)
            {
                if (!this.reminders.TryGetValue(theirReminder.ReminderName, out var o))
                {
                    return Task.FromException(new ReminderException("The reminder does not exist."));
                }

                var ourReminder = (TestKitReminder)o;
                if (ourReminder == null || ourReminder.ETag != theirReminder.ETag)
                {
                    return Task.FromException(new ReminderException("The reminder versions do not match."));
                }

                this.reminders.Remove(theirReminder.ReminderName);
            }

            return Task.CompletedTask;
        }
    }
}
