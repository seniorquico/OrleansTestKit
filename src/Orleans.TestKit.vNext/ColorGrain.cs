using System.Threading.Tasks;

namespace Orleans.TestKit.Test
{
    public sealed class ColorGrain : Grain, IColorGrain
    {
        /// <summary>The current color.</summary>
        private string color;

        /// <summary>Asynchronously gets the color.</summary>
        /// <returns>A task that represents the asynchronous operation. The task result returns the color.</returns>
        public Task<string> GetColor() =>
            Task.FromResult(this.color);

        /// <summary>Asynchronously sets the color.</summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task SetColor(string color)
        {
            this.color = color;
            return Task.CompletedTask;
        }
    }
}
