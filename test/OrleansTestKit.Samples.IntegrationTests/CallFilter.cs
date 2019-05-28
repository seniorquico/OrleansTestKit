using System;
using System.Threading.Tasks;

namespace Orleans.TestKit
{
    public sealed class CallFilter : IOutgoingGrainCallFilter
    {
        private readonly IGrainFactory grainFactory;

        public CallFilter(IGrainFactory grainFactory) =>
            this.grainFactory = grainFactory ?? throw new ArgumentNullException(nameof(grainFactory));

        public Task Invoke(IOutgoingGrainCallContext context)
        {
            if ("Orleans.TestKit.Samples.Calls".Equals(context?.InterfaceMethod?.DeclaringType?.Namespace, StringComparison.Ordinal))
            {
            }

            return context.Invoke();
        }
    }
}
