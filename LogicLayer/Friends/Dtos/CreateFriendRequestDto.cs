using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoard.LogicLayer.Friends.Dtos
{
    public sealed class CreateFriendRequestDto
    {
        public string UserIdFrom { get; }
        public string UserIdTo { get; }

        public CreateFriendRequestDto(string userIdFrom, string userIdTo)
        {
            UserIdFrom = userIdFrom;
            UserIdTo = userIdTo;
        }
    }
}
