using System;
using System.Threading.Tasks;

namespace Orleans.TestKit.Samples.Calls
{
    public interface IUserGrain : IGrainWithGuidKey
    {
        Task AddGroupAsync(Guid groupId);

        Task<Guid[]> GetGroupsAsync();

        Task RemoveGroupAsync(Guid groupId);
    }
}
