using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.UserSearch.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.LogicLayer.UserSearch
{
    internal sealed class UserSearchService : IUserSearchService
    {
        private const int MaxUsersToShow = 100;
        private readonly IGameBoardRepository _repository;

        public UserSearchService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserDto>> GetSearchCandidatesAsync(string userNameInput)
        {
            var normalizedUserNameInput = userNameInput.ToUpper();
            var matchingPrefixesList = await GetMatchingPrefixesAsync(normalizedUserNameInput);
            var usersToShowLeft = MaxUsersToShow - matchingPrefixesList.Count;
            var matchingInfixesList =
                await GetMatchingNonPrefixInfixesUpToACapAsync(normalizedUserNameInput, usersToShowLeft);

            return matchingPrefixesList.Concat(matchingInfixesList);
        }

        private async Task<List<UserDto>> GetMatchingPrefixesAsync(string normalizedUserNameInput) =>
            await _repository.ApplicationUsers
                .Where(u => u.NormalizedUserName.StartsWith(normalizedUserNameInput))
                .Take(MaxUsersToShow)
                .Select(u => new UserDto(u.Id, u.UserName, u.Email))
                .ToListAsync();

        private async Task<List<UserDto>> GetMatchingNonPrefixInfixesUpToACapAsync(
            string normalizedUserNameInput,
            int usersToShowLeft)
        {
            if (usersToShowLeft == 0)
            {
                return new List<UserDto>();
            }

            return await _repository.ApplicationUsers
                .Where(
                    u => EF.Functions.Like(u.NormalizedUserName, $"_%{normalizedUserNameInput}%")
                        && !u.NormalizedUserName.StartsWith(normalizedUserNameInput))
                .Take(usersToShowLeft)
                .Select(u => new UserDto(u.Id, u.UserName, u.Email))
                .ToListAsync();
        }
    }
}