using System;
using System.Diagnostics;
using Orleans.Concurrency;

namespace Orleans.TestKit
{
    /// <summary>Represents a grain reminder.</summary>
    [DebuggerStepThrough]
    [Immutable]
    public sealed class TestKitReminder
    {
        /// <summary>Initializes a new instance of the <see cref="TestKitReminder"/> class.</summary>
        /// <param name="reminderName">The reminder name.</param>
        /// <param name="dueTime">The time interval before the initial call to the grain.</param>
        /// <param name="period">The time interval between subsequent calls to the grain.</param>
        internal TestKitReminder(string reminderName, TimeSpan dueTime, TimeSpan period)
        {
            this.DueTime = dueTime;
            this.Period = period;
            this.ReminderName = reminderName;
        }

        /// <summary>Gets the time interval before the initial call to the grain.</summary>
        public TimeSpan DueTime { get; }

        /// <summary>Gets the timer interval between subsequent calls to the grain.</summary>
        public TimeSpan Period { get; }

        /// <summary>Gets the reminder name.</summary>
        public string ReminderName { get; }
    }
}
