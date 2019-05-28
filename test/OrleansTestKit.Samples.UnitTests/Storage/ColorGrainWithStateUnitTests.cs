using System;
using System.Threading.Tasks;
using FluentAssertions;
using Orleans.Storage;
using Xunit;

namespace Orleans.TestKit.Samples.Storage
{
    public sealed class ColorGrainWithStateUnitTests
    {
        private readonly Guid grainId;

        private readonly TestKitSilo silo;

        public ColorGrainWithStateUnitTests()
        {
            // Common Arrange
            this.grainId = Guid.Parse("024be918-7bd0-4675-a6da-2e6ce7f24ac3");
            this.silo = new TestKitSilo();
        }

        [Fact]
        public async Task GetColor_WithDefaultState_ReturnsUnknown()
        {
            // Arrange
            var grain = await this.silo.CreateGrainAsync<ColorGrainWithState>(this.grainId);

            // Act
            var color = await grain.GetColor();

            // Assert
            color.Should().Be(Color.Unknown);
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

            var grain = await this.silo.CreateGrainAsync<ColorGrainWithState>(this.grainId);

            // Act
            var color = await grain.GetColor();

            // Assert
            color.Should().Be(Color.Red);
        }

        [Fact]
        public async Task OnActivateAsync_WithDefaultState_DoesNotMutateState()
        {
            // Arrange
            var storage = this.silo.StorageManager.GetStorage<ColorGrainState>();

            // Act
            var grain = await this.silo.CreateGrainAsync<ColorGrainWithState>(this.grainId);

            // Assert
            storage.State.Color.Should().Be(Color.Unknown);
            storage.State.Id.Should().Be(Guid.Empty);

            // Note: I really don't like this test. I found it necessary when the grain logic was different and performed
            // some actions in OnActivateAsync. I think there's a difference between the "internal" State object of the
            // grain instance versus the persisted state in the "external" store. Right now, we're really exposing and
            // testing the "internal" State object due to the shared reference. However, that wasn't intuitive. I was
            // expecting it to be the value in the "external" store. This may be a bug in our fake storage provider.
        }

        [Fact]
        public async Task ResetColor_WithDefaultState_DoesNotClearState()
        {
            // Arrange
            var storage = this.silo.StorageManager.GetStorage<ColorGrainState>();
            var grain = await this.silo.CreateGrainAsync<ColorGrainWithState>(this.grainId);

            // Act
            await grain.ResetColor();

            // Assert
            storage.State.Color.Should().Be(Color.Unknown);
            storage.State.Id.Should().Be(Guid.Empty);

            // Note that the following assert ties this test to the _implementation_ details. Generally, one should try
            // to avoid tying the test to the implementation details. It can lead to more brittle tests. However, one may
            // choose to accept this as a trade-off when the implementation detail represents an important behavior.
            this.silo.StorageStats().Clears.Should().Be(0);
        }

        [Fact(Skip = "This fails due to invalid TestKit behavior of `ClearStateAsync`.")]
        public async Task ResetColor_WithState_ClearsState()
        {
            // Arrange
            var storage = this.silo.StorageManager.GetStorage<ColorGrainState>();
            storage.State = new ColorGrainState
            {
                Color = Color.Red,
                Id = grainId,
            };

            var grain = await this.silo.CreateGrainAsync<ColorGrainWithState>(this.grainId);

            // Act
            await grain.ResetColor();

            // Assert
            storage.State.Color.Should().Be(Color.Unknown);
            storage.State.Id.Should().Be(Guid.Empty);
        }

        [Fact]
        public async Task SetColor_WithDefaultState_SetsState()
        {
            // Arrange
            var storage = this.silo.StorageManager.GetStorage<ColorGrainState>();
            var grain = await this.silo.CreateGrainAsync<ColorGrainWithState>(this.grainId);

            // Act
            await grain.SetColor(Color.Blue);

            // Assert
            storage.State.Color.Should().Be(Color.Blue);
            storage.State.Id.Should().Be(this.grainId);
        }

        [Theory]
        [InlineData(Color.Unknown)]
        [InlineData((Color)int.MaxValue)]
        public async Task SetColor_WithInvalidColor_ThrowsArgumentException(Color color)
        {
            // Arrange
            var grain = await this.silo.CreateGrainAsync<ColorGrainWithState>(this.grainId);
            Action action = () => grain.SetColor(color);

            // Act+Assert
            action.Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be("color");
        }

        [Fact(Skip = "This fails because the TestKit does not support the ETag.")]
        public async Task SetColor_WithMutatedState_ThrowsInconsistentStateException()
        {
            // Arrange
            var storage = this.silo.StorageManager.GetStorage<ColorGrainState>();
            storage.State = new ColorGrainState
            {
                Color = Color.Red,
                Id = grainId,
            };

            var grain = await this.silo.CreateGrainAsync<ColorGrainWithState>(this.grainId);
            Action action = () => grain.SetColor(Color.Green);

            // Act+Assert
            storage.State.Color = Color.Blue; // mutate storage state
            action.Should().Throw<InconsistentStateException>();
        }

        [Fact]
        public async Task SetColor_WithState_SetsState()
        {
            // Arrange
            var storage = this.silo.StorageManager.GetStorage<ColorGrainState>();
            storage.State = new ColorGrainState
            {
                Color = Color.Red,
                Id = grainId,
            };

            var grain = await this.silo.CreateGrainAsync<ColorGrainWithState>(this.grainId);

            // Act
            await grain.SetColor(Color.Blue);

            // Assert
            storage.State.Color.Should().Be(Color.Blue);
            storage.State.Id.Should().Be(this.grainId);
        }
    }
}
