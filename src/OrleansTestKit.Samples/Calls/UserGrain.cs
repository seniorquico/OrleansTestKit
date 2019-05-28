using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orleans.TestKit.Samples.Calls
{
    public sealed class UserGrain : Grain, IUserGrain
    {
        private readonly SortedSet<Guid> groups = new SortedSet<Guid>();

        public async Task AddGroupAsync(Guid groupId)
        {
            var groupGrain = this.GrainFactory.GetGrain<IGroupGrain>(groupId);
            await groupGrain.AddUserAsync(this.GetPrimaryKey());

            this.groups.Add(groupId);
        }

        public Task<Guid[]> GetGroupsAsync() =>
            Task.FromResult(this.groups.ToArray());

        public async Task RemoveGroupAsync(Guid groupId)
        {
            var groupGrain = this.GrainFactory.GetGrain<IGroupGrain>(groupId);
            await groupGrain.RemoveUserAsync(this.GetPrimaryKey());

            this.groups.Remove(groupId);
        }
    }
}
