using System.Collections.Generic;
using System.Threading.Tasks;
using GameBoard.LogicLayer.UserSearch.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.UserSearch
{
    public interface IUserSearchService
    {
        [NotNull]
        [ItemNotNull]
        Task<IEnumerable<UserDto>> GetSearchCandidatesAsync([NotNull] string userNameInput);

        [NotNull]
        [ItemNotNull]
        Task<IEnumerable<UserDto>> GetSearchFriendCandidatesAsync(string userName, string input);
    }
}