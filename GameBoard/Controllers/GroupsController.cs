using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.LogicLayer.Friends;
using Microsoft.AspNetCore.Authorization;
using GameBoard.LogicLayer.Groups;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private readonly IGroupsService _groupsService;
        private readonly IFriendsService _friendsService;

        public GroupsController(/*IGroupsService groupsService*/IFriendsService friendsService)
        {
            _friendsService = friendsService;
            // _groupsService = groupsService;
        }

        public IActionResult ManageGroups() => View();
    }
}