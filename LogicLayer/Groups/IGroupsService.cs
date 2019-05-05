using System.Collections.Generic;
using JetBrains.Annotations;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;

namespace GameBoard.LogicLayer.Groups
{
    public interface IGroupsService
    {
        // Gets groups created by this user(userName).
        [NotNull]
        [ItemNotNull]
        Task<IEnumerable<Group>> GetGroupsByUserNameAsync([NotNull] string userName);

        // Adds new group created by user(userName).
        Task AddGroupAsync([NotNull] string userName, [NotNull] string groupName);

        // Adds new user to group(groupId).
        Task AddUserToGroupAsync([NotNull] string userName, int groupId);
    }
}
