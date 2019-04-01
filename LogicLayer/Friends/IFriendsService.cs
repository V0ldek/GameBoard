using System.Collections.Generic;
using System.Threading.Tasks;
using GameBoard.LogicLayer.Friends.Dtos;
using GameBoard.LogicLayer.UserSearch.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.Friends
{
    public interface IFriendsService
    {
        [NotNull]
        [ItemNotNull]
        Task<IEnumerable<UserDto>> GetFriendsByUserNameAsync([NotNull] string userName);

        // Throws FriendRequestAlreadyPending if there exists a request between the two users
        // that was not accepted or rejected.
        Task SendFriendRequestAsync([NotNull] CreateFriendRequestDto friendRequest);

        // Returns null if friend request does not exist.
        [CanBeNull]
        Task<FriendRequestDto> GetFriendRequestAsync([NotNull] string friendRequestId);

        // Throws FriendRequestAlreadyFinalized if the request was already accepted or rejected.
        Task AcceptFriendRequestAsync([NotNull] string friendRequestId);

        // Throws FriendRequestAlreadyFinalized if the request was already accepted or rejected.
        Task RejectFriendRequestAsync([NotNull] string friendRequestId);
    }
}