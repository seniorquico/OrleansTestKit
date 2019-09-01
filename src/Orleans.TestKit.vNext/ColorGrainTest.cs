using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using NodaTime.Testing;
using Orleans.Core;
using Orleans.Runtime;
using Orleans.Timers;

namespace Orleans.TestKit.Test
{
    public sealed class ColorGrainTest
    {
        public void SetAndGetColor()
        {
            var grain = new ColorGrain();

            var runtime = new TestGrainRuntime();

            // IGrainRuntime grain.Runtime, IGrainIdentity grain.Identity
        }

        private sealed class FakeClockScheduler
        {
            private readonly FakeClock clock;
            private readonly TestTimerRegistry timerRegistry;
            private Dictionary<TestGrainTimer, Instant> last;

            public FakeClockScheduler(TestTimerRegistry timerRegistry, Instant initial)
                : this(timerRegistry, new FakeClock(initial))
            {
            }

            public FakeClockScheduler(TestTimerRegistry timerRegistry, FakeClock clock)
            {
                this.timerRegistry = timerRegistry ?? throw new ArgumentNullException(nameof(timerRegistry));
                this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
            }

            public Task AdvanceTime(DateTime now, bool invokeCallbacks = true)
            {
                foreach (var timer in this.timerRegistry.Timers)
                {
                    if (last == null || !last.TryGetValue(timer, out var lastTime))
                    {
                        lastTime = Instant.MinValue;
                    }

                    if (last != null)
                    {
                        ;
                    }
                }
            }
        }

        private sealed class TestGrainReminder : IGrainReminder
        {
            public TestGrainReminder(string reminderName, TimeSpan dueTime, TimeSpan period)
            {
                this.ReminderName = reminderName ?? throw new ArgumentNullException(nameof(reminderName));
                this.DueTime = dueTime;
                this.Period = period;
            }

            public TimeSpan DueTime { get; set; }

            public TimeSpan Period { get; set; }

            public string ReminderName { get; set; }
        }

        private sealed class TestGrainRuntime : IGrainRuntime
        {
            public IGrainFactory GrainFactory { get; }

            public TestReminderRegistry ReminderRegistry { get; } = new TestReminderRegistry();

            public string ServiceId { get; }

            public IServiceProvider ServiceProvider { get; }

            public SiloAddress SiloAddress { get; }

            public string SiloIdentity { get; }

            public TestTimerRegistry TimerRegistry { get; } = new TestTimerRegistry();

            IReminderRegistry IGrainRuntime.ReminderRegistry => this.ReminderRegistry;

            ITimerRegistry IGrainRuntime.TimerRegistry => this.TimerRegistry;

            public void DeactivateOnIdle(Grain grain) =>
                throw new NotImplementedException();

            public void DelayDeactivation(Grain grain, TimeSpan timeSpan) =>
                throw new NotImplementedException();

            public IStorage<TGrainState> GetStorage<TGrainState>(Grain grain) where TGrainState : new() =>
                throw new NotImplementedException();
        }

        private sealed class TestReminderRegistry : IReminderRegistry
        {
            private readonly Dictionary<string, TestGrainReminder> reminders = new Dictionary<string, TestGrainReminder>();

            public Task<IGrainReminder> GetReminder(string reminderName)
            {
                this.reminders.TryGetValue(reminderName, out var reminder);
                return Task.FromResult<IGrainReminder>(reminder);
            }

            public Task<List<IGrainReminder>> GetReminders() =>
                Task.FromResult(this.reminders.Values.ToList<IGrainReminder>());

            public Task<IGrainReminder> RegisterOrUpdateReminder(string reminderName, TimeSpan dueTime, TimeSpan period)
            {
                var reminder = new TestGrainReminder(reminderName, dueTime, period);
                this.reminders[reminderName] = reminder;
                return Task.FromResult<IGrainReminder>(reminder);
            }

            public Task UnregisterReminder(IGrainReminder reminder)
            {
                this.reminders.Remove(reminder.ReminderName);
                return Task.CompletedTask;
            }
        }
    }
}
