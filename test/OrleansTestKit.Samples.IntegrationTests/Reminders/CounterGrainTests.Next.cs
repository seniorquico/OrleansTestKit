using System;
using System.Threading.Tasks;
using Orleans.Hosting;
using Orleans.Runtime;
using Orleans.TestingHost;
using Xunit;

namespace Orleans.TestKit.Samples.Reminders
{
    public sealed class CounterGrainTests :
        BaseClusterFixture<CounterGrainTests.SiloBuilderConfigurator>
    {
        [Fact]
        public async Task CreateReminder()
        {
            // Arrange
            var grainId = Guid.NewGuid();

            // Act
            var grain = this.Cluster.GrainFactory.GetGrain<ICounterGrain>(grainId);

            // Assert
            var value = await grain.GetValueAsync();
            Assert.Equal(0, value);
        }

        [Fact]
        public async Task FireReminder()
        {
            // Arrange
            var grainId = Guid.NewGuid();
            var grain = this.Cluster.GrainFactory.GetGrain<ICounterGrain>(grainId);

            // Act
            await this.FireReminderAsync(grain, "Increment", new TickStatus());

            // Assert
            var value = await grain.GetValueAsync();
            Assert.Equal(1, value);
        }

        public sealed class SiloBuilderConfigurator : ISiloBuilderConfigurator
        {
            public void Configure(ISiloHostBuilder builder) =>
                builder.UseTestKitReminderService();
        }
    }
}
