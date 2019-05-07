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

        public GroupsController(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddUserToGroup(string userName, int groupId)
        {
            await _groupsService.AddUserToGroupAsync(userName, groupId);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateNewGroup(string groupName)
        {
            await _groupsService.AddGroupAsync(User.Identity.Name, groupName);

            return RedirectToAction("Index", "Home");
        }
    }
}