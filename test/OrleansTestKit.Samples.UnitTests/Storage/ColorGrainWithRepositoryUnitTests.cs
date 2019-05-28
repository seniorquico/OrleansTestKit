using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Orleans.TestKit.Samples.Storage
{
    public sealed class ColorGrainWithRepositoryUnitTests
    {
        private readonly Guid grainId;

        private readonly TestKitSilo silo;

        public ColorGrainWithRepositoryUnitTests()
        {
            // Shared Arrange
            this.grainId = Guid.Parse("fe2d6b8b-dd46-4cb9-9001-61f1f48c1ba5");
            this.silo = new TestKitSilo();
        }

        [Fact]
        public async Task GetColor_WithoutState_ReturnsUnknown()
        {
            // Act
            var grain = await this.silo.CreateGrainAsync<ColorGrainWithRepository>(this.grainId);
            var color = await grain.GetColor();

            // Assert
            color.Should().Be(Color.Unknown);

            this.silo.StorageStats().Reads.Should().Be(0);
            this.silo.StorageStats().Writes.Should().Be(0);
            this.silo.StorageStats().Clears.Should().Be(0);
        }

        [Fact]
        public async Task GetColor_WithState_ReturnsColor()
        {
            // Arrange
            var storage = this.silo.StorageManager.GetStorage<ColorGrainState>();
            storage.State = new ColorGrainState
            {
                Color = Color.Red,
                Id = grainId,
            };

            // Act
            var grain = await this.silo.CreateGrainAsync<ColorGrainWithRepository>(this.grainId);
            var color = await grain.GetColor();

            // Assert
            color.Should().Be(Color.Red);

            this.silo.StorageStats().Reads.Should().Be(0);
            this.silo.StorageStats().Writes.Should().Be(0);
            this.silo.StorageStats().Clears.Should().Be(0);
        }

        [Fact]
        public async Task ResetColor_WithoutState_DoesNotCallClearState()
        {
            // Act
            var grain = await this.silo.CreateGrainAsync<ColorGrainWithRepository>(this.grainId);
            await grain.ResetColor();

            // Assert
            this.silo.StorageStats().Reads.Should().Be(0);
            this.silo.StorageStats().Writes.Should().Be(0);
            this.silo.StorageStats().Clears.Should().Be(0);
        }

        [Fact]
        public async Task ResetColor_WithState_DoesCallClearState()
        {
            // Arrange
            var storage = this.silo.StorageManager.GetStorage<ColorGrainState>();
            storage.State = new ColorGrainState
            {
                Color = Color.Red,
                Id = grainId,
            };

            // Act
            var grain = await this.silo.CreateGrainAsync<ColorGrainWithRepository>(this.grainId);
            await grain.ResetColor();

            // Assert
            this.silo.StorageStats().Reads.Should().Be(0);
            this.silo.StorageStats().Writes.Should().Be(0);
            this.silo.StorageStats().Clears.Should().Be(1);
        }

        [Theory]
        [InlineData(Color.Unknown)]
        [InlineData((Color)int.MaxValue)]
        public async Task SetColor_WithInvalidColor_ThrowsArgumentException(Color color)
        {
            // Act+Assert
            var grain = await this.silo.CreateGrainAsync<ColorGrainWithRepository>(this.grainId);

            Action action = () => grain.SetColor(color);
            action.Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be("color");

            this.silo.StorageStats().Reads.Should().Be(0);
            this.silo.StorageStats().Writes.Should().Be(0);
            this.silo.StorageStats().Clears.Should().Be(0);
        }

        [Fact]
        public async Task SetColor_WithoutState_DoesCallSetState()
        {
            // Act
            var grain = await this.silo.CreateGrainAsync<ColorGrainWithRepository>(this.grainId);
            await grain.SetColor(Color.Red);

            // Assert
            var state = this.silo.StorageManager.GetStorage<ColorGrainState>();
            state.State.Color.Should().Be(Color.Red);
            state.State.Id.Should().Be(this.grainId);

            this.silo.StorageStats().Reads.Should().Be(0);
            this.silo.StorageStats().Writes.Should().Be(1);
            this.silo.StorageStats().Clears.Should().Be(0);
        }

        [Fact]
        public async Task SetColor_WithState_DoesCallSetState()
        {
            // Arrange
            var storage = this.silo.StorageManager.GetStorage<ColorGrainState>();
            storage.State = new ColorGrainState
            {
                Color = Color.Blue,
                Id = grainId,
            };

            // Act
            var grain = await this.silo.CreateGrainAsync<ColorGrainWithRepository>(this.grainId);
            await grain.SetColor(Color.Red);

            // Assert
            var state = this.silo.StorageManager.GetStorage<ColorGrainState>();
            state.State.Color.Should().Be(Color.Red);
            state.State.Id.Should().Be(this.grainId);

            this.silo.StorageStats().Reads.Should().Be(0);
            this.silo.StorageStats().Writes.Should().Be(1);
            this.silo.StorageStats().Clears.Should().Be(0);
        }
    }
}
