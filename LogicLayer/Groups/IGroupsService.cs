using System.Collections.Generic;
using System.Threading.Tasks;
using GameBoard.LogicLayer.Groups.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.Groups
{
    public interface IGroupsService
    {
        [NotNull]
        [ItemNotNull]
        Task<IEnumerable<GroupDto>> GetGroupsByUserNameAsync([NotNull] string userName);

        // Throws InvalidOperationException if such group doesn't exist.
        [ItemNotNull]
        Task<GroupDto> GetGroupByNamesAsync([NotNull] string owner, [NotNull] string groupName);

        Task AddGroupAsync([NotNull] string userName, [NotNull] string groupName);

        Task AddUserToGroupAsync([NotNull] string userName, int groupId);
    }
}