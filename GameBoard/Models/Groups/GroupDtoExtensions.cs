﻿using System.Linq;
using GameBoard.Models.User;
using GameBoard.LogicLayer.Groups.Dtos;

namespace GameBoard.Models.Groups
{
    public static class GroupDtoExtensions
    {
        public static GroupViewModel ToViewModel(this GroupDto groupDto) =>
            new GroupViewModel(groupDto.GroupId, groupDto.GroupName, groupDto.Users.Select(u => u.ToViewModel()));
    }
}