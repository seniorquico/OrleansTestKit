using System;
using System.Threading.Tasks;

namespace Orleans.TestKit.Samples.Storage
{
    /// <summary>
    ///     A grain that tracks a color. Specifically, this is an <see cref="IColorGrain"/> implementation that uses a
    ///     custom storage API and the dependency injection pattern.
    /// </summary>
    public sealed class ColorGrainWithRepository : Grain, IColorGrain
    {
        /// <summary>The custom storage API.</summary>
        private readonly IColorRepository repository;

        /// <summary>The current state. Note, this may be <c>null</c>.</summary>
        private ColorGrainState state;

        /// <summary>Initializes a new instance of the <see cref="ColorGrainWithRepository"/> class.</summary>
        /// <param name="repository">The custom storage API.</param>
        public ColorGrainWithRepository(IColorRepository repository) =>
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));

        /// <summary>Asynchronously gets the color.</summary>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result returns the color or
        ///     <see cref="Color.Unknown"/> if the color has not yet been set.
        /// </returns>
        public Task<Color> GetColor() =>
            this.state == null
                ? Task.FromResult(Color.Unknown)
                : Task.FromResult(this.state.Color);

        /// <summary>Asynchronously activates the grain by initializing the current state.</summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async Task OnActivateAsync() =>
            this.state = await this.repository.GetByIdAsync(this.GetPrimaryKey());

        /// <summary>
        ///     Asynchronously resets the color to <see cref="Color.Unknown"/>. This has no effect if the color has not
        ///     yet been set or if the color has already been reset.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task ResetColor() =>
            this.state == null
                ? Task.CompletedTask
                : this.repository.DeleteAsync(this.state.Id);

        /// <summary>Asynchronously sets the color.</summary>
        /// <param name="color">The new color.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="color"/> is <see cref="Color.Unknown"/> or an invalid value.
        /// </exception>
        public Task SetColor(Color color)
        {
            if (!Enum.IsDefined(typeof(Color), color))
            {
                throw new ArgumentException("The color is invalid.", nameof(color));
            }

            if (color == Color.Unknown)
            {
                throw new ArgumentException("The color must not be \"unknown\".", nameof(color));
            }

            if (this.state == null)
            {
                this.state = new ColorGrainState
                {
                    Color = color,
                    Id = this.GetPrimaryKey(),
                };
                return this.repository.CreateAsync(this.state);
            }
            else
            {
                this.state.Color = color;
                return this.repository.UpdateAsync(this.state.Id, this.state);
            }
        }
    }
}
