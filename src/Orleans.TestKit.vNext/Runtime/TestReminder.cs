using Orleans.Runtime;

namespace Orleans.TestKit.Runtime
{
    internal sealed class TestReminder : IGrainReminder
    {
        public string ReminderName { get; }
    }
}
