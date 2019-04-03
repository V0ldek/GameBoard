using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameBoard.LogicLayer.UserSearch.Dtos;

namespace GameBoard.LogicLayer.UserSearch
{
    internal sealed class MockUserSearchService : IUserSearchService
    {
        public Task<UserDto> GetUserByUsernameAsync(string username) => throw new NotImplementedException();

        public Task<IEnumerable<UserDto>> GetSearchCandidatesAsync(string usernameInput)
        {
            if (usernameInput == "crash")
            {
                throw new InvalidOperationException();
            }

            return Task.FromResult(
                new List<UserDto>
                {
                    new UserDto("1", "V0ldek", "registermen@gmail.com"),
                    new UserDto("3", "Nadol", "nadol@gmail.com"),
                    new UserDto("4", "Żochu", "mzochowski@gmail.com"),
                    new UserDto("5", "johny", "johny@gmail.com"),
                    new UserDto("2", "Charlie", "example@example.com")
                } as IEnumerable<UserDto>);
        }
    }
}
