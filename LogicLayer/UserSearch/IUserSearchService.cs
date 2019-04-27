using System.Collections.Generic;
using System.Threading.Tasks;
using GameBoard.LogicLayer.UserSearch.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.UserSearch
{
    public interface IUserSearchService
    {
        // Returns null if user does not exist.
        [CanBeNull]
        Task<UserDto> GetUserByUsernameAsync([NotNull] string userName);

        [NotNull]
        [ItemNotNull]
        Task<IEnumerable<UserDto>> GetSearchCandidatesAsync([NotNull] string userNameInput);
    }
}