using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameBoard.LogicLayer.UserSearch.Dtos;

namespace GameBoard.LogicLayer.UserSearch
{
    public interface IUserSearchService
    {
        Task<UserDto> GetUserByUsernameAsync(string username);

        Task<IEnumerable<UserDto>> GetSearchCandidatesAsync(string usernameInput);
    }
}
