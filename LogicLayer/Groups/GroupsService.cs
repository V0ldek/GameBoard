using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.Configurations;
using GameBoard.LogicLayer.Groups.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GameBoard.LogicLayer.Groups
{
    public class GroupsService : IGroupsService
    {
        private readonly IGameBoardRepository _repository;
        private GroupsConfiguration GroupsOptions { get; }

        public GroupsService(IGameBoardRepository repository, IOptions<GroupsConfiguration> groupsOptions)
        {
            _repository = repository;
            GroupsOptions = groupsOptions.Value;
        }

        public async Task AddGroupAsync(string userName, string groupName)
        {
            var user = _repository.ApplicationUsers.Single(ApplicationUser.UserNameEquals(userName));

            var group = new Group
            {
                Name = groupName,
                OwnerId = user.Id,
                Owner = user
            };

            _repository.Groups.Add(group);
            await _repository.SaveChangesAsync();
        }

        public async Task AddUserToGroupAsync(string userName, int groupId)
        {
            var user = _repository.ApplicationUsers.Single(ApplicationUser.UserNameEquals(userName));
            var group = _repository.Groups.Single(g => g.Id == groupId);

            var groupUser = new GroupUser
            {
                UserId = user.Id,
                User = user,
                GroupId = groupId,
                Group = group
            };

            _repository.GroupUser.Add(groupUser);
            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<GroupDto>> GetGroupsByUserNameAsync(string userName) =>
            await _repository.Groups
                .Where(g => g.Owner.UserName == userName)
                .Include(g => g.GroupUser)
                .ThenInclude(gu => gu.User)
                .Select(g => g.ToDto())
                .ToListAsync();

        public async Task<GroupDto> GetGroupByNamesAsync(string owner, string groupName) =>
           await _repository.Groups
                .Where(g => g.Owner.UserName == owner && g.Name == groupName)
                .Include(g => g.GroupUser)
                .ThenInclude(gu => gu.User)
                .Select(g => g.ToDto())
                .SingleAsync();
    }
}