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
    internal /*sealed?*/ class UserSearchService : IUserSearchService
    {
        private readonly int MAX_USERS_TO_SHOW = 100; // correct specifiers
        private readonly IGameBoardRepository _repository;

        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private bool _alreadyAdded;

        public UserSearchService(IGameBoardRepository repository, ILogger<UserSearchService> logger, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
            _logger = logger;
            _alreadyAdded = false;
        }

        public Task<UserDto> GetUserByUsernameAsync(string userName)
        {
            var normalizedUserName = userName.ToUpper();
            var user = _repository.ApplicationUsers.Where(u => u.NormalizedUserName == normalizedUserName); // This user might have been just deleted.

            return user.Select(u => new UserDto(u.Id, u.UserName, u.Email)).FirstAsync(); // Single? HashIndex on UserName?
        }

        public async Task<IEnumerable<UserDto>> GetSearchCandidatesAsync(string userNameInput)
        {
            //var newUsers = new List<ApplicationUser>
            //    {
            //        new ApplicationUser { UserName = "V0ldek", Email = "registermen@gmail.com" },
            //        new ApplicationUser { UserName = "ZiOmEk", Email = "jj@jj" },
            //        new ApplicationUser { UserName = "Nadol", Email = "nadol@gmail.com" },
            //        new ApplicationUser { UserName = "Żochu", Email = "mzochowski@gmail.com" },
            //        new ApplicationUser { UserName = "johny", Email = "johny@gmail.com" },
            //        new ApplicationUser { UserName = "Charlie", Email = "example@example.com" }
            //    };
            //// //.Concat(Enumerable.Repeat(new ApplicationUser { UserName = "mockingbird", Email = "invalidEmail"}, 10000));
            //await _repository.ApplicationUsers.AddRangeAsync(newUsers);
            //await _repository.SaveChangesAsync();

            //if (!_alreadyAdded)
            //{
            //    _alreadyAdded = true;
            //    var user = new ApplicationUser { UserName = "80808080", Email = "8080@8080" };
            //    var result = await _userManager.CreateAsync(user, "password");
            //    if (result.Succeeded)
            //    {
            //        _logger.LogInformation("Successfully Registered 80808080");
            //    }
            //    else
            //    {
            //        _logger.LogError("Something went wrong in registering 80808080");
            //    }
            //}
            //else
            //{
            //    _logger.LogInformation("Already registered 80808080");
            //}

            //_logger.LogInformation("Before adding 2mockingbirds");
            //int NUM_OF_MOCKS = 10000;
            //var mocks = new List<ApplicationUser>(NUM_OF_MOCKS);
            //for (int i = 0; i < NUM_OF_MOCKS; i++)
            //{
            //    mocks.Add(new ApplicationUser { UserName = $"2MoCkInGbIrD{i}", NormalizedUserName = $"2MOCKINGBIRD{i}", Email = $"2mocking{i}@bird.pl" });
            //}

            //await _repository.ApplicationUsers.AddRangeAsync(mocks);
            //await _repository.SaveChangesAsync();
            //_logger.LogInformation("Added 2mockingbirds");

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
