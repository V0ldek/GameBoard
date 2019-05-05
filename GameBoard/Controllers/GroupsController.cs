using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}