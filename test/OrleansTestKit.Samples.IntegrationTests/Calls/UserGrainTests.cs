//using System;
//using System.Threading.Tasks;
//using Orleans.Hosting;
//using Orleans.TestingHost;
//using Xunit;

//namespace Orleans.TestKit.Samples.Calls
//{
//    public sealed class UserGrainTests :
//        BaseClusterFixture<UserGrainTests.SiloBuilderConfigurator>
//    {
//        [Fact]
//        public async Task Facet_SetColor_WithoutState()
//        {
//            // Arrange
//            var userId = Guid.NewGuid();
//            var userGrain = this.Cluster.GrainFactory.GetGrain<IUserGrain>(userId);

//            var groupId = Guid.NewGuid();

//            // Act
//            await userGrain.AddGroupAsync(groupId);
//        }

//        public sealed class SiloBuilderConfigurator : ISiloBuilderConfigurator
//        {
//            public void Configure(ISiloHostBuilder builder) =>
//                builder
//                    .AddOutgoingGrainCallFilter<CallFilter>()
//                    .AddTestKitStorageAsDefault();
//        }
//    }
//}
