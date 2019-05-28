using System;
using Orleans;
using Orleans.Core;
using Orleans.Runtime;
using Orleans.Timers;

namespace OrleansTestKit.Runtime
{
    internal sealed class TestGrainRuntime : IGrainRuntime
    {
        public TestGrainRuntime(IServiceProvider serviceProvider) =>
            this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        public IGrainFactory GrainFactory =>
            throw new NotImplementedException();

        public IReminderRegistry ReminderRegistry =>
            throw new NotImplementedException();

        public string ServiceId =>
            throw new NotImplementedException();

        public IServiceProvider ServiceProvider { get; }

        public SiloAddress SiloAddress =>
            throw new NotImplementedException();

        public string SiloIdentity =>
            throw new NotImplementedException();

        public ITimerRegistry TimerRegistry =>
            throw new NotImplementedException();

        public void DeactivateOnIdle(Grain grain) =>
            throw new NotImplementedException();

        public void DelayDeactivation(Grain grain, TimeSpan timeSpan) =>
            throw new NotImplementedException();

        public IStorage<TGrainState> GetStorage<TGrainState>(Grain grain) where TGrainState : new() =>
            throw new NotImplementedException();
    }
}
