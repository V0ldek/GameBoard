using System;
using System.Net;
using System.Threading.Tasks;
using GameBoard.Errors;
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
            try
            {
                await _groupsService.AddUserToGroupAsync(userName, groupId);
            }
            catch (ApplicationException exception)
            {
                return Error.FromController(this).ErrorJson("Error!", exception.Message, HttpStatusCode.BadRequest);
            }
            catch
            {
                return Error.FromController(this).ErrorJson(
                    "Error!",
                    "An unexpected error has occured while processing your request.",
                    HttpStatusCode.InternalServerError);
            }

            return Ok(
                new
                {
                    title = "Group updated.",
                    message = $"User {userName} was added to this group." + 
                        "Refresh this page to see changes."
                });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateNewGroup(string groupName)
        {
            System.Diagnostics.Debug.WriteLine(groupName);
            try
            {
                await _groupsService.AddGroupAsync(User.Identity.Name, groupName);
            }
            catch (ApplicationException exception)
            {
                return Error.FromController(this).ErrorJson("Error!", exception.Message, HttpStatusCode.BadRequest);
            }
            catch
            {
                return Error.FromController(this).ErrorJson(
                    "Error!",
                    "An unexpected error has occured while processing your request.",
                    HttpStatusCode.InternalServerError);
            }

            return Ok(
                new
                {
                    title = "Group added.",
                    message = "Refresh this page to see changes."
                });
        }
    }
}