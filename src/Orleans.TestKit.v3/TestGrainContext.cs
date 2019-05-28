using System;
using System.Collections.Generic;
using System.Text;
using Orleans.Runtime;

namespace Orleans.TestKit
{
    public sealed class TestGrainContext<TGrain> where TGrain : Grain
    {
        public TGrain Grain { get; }

        public GrainReference GrainReference { get; }
    }
}
