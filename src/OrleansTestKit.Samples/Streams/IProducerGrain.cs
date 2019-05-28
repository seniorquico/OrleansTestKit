using System.Threading.Tasks;

namespace Orleans.TestKit.Samples.Streams
{
    public interface IProducerGrain : IGrainWithGuidKey
    {
        Task GoToSleepAsync();

        Task PublishValueAsync(int value);
    }
}
