using System;
using System.Threading.Tasks;

namespace Orleans.TestKit.Samples.Calls
{
    public interface IGroupGrain : IGrainWithGuidKey
    {
        Task AddUserAsync(Guid userId);

        Task<Guid[]> GetUsersAsync();

        Task RemoveUserAsync(Guid userId);
    }
}
