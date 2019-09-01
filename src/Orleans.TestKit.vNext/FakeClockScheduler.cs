using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using NodaTime.Testing;

namespace Orleans.TestKit
{
    public sealed class FakeClockScheduler
    {
        private readonly FakeClock clock;

        private readonly TestTimerRegistry timerRegistry;

        public FakeClockScheduler(TestTimerRegistry timerRegistry, Instant initial)
                : this(timerRegistry, new FakeClock(initial))
        {
        }

        public FakeClockScheduler(TestTimerRegistry timerRegistry, FakeClock clock)
        {
            this.timerRegistry = timerRegistry ?? throw new ArgumentNullException(nameof(timerRegistry));
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        public Task AdvanceAsync(Duration duration)
        {
            this.clock.Advance(duration);
            return this.FireDueTimersAsync();
        }

        public Task AdvanceDaysAsync(int days)
        {
            this.clock.AdvanceDays(days);
            return this.FireDueTimersAsync();
        }

        public Task AdvanceHoursAsync(int hours)
        {
            this.clock.AdvanceHours(hours);
            return this.FireDueTimersAsync();
        }

        public Task AdvanceMillisecondsAsync(long milliseconds)
        {
            this.clock.AdvanceMilliseconds(milliseconds);
            return this.FireDueTimersAsync();
        }

        public Task AdvanceMinutesAsync(long minutes)
        {
            this.clock.AdvanceMinutes(minutes);
            return this.FireDueTimersAsync();
        }

        public Task AdvanceNanosecondsAsync(long nanoseconds)
        {
            this.clock.AdvanceNanoseconds(nanoseconds);
            return this.FireDueTimersAsync();
        }

        public Task AdvanceSecondsAsync(long seconds)
        {
            this.clock.AdvanceSeconds(seconds);
            return this.FireDueTimersAsync();
        }

        public Task AdvanceTicksAsync(long ticks)
        {
            this.clock.AdvanceTicks(ticks);
            return this.FireDueTimersAsync();
        }

        private Task FireDueTimersAsync()
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
}
