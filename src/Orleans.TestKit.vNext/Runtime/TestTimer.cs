using System;
using System.Threading.Tasks;

namespace Orleans.TestKit.Runtime
{
    internal sealed class TestTimer : IDisposable
    {
        private bool disposed;

        /// <summary>Initializes a new instance of the <see cref="TestGrainTimer"/> class.</summary>
        /// <param name="asyncCallback"></param>
        /// <param name="state"></param>
        /// <param name="dueTime"></param>
        /// <param name="period"></param>
        /// <exception cref="ArgumentNullException">If <paramref name="asyncCallback"/> is <c>null</c>.</exception>
        internal TestTimer(Func<object, Task> asyncCallback, object state, TimeSpan dueTime, TimeSpan period)
        {
            this.AsyncCallback = asyncCallback ?? throw new ArgumentNullException(nameof(asyncCallback));
            this.State = state;
            this.DueTime = dueTime;
            this.Period = period;
        }

        public Func<object, Task> AsyncCallback { get; }

        public TimeSpan DueTime { get; }

        public TimeSpan Period { get; }

        public object State { get; }

        public void Dispose()
        {
            this.disposed = true;
        }

        public Task Fire() =>
            this.AsyncCallback(this.State);
    }
}
