using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.UserSearch.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameBoard.LogicLayer.UserSearch
{
    internal sealed class UserSearchService : IUserSearchService
    {
        private readonly int MAX_USERS_TO_SHOW = 100; // correct specifiers
        private readonly IGameBoardRepository _repository;

        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSearchService(IGameBoardRepository repository, ILogger<UserSearchService> logger, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
            _logger = logger;
        }

        public Task<UserDto> GetUserByUsernameAsync(string userName)
        {
            var normalizedUserName = userName.ToUpper();
            var user = _repository.ApplicationUsers.Where(u => u.NormalizedUserName == normalizedUserName); // This user might have just been deleted. Possible?

            return user.Select(u => new UserDto(u.Id, u.UserName, u.Email)).FirstAsync(); // SingleAsync?
        }

        public async Task<IEnumerable<UserDto>> GetSearchCandidatesAsync(string userNameInput)
        {
            var normalizedUserNameInput = userNameInput.ToUpper();

            var matchingPrefixesList = await _repository.ApplicationUsers
                    .Where(u => u.NormalizedUserName.StartsWith(normalizedUserNameInput))
                    .Take(MAX_USERS_TO_SHOW)
                    .Select(u => new UserDto(u.Id, u.UserName, u.Email))
                    .ToListAsync();

            var usersToShowLeft = MAX_USERS_TO_SHOW - matchingPrefixesList.Count; 

            if (usersToShowLeft == 0)
            {
                return matchingPrefixesList;
            }

            var matchingInfixesList = await _repository.ApplicationUsers
                .Where(
                    u => EF.Functions.Like(u.NormalizedUserName, $"_%{normalizedUserNameInput}%")
                        && !u.NormalizedUserName.StartsWith(normalizedUserNameInput))
                .Take(usersToShowLeft)
                .Select(u => new UserDto(u.Id, u.UserName, u.Email))
                .ToListAsync();

            matchingPrefixesList.AddRange(matchingInfixesList);

            return matchingPrefixesList;
        }
    }
}
