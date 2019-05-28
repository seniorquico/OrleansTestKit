using System.Threading.Tasks;

namespace Orleans.TestKit.Samples.Streams
{
    public interface IConsumerGrain : IGrainWithGuidKey
    {
        Task<int> GetValueAsync();

        Task GoToSleepAsync();
    }
}
