using System.Threading.Tasks;
using Orleans.Runtime;
using Orleans.TestingHost;
using Xunit;

namespace Orleans.TestKit
{
    public class BaseClusterFixture<TSiloBuilderConfigurator> : IAsyncLifetime
        where TSiloBuilderConfigurator : ISiloBuilderConfigurator, new()
    {
        public BaseClusterFixture()
        {
            var builder = new TestClusterBuilder();
            builder.AddSiloBuilderConfigurator<TSiloBuilderConfigurator>();

            this.Cluster = builder.Build();
        }

        public TestCluster Cluster { get; }

        public Task DisposeAsync() =>
            this.Cluster.StopAllSilosAsync();

        public Task FireReminderAsync(IAddressable grain, string reminderName, TickStatus status) =>
            grain.FireReminderAsync(reminderName, status);

        public Task InitializeAsync() =>
            this.Cluster.DeployAsync();
    }
}
