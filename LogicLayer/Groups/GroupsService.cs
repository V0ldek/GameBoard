using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.Configurations;
using GameBoard.LogicLayer.Groups.Dtos;
using GameBoard.LogicLayer.Groups.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.LogicLayer.Groups
{
    public class GroupsService : IGroupsService
    {
        private readonly IGameBoardRepository _repository;

        public GroupsService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task AddGroupAsync(string userName, string groupName)
        {
            var user = await _repository.ApplicationUsers.SingleAsync(ApplicationUser.UserNameEquals(userName));

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
            var user = await _repository.ApplicationUsers.SingleAsync(ApplicationUser.UserNameEquals(userName));
            var group = await _repository.Groups.SingleAsync(g => g.Id == groupId);
            var isUserAlreadyInThisGroup = await _repository.GroupUsers
                .Where(gu => gu.UserId == user.Id && gu.GroupId == group.Id)
                .AnyAsync();

            if (group.OwnerId == user.Id)
            {
                throw new GroupsException("You cannot add yourself to your group.");
            }

            if (isUserAlreadyInThisGroup)
            {
                throw new GroupsException("User is already in this group.");
            }

            var groupUser = new GroupUser
            {
                UserId = user.Id,
                User = user,
                GroupId = groupId,
                Group = group
            };

            _repository.GroupUsers.Add(groupUser);
            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<GroupDto>> GetGroupsByUserNameAsync(string userName) =>
            await _repository.Groups
                .Where(g => g.Owner.UserName == userName)
                .Include(g => g.GroupUsers)
                .ThenInclude(gu => gu.User)
                .Select(g => g.ToDto())
                .ToListAsync();

        public async Task<GroupDto> GetGroupByNamesAsync(string owner, string groupName) =>
            await _repository.Groups
                .Where(g => g.Owner.UserName == owner && g.Name == groupName)
                .Include(g => g.GroupUsers)
                .ThenInclude(gu => gu.User)
                .Select(g => g.ToDto())
                .SingleAsync();
    }
}