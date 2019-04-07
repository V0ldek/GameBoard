//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using GameBoard.DataLayer.Entities;
//using GameBoard.DataLayer.Enums;
//using GameBoard.LogicLayer.Friends.Dtos;
//using GameBoard.LogicLayer.UserSearch.Dtos;
//using GameBoard.DataLayer.Repositories;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace GameBoard.LogicLayer.Friends
//{
//    internal /*sealed?*/ class FriendService : IFriendsService
//    {
//        private readonly IGameBoardRepository _repository;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public FriendService(IGameBoardRepository repository, UserManager<ApplicationUser> userManager)
//        {
//            _repository = repository;
//            _userManager = userManager;
//        }

//        public Task<IEnumerable<UserDto>> GetFriendsByUserNameAsync(string userName)
//        {
//            var user = _repository.ApplicationUsers.Where(u => u.UserName == userName);
//            var friends = user.Include(u => u.SmallerIdInFriendships)
//                .SelectMany(u => u.SmallerIdInFriendships)
//                .Where(f => f.FriendshipStatus == FriendshipStatus.Lasts)
//                .Include(f => f.UserGreater)
//                .Select(f => f.UserGreater.Id /* UserName, Email */);
//            return null;
//        }

//        public async Task SendFriendRequestAsync(CreateFriendRequestDto friendRequest)
//        {
//            string userSmallerId;
//            string userGreaterId;
//            WhoSentLatestRequest whoSent;

//            if (String.Compare(friendRequest.UserNameFrom, friendRequest.UserNameTo) < 0)
//            {
//                userSmallerId = friendRequest.UserNameFrom;
//                userGreaterId = friendRequest.UserNameTo;
//                whoSent = WhoSentLatestRequest.UserWithSmallerId;
//            }
//            else
//            {
//                userSmallerId = friendRequest.UserNameTo;
//                userGreaterId = friendRequest.UserNameFrom;
//                whoSent = WhoSentLatestRequest.UserWithGreaterId;
//            }

//            using (var transaction = new GameBoardDbContext())
//            {
//                var friendship = await _repository.Friendships
//                    .SingleOrDefaultAsync(f => (f.UserSmallerId == userSmallerId && f.UserSmallerId == userGreaterId));

//                if (friendship != null)
//                {

//                }

//                _repository.Friendships.Add(
//                    new Friendship()
//                    {
//                        //auto increment
//                        UserSmallerId = friendRequest.UserNameFrom,
//                        UserGreaterId = friendRequest.UserNameTo,
//                        FriendshipStatus = FriendshipStatus.PendingFriendRequest
//                    });
//            }


//            await _repository.SaveChangesAsync();

//            //send mail with link
//        }

//        public Task<FriendRequestDto> GetFriendRequestAsync(string friendRequestId)
//        {
//            int id = Int32.Parse(friendRequestId);
//            var friendship = _repository.Friendships.Where(f => f.Id == id);

//            return friendship
//                .Include(f => f.UserSmaller)
//                .Include(f => f.UserGreater)
//                .Select(
//                    f => new FriendRequestDto(
//                        friendRequestId,
//                        new UserDto(f.UserSmaller.Id, f.UserSmaller.UserName, f.UserSmaller.Email),//wrong, greater can send request too.
//                        new UserDto(f.UserGreater.Id, f.UserGreater.UserName, f.UserGreater.Email),
//                        f.FriendshipStatus.ToFriendRequest()))
//                .SingleAsync();
//        }

//        private async Task ChangeFriendRequestStatus(string friendRequestId, FriendshipStatus friendshipStatus)
//        {
//            int id = Int32.Parse(friendRequestId);
//            var friendship = await _repository.Friendships.SingleAsync(f => f.Id == id); // can I update in single step?

//            friendship.FriendshipStatus = friendshipStatus;

//            _repository.Friendships.Update(friendship);
//            await _repository.SaveChangesAsync(); // can I avoid using await here?
//        }

//        public async Task AcceptFriendRequestAsync(string friendRequestId) =>
//            await ChangeFriendRequestStatus(friendRequestId, FriendshipStatus.Lasts); // can I avoid using await here? 

//        public async Task RejectFriendRequestAsync(string friendRequestId) =>
//            await ChangeFriendRequestStatus(friendRequestId, FriendshipStatus.Rejected); // can I avoid using await here?
//    }
//}
