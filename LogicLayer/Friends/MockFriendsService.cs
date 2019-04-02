using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameBoard.LogicLayer.Friends.Dtos;
using GameBoard.LogicLayer.Friends.Exceptions;
using GameBoard.LogicLayer.UserSearch.Dtos;

namespace GameBoard.LogicLayer.Friends
{
    internal sealed class MockFriendsService : IFriendsService
    {
        public Task<IEnumerable<UserDto>> GetFriendsByUserNameAsync(string userName) => userName == "V0ldek"
            ? Task.FromResult(
                new List<UserDto>
                {
                    new UserDto("1", "Alice", "alice@gmail.com"),
                    new UserDto("2", "V0ldek", "registermen@gmail.com"),
                    new UserDto("3", "Bob", "bob@gmail.com"),
                } as IEnumerable<UserDto>)
            : Task.FromResult((IEnumerable<UserDto>) null);

        public Task SendFriendRequestAsync(CreateFriendRequestDto friendRequest)
        {
            if (friendRequest.UserNameFrom == "V0ldek" && friendRequest.UserNameTo == "Żochu")
            {
                return Task.CompletedTask;
            }
            else if(friendRequest.UserNameFrom == friendRequest.UserNameTo)
            {
                throw new FriendRequestAlreadyFinalizedException("You cannot invite yourself.");
            }
            else
            {
                throw new InvalidOperationException($"Sending from {friendRequest.UserNameFrom} to {friendRequest.UserNameTo} is forbidden.");
            }
        }

        public Task<FriendRequestDto> GetFriendRequestAsync(string friendRequestId) =>
            throw new NotImplementedException();

        public Task<FriendRequestDto> GetFriendRequestAsync(string userNameFrom, string userNameTo) =>
            throw new NotImplementedException();

        public Task AcceptFriendRequestAsync(string friendRequestId) => throw new NotImplementedException();

        public Task RejectFriendRequestAsync(string friendRequestId) => throw new NotImplementedException();
    }
}