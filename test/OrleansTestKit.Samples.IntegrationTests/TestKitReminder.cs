using System;
using Orleans.Runtime;

namespace Orleans.TestKit
{
    public sealed class TestKitReminder : IGrainReminder
    {
        public TimeSpan DueTime { get; set; }

        public TimeSpan Period { get; set; }

        public string ReminderName { get; set; }

        internal Guid ETag { get; set; }
    }
}
