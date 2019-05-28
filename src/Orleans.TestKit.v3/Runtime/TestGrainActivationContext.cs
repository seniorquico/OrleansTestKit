using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Core;
using Orleans.GrainDirectory;
using Orleans.Runtime;

namespace OrleansTestKit.Runtime
{
    internal sealed class TestGrainActivationContext : IDisposable, IGrainActivationContext
    {
        private IServiceScope serviceScope;

        public TestGrainActivationContext(Type grainType, IServiceProvider serviceProvider)
        {
            this.serviceScope = serviceProvider.CreateScope();
            this.GrainType = grainType ?? throw new ArgumentNullException(nameof(grainType));
        }

        public IServiceProvider ActivationServices =>
            this.serviceScope.ServiceProvider;

        public IGrainIdentity GrainIdentity =>
            throw new NotImplementedException();

        public Grain GrainInstance { get; internal set; }

        public Type GrainType { get; }

        public IDictionary<object, object> Items =>
            throw new NotImplementedException();

        public IGrainLifecycle ObservableLifecycle =>
            throw new NotImplementedException();

        public IMultiClusterRegistrationStrategy RegistrationStrategy =>
            throw new NotImplementedException();

        public void Dispose()
        {
            if (this.serviceScope != null)
            {
                this.serviceScope.Dispose();
                this.serviceScope = null;
            }
        }
    }
}
