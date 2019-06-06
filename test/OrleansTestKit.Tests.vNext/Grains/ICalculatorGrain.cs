using System.Threading.Tasks;
using Orleans;

namespace OrleansTestKit.Grains
{
    public interface ICalculatorGrain : IGrainWithIntegerKey
    {
        Task<int> Add(int x, int y);

        Task<int> Divide(int x, int y);

        Task<int> Multiply(int x, int y);

        Task<int> Subtract(int x, int y);
    }
}
