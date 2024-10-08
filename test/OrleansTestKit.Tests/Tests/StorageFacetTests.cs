﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Orleans.Core;
using Orleans.Runtime;
using Orleans.Storage;
using TestGrains;
using TestInterfaces;
using Xunit;

namespace Orleans.TestKit.Tests
{
    public class StorageFacetTests : TestKitBase
    {
        private static readonly Guid GrainId = Guid.Parse("f267aaeb-38dd-4ff4-b7c8-20061077ab10");

        [Fact]
        public async Task GetColor_WithDefaultState_ReturnsUnknown()
        {
            // Arrange
            Silo.AddPersistentState<ColorGrainState>("State");

            var grain = await Silo.CreateGrainAsync<ColorGrain>(GrainId);

            // Act
            var color = await grain.GetColor();

            // Assert
            color.Should().Be(Color.Unknown);
        }

        [Fact]
        public async Task GetColor_WithState_ReturnsColor()
        {
            // Arrange
            var state = new ColorGrainState
            {
                Color = Color.Red,
                Id = GrainId,
            };

            Silo.AddPersistentState(stateName: "State", state: state);

            var grain = await Silo.CreateGrainAsync<ColorGrain>(GrainId);

            // Act
            var color = await grain.GetColor();

            // Assert
            color.Should().Be(Color.Red);
        }

        [Fact]
        public async Task OnActivateAsync_WithDefaultState_DoesNotMutateState()
        {
            // Arrange
            var state = new ColorGrainState();
            Silo.AddPersistentState(stateName: "State", state: state);

            // Act
            var grain = await Silo.CreateGrainAsync<ColorGrain>(GrainId);

            // Assert
            state.Color.Should().Be(Color.Unknown);
            state.Id.Should().Be(Guid.Empty);
        }

        [Fact]
        public async Task ResetColor_WithDefaultState_DoesNotClearState()
        {
            // Arrange
            var state = new ColorGrainState();
            Silo.AddPersistentState(stateName: "State", state: state);

            var grain = await Silo.CreateGrainAsync<ColorGrain>(GrainId);

            // Act
            await grain.ResetColor();

            // Assert
            state.Color.Should().Be(Color.Unknown);
            state.Id.Should().Be(Guid.Empty);

            // Note that the following assert ties this test to the _implementation_ details. Generally, one should try
            // to avoid tying the test to the implementation details. It can lead to more brittle tests. However, one may
            // choose to accept this as a trade-off when the implementation detail represents an important behavior.
            var storageStats = Silo.StorageManager.GetStorageStats(stateName: "State");
            storageStats.Clears.Should().Be(0);
        }

        [Fact]
        public async Task ResetColor_WithState_ClearsState()
        {
            // Arrange
            var state = new ColorGrainState
            {
                Color = Color.Red,
                Id = GrainId,
            };
            Silo.AddPersistentState(stateName: "State", state: state);

            var grain = await Silo.CreateGrainAsync<ColorGrain>(GrainId);

            // Act
            await grain.ResetColor();

            // Assert
            var storageStats = Silo.StorageManager.GetStorageStats(stateName: "State");
            storageStats.Clears.Should().Be(1);
        }

        [Fact]
        public async Task SetColor_WithDefaultState_SetsState()
        {
            // Arrange
            var state = new ColorGrainState();
            Silo.AddPersistentState(stateName: "State", state: state);

            var grain = await Silo.CreateGrainAsync<ColorGrain>(GrainId);

            // Act
            await grain.SetColor(Color.Blue);

            // Assert
            state.Color.Should().Be(Color.Blue);
            state.Id.Should().Be(GrainId);
        }

        [Theory]
        [InlineData(Color.Unknown)]
        [InlineData((Color)int.MaxValue)]
        public async Task SetColor_WithInvalidColor_ThrowsArgumentException(Color color)
        {
            // Arrange
            Silo.AddPersistentState<ColorGrainState>(stateName: "State");

            var grain = await Silo.CreateGrainAsync<ColorGrain>(GrainId);
            Action action = () => grain.SetColor(color);

            // Act+Assert
            action.Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be("color");
        }

        [Fact]
        public async Task SetColor_WithMutatedState_ThrowsInconsistentStateException()
        {
            // Arrange
            var state = new ColorGrainState();

            var mockState = new Mock<IStorage<ColorGrainState>>();
            mockState.SetupGet(o => o.State).Returns(state);
            mockState.Setup(o => o.WriteStateAsync()).Throws<InconsistentStateException>();

            Silo.AddPersistentStateStorage(stateName: "State", storage: mockState.Object);

            var grain = await Silo.CreateGrainAsync<ColorGrain>(GrainId);
            Action action = () => grain.SetColor(Color.Green);

            // Act+Assert
            action.Should().Throw<InconsistentStateException>();
        }

        [Fact]
        public async Task SetColor_WithState_SetsState()
        {
            // Arrange
            var state = new ColorGrainState
            {
                Color = Color.Red,
                Id = GrainId,
            };
            Silo.AddPersistentState(stateName: "State", state: state);

            var grain = await Silo.CreateGrainAsync<ColorGrain>(GrainId);

            // Act
            await grain.SetColor(Color.Blue);

            // Assert
            state.Color.Should().Be(Color.Blue);
            state.Id.Should().Be(GrainId);
        }

        [Fact]
        public async Task GetColor_WithGrainState_ReturnsColor()
        {
            // Arrange
            var state = new ColorGrainState
            {
                Color = Color.Red,
                Id = GrainId,
            };

            Silo.AddGrainState<ColorRankingGrain, ColorGrainState>(state: state);

            var grain = await Silo.CreateGrainAsync<ColorRankingGrain>(GrainId);

            // Act
            var color = await grain.GetLeastFavouriteColor();
            color.Should().Be(Color.Red);
        }

        [Fact]
        public async Task SetLeastFavouriteColor_WithDefaultGrainState_SetsState()
        {
            var grain = await Silo.CreateGrainAsync<ColorRankingGrain>(GrainId);

            // Act
            await grain.SetLeastFavouriteColor(Color.Blue);

            var storage = Silo.StorageManager.GetGrainStorage<ColorRankingGrain, ColorGrainState>();
            var state = storage.State;

            state.Color.Should().Be(Color.Blue);
            state.Id.Should().Be(GrainId);
        }

        [Fact]
        public async Task GetLeastFavouriteColor_WithGrainState_ReturnsColor()
        {
            // Arrange
            var state = new ColorGrainState
            {
                Color = Color.Red,
                Id = GrainId,
            };

            Silo.AddGrainState<ColorRankingGrain, ColorGrainState>(state: state);

            var grain = await Silo.CreateGrainAsync<ColorRankingGrain>(GrainId);

            // Act
            var color = await grain.GetLeastFavouriteColor();
            color.Should().Be(Color.Red);
        }

        [Fact]
        public async Task SetFavouriteColor_WithState_SetsState()
        {
            var persistentState = Silo.AddPersistentState("Default", state: new ColorGrainState
            {
                Id = GrainId,
                Color = Color.Red
            });

            var grain = await Silo.CreateGrainAsync<ColorRankingGrain>(GrainId);

            // Act
            await grain.SetFavouriteColor(Color.Blue);

            var state = persistentState.State;
            state.Color.Should().Be(Color.Blue);
            state.Id.Should().Be(GrainId);
        }

        [Fact]
        public async Task GetFavouriteColor_WithState_ReturnsColor()
        {
            Silo.AddPersistentState("Default", state: new ColorGrainState
            {
                Id = GrainId,
                Color = Color.Blue
            });

            var grain = await Silo.CreateGrainAsync<ColorRankingGrain>(GrainId);

            // Act
            var color = await grain.GetFavouriteColor();
            color.Should().Be(Color.Blue);
        }
    }
}
