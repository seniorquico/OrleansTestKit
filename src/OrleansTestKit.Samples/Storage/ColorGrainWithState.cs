using System;
using System.Threading.Tasks;

namespace Orleans.TestKit.Samples.Storage
{
    /// <summary>
    ///     A grain that tracks a color. Specifically, this is an <see cref="IColorGrain"/> implementation that uses an
    ///     Orleans storage provider and the <see cref="Grain{TGrainState}"/> persistence pattern.
    /// </summary>
    public sealed class ColorGrainWithState : Grain<ColorGrainState>, IColorGrain
    {
        /// <summary>Asynchronously gets the color.</summary>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result returns the color or
        ///     <see cref="Color.Unknown"/> if the color has not yet been set.
        /// </returns>
        public Task<Color> GetColor() =>
            Task.FromResult(this.State.Color);

        /// <summary>
        ///     Asynchronously resets the color to <see cref="Color.Unknown"/>. This has no effect if the color has not
        ///     yet been set or if the color has already been reset.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task ResetColor() =>
            this.State.Initialized
                ? this.ClearStateAsync()
                : Task.CompletedTask;

        /// <summary>
        ///     Asynchronously sets the color. This has no effect if the color has already been set to the specified
        ///     value.
        /// </summary>
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

            if (this.State.Initialized)
            {
                if (color == this.State.Color)
                {
                    return Task.CompletedTask;
                }
            }
            else
            {
                this.State.Id = this.GetPrimaryKey();
            }

            this.State.Color = color;
            return this.WriteStateAsync();
        }
    }
}
