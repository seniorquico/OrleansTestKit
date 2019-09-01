using System;
using System.Threading.Tasks;
using Orleans.Timers;

namespace Orleans.TestKit.Runtime
{
    internal sealed class TestTimerRegistry : ITimerRegistry
    {
        public IDisposable RegisterTimer(Grain grain, Func<object, Task> asyncCallback, object state, TimeSpan dueTime, TimeSpan period) =>
            new TestTimer();
    }
}
