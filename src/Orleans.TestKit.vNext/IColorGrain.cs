using System.Threading.Tasks;

namespace Orleans.TestKit.Test
{
    public interface IColorGrain : IGrainWithIntegerKey
    {
        /// <summary>Asynchronously gets the color.</summary>
        /// <returns>A task that represents the asynchronous operation. The task result returns the color.</returns>
        Task<string> GetColor();

        /// <summary>Asynchronously sets the color.</summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SetColor(string color);
    }
}
