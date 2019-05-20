﻿using System.Collections.Generic;
using GameBoard.Models.User;

namespace GameBoard.Models.FriendSearch
{
    public class FriendSearchResultViewModel
    {
        public IEnumerable<UserViewModel> Users { get; }
        public string GroupId { get; }

        public FriendSearchResultViewModel(IEnumerable<UserViewModel> users, string groupId)
        {
            Users = users;
            GroupId = groupId;
        }
    }
}