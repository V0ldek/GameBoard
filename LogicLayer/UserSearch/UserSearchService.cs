using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.UserSearch.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.LogicLayer.UserSearch
{
    internal /*sealed?*/ class UserSearchService : IUserSearchService
    {
        private readonly int MAX_USERS_TO_SHOW = 100; // correct specifiers
        private readonly IGameBoardRepository _repository;

        public UserSearchService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public Task<UserDto> GetUserByUsernameAsync(string userName)
        {
            var user = _repository.ApplicationUsers.Where(u => u.UserName == userName);

            return user.Select(u => new UserDto(u.Id, u.UserName, u.Email)).FirstAsync(); // Single? HashIndex on UserName?
        }

        public async Task<IEnumerable<UserDto>> GetSearchCandidatesAsync(string userNameInput)
        {
            var inputUpper = userNameInput.ToUpper();
            var matchingPrefixesList = await _repository.ApplicationUsers
                    .Where(u => u.UserName.StartsWith(inputUpper))
                    .Take(MAX_USERS_TO_SHOW)
                    .Select(u => new UserDto(u.Id, u.UserName, u.Email))
                    .ToListAsync();

            var usersToShowLeft = MAX_USERS_TO_SHOW - matchingPrefixesList.Count; 

            if (usersToShowLeft == 0)
            {
                return matchingPrefixesList;
            }

            var matchingInfixesList = await _repository.ApplicationUsers // can I avoid using await here?
                .Where(
                    u => EF.Functions
                        .Like(u.UserName, $"_%{inputUpper}%"))
                .Take(usersToShowLeft)
                .Select(u => new UserDto(u.Id, u.UserName, u.Email))
                .ToListAsync();

            matchingPrefixesList.AddRange(matchingInfixesList);

            return matchingPrefixesList;
        }
    }
}
