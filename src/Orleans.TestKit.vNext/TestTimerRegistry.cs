using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans.Timers;

namespace Orleans.TestKit
{
    public sealed class TestTimerRegistry
    {
        private readonly List<TestGrainTimer> timers = new List<TestGrainTimer>();

        private readonly object timersLock = new object();

        public IList<TestGrainTimer> Timers
        {
            get
            {
                lock (this.timersLock)
                {
                    return new List<TestGrainTimer>(this.timers);
                }
            }
        }

        public TestGrainTimer AddTimer(Func<object, Task> asyncCallback, object state, TimeSpan dueTime, TimeSpan period)
        {
            if (asyncCallback == null)
            {
                throw new ArgumentNullException(nameof(asyncCallback));
            }

            lock (this.timersLock)
            {
            }
        }

        public IDisposable RegisterTimer(
            Grain grain,
            Func<object, Task> asyncCallback,
            object state,
            TimeSpan dueTime,
            TimeSpan period)
        {
            var timer = new TestGrainTimer(asyncCallback, state, dueTime, period);
            lock (this.timersLock)
            {
                this.timers.Add(timer);
            }

            return new TestGrainTimerDisposable(this, timer);
        }

        public void RemoveTimer(TestGrainTimer timer)
        {
        }

        private sealed class TestGrainTimerWrapper : IDisposable
        {
            private readonly TestGrainTimer timer;

            private readonly TestTimerRegistry timerRegistry;

            public TestGrainTimerDisposable(TestTimerRegistry timerRegistry, TestGrainTimer timer)
            {
                this.timerRegistry = timerRegistry ?? throw new ArgumentNullException(nameof(timerRegistry));
                this.timer = timer ?? throw new ArgumentNullException(nameof(timer));
            }

            public void Dispose() =>
                this.timerRegistry.timers.Remove(this.timer);
        }

        private sealed class TestTimerRegistry : ITimerRegistry
        {
            public IDisposable RegisterTimer(Grain grain, Func<object, Task> asyncCallback, object state, TimeSpan dueTime, TimeSpan period) => throw new NotImplementedException();
        }
    }
}
