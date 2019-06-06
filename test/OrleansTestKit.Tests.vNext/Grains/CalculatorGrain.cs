using System.Threading.Tasks;
using Orleans;

namespace OrleansTestKit.Grains
{
    public sealed class CalculatorGrain : Grain, ICalculatorGrain
    {
        public Task<int> Add(int x, int y) =>
            Task.FromResult(x + y);

        public Task<int> Divide(int x, int y) =>
            Task.FromResult(x / y);

        public Task<int> Multiply(int x, int y) =>
            Task.FromResult(x * y);

        public Task<int> Subtract(int x, int y) =>
            Task.FromResult(x - y);
    }
}
