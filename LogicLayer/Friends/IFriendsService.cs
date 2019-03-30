using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameBoard.LogicLayer.Friends.Dtos;
using GameBoard.LogicLayer.UserSearch.Dtos;

namespace GameBoard.LogicLayer.Friends
{
    public interface IFriendsService
    {
        Task<IEnumerable<UserDto>> GetFriendsByUserNameAsync(string userName);

        Task SendFriendRequestAsync(CreateFriendRequestDto friendRequest);

        Task<FriendRequestDto> GetFriendRequestAsync(string friendRequestId);

        Task AcceptFriendRequestAsync(string friendRequestId);
    }
}