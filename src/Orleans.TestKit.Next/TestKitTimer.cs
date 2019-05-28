using System;
using System.Diagnostics;
using System.Reflection;
using Orleans.Concurrency;
using Orleans.Runtime;

namespace Orleans.TestKit
{
    /// <summary>Represents a grain timer.</summary>
    [DebuggerStepThrough]
    [Immutable]
    public sealed class TestKitTimer
    {
        /// <summary>Initializes a new instance of the <see cref="TestKitTimer"/> class.</summary>
        internal TestKitTimer(Guid timerId, GrainReference grain, MethodInfo method, object state, TimeSpan dueTime, TimeSpan period)
        {
            this.DueTime = dueTime;
            this.Grain = grain;
            this.Method = method;
            this.Period = period;
            this.State = state;
            this.TimerId = timerId;
        }

        public TimeSpan DueTime { get; }

        public GrainReference Grain { get; }

        public MethodInfo Method { get; }

        public TimeSpan Period { get; }

        public object State { get; }

        internal Guid TimerId { get; }
    }
}
