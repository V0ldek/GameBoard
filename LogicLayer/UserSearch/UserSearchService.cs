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
    class UserSearchService : IUserSearchService
    {
        IGameBoardRepository _repository;

        UserSearchService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDto> GetUserByUsernameAsync(string userName)
        {

        }

        public async Task<IEnumerable<UserDto>> GetSearchCandidatesAsync(string userNameInput)
        {
            string inputLower = userNameInput.ToLower();
            var matchingPrefix =
                _repository.ApplicationUsers.Where(u => u.UserName.ToLower().StartsWith(inputLower));
            var matchingInfix = _repository.ApplicationUsers
                .Where(u => EF.Functions.Like(u.UserName, $"%_{inputLower}"));


        }
    }
}
