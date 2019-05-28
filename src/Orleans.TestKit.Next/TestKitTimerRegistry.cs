using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Orleans.Runtime;
using Orleans.Timers;

namespace Orleans.TestKit
{
    internal sealed class TestKitTimerRegistry : ITimerRegistry
    {
        private readonly Dictionary<Guid, Timer> timers = new Dictionary<Guid, Timer>();

        public IDisposable RegisterTimer(Grain grain, Func<object, Task> asyncCallback, object state, TimeSpan dueTime, TimeSpan period)
        {
            var timer = new Timer(this, Guid.NewGuid(), grain.GrainReference, asyncCallback.Method, state, dueTime, period);
            this.timers[timer.TimerId] = timer;
            return timer;
        }

        private void UnregisterTimer(Timer timer) =>
            this.timers.Remove(timer.TimerId);

        [DebuggerStepThrough]
        private sealed class Timer : IDisposable
        {
            private readonly TestKitTimerRegistry timerRegistry;

            public Timer(TestKitTimerRegistry timerRegistry, Guid timerId, GrainReference grain, MethodInfo method, object state, TimeSpan dueTime, TimeSpan period)
            {
                this.DueTime = dueTime;
                this.Grain = grain;
                this.Method = method;
                this.Period = period;
                this.State = state;
                this.TimerId = timerId;
                this.timerRegistry = timerRegistry;
            }

            public TimeSpan DueTime { get; }

            public GrainReference Grain { get; }

            public MethodInfo Method { get; }

            public TimeSpan Period { get; }

            public object State { get; }

            public Guid TimerId { get; }

            public void Dispose() =>
                this.timerRegistry.UnregisterTimer(this);
        }
    }
}
