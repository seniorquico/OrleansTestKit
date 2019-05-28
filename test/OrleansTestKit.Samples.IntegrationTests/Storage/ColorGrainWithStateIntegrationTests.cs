//using System;
//using System.Threading.Tasks;
//using FluentAssertions;
//using Orleans.Hosting;
//using Orleans.TestingHost;
//using Xunit;

//namespace Orleans.TestKit.Samples.Storage
//{
//    public sealed class ColorGrainWithStateIntegrationTests :
//        BaseClusterFixture<ColorGrainWithStateIntegrationTests.SiloBuilderConfigurator>
//    {
//        [Fact]
//        public async Task Facet_GetColor_WithoutState()
//        {
//            // Arrange
//            var grainId = Guid.NewGuid();
//            var grain = this.Cluster.GrainFactory.GetGrain<IColorGrain>(grainId, typeof(ColorGrainWithFacet).FullName);

//            // Act
//            var color = await grain.GetColor();

//            // Assert
//            color.Should().Be(Color.Unknown);
//        }

//        [Fact]
//        public async Task Facet_SetColor_WithoutState()
//        {
//            // Arrange
//            var grainId = Guid.NewGuid();
//            var grain = this.Cluster.GrainFactory.GetGrain<IColorGrain>(grainId, typeof(ColorGrainWithFacet).FullName);

//            // Act
//            await grain.SetColor(Color.Yellow);

//            // Assert
//            var (state, etag) = await this.GetGrainFacetStateAsync<ColorGrainState>(grain, "State");
//            state.Should().NotBeNull();
//            state.Color.Should().Be(Color.Yellow);
//            state.Id.Should().Be(grainId);
//            etag.Should().NotBeNull();
//        }

//        [Fact]
//        public async Task GetColor_WithoutState()
//        {
//            // Arrange
//            var grainId = Guid.NewGuid();
//            var grain = this.Cluster.GrainFactory.GetGrain<IColorGrain>(grainId, typeof(ColorGrainWithState).FullName);

//            // Act
//            var color = await grain.GetColor();

//            // Assert
//            color.Should().Be(Color.Unknown);
//        }

//        [Fact]
//        public async Task SetColor_WithoutState()
//        {
//            // Arrange
//            var grainId = Guid.NewGuid();
//            var grain = this.Cluster.GrainFactory.GetGrain<IColorGrain>(grainId, typeof(ColorGrainWithState).FullName);

//            // Act
//            await grain.SetColor(Color.Orange);

//            // Assert
//            var (state, etag) = await this.GetGrainStateAsync<ColorGrainState>(grain);
//            state.Should().NotBeNull();
//            state.Color.Should().Be(Color.Orange);
//            state.Id.Should().Be(grainId);
//            etag.Should().NotBeNull();
//        }

//        public sealed class SiloBuilderConfigurator : ISiloBuilderConfigurator
//        {
//            public void Configure(ISiloHostBuilder builder) =>
//                builder
//                    .AddTestKitStorageAsDefault();
//        }
//    }
//}
