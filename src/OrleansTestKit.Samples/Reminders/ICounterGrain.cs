using System.Threading.Tasks;

namespace Orleans.TestKit.Samples.Reminders
{
    public interface ICounterGrain : IGrainWithGuidKey
    {
        Task<int> GetValueAsync();
    }
}
