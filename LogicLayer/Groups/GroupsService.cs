using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Repositories;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.Groups
{
    public class GroupsService : IGroupsService
    {
        private readonly IGameBoardRepository _repository;

        public GroupsService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task AddGroupAsync([NotNull] string userName, [NotNull] string groupName)
        {
            var user = _repository.ApplicationUsers.Single(ApplicationUser.UserNameEquals(userName));

            var group = new Group()
            {
                Name = groupName,
                OwnerId = user.Id,
                Owner = user
            };

            _repository.Groups.Add(group);
            await _repository.SaveChangesAsync();
        }

        public async Task AddUserToGroupAsync([NotNull] string userName, int groupId)
        {
            var user = _repository.ApplicationUsers.Single(ApplicationUser.UserNameEquals(userName));
            var group = _repository.Groups.Single(g => g.Id == groupId);

            var groupUser = new GroupUser()
            {
                UserId = user.Id,
                User = user,
                GroupId = groupId,
                Group = group

            };

            _repository.GroupUser.Add(groupUser);
            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Group>> GetGroupsByUserNameAsync([NotNull] string userName)
        {
            var user = _repository.ApplicationUsers.Single(ApplicationUser.UserNameEquals(userName));

            return _repository.Groups.Where(g => g.Owner.UserName == userName).ToList();
        }
    }
}