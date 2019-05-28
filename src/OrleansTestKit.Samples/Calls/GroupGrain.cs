using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orleans.TestKit.Samples.Calls
{
    public sealed class GroupGrain : Grain, IGroupGrain
    {
        private readonly SortedSet<Guid> users = new SortedSet<Guid>();

        public Task AddUserAsync(Guid userId)
        {
            this.users.Add(userId);
            return Task.CompletedTask;
        }

        public Task<Guid[]> GetUsersAsync() =>
            Task.FromResult(this.users.ToArray());

        public Task RemoveUserAsync(Guid userId)
        {
            this.users.Remove(userId);
            return Task.CompletedTask;
        }
    }
}
