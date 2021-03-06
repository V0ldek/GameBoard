﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameBoard.LogicLayer.DescriptionTabs.Dtos;
using GameBoard.LogicLayer.GameEvents.Dtos;
using GameBoard.Models.DescriptionTab;
using GameBoard.Models.User;

namespace GameBoard.Models.GameEvent
{
    public class GameEventViewModel
    {
        private readonly GameEventPermission _permission;
        public int Id { get; }

        public string Name { get; }

        public string Place { get; }

        public DateTime? Date { get; }

        [Display(Name = "Planned games")]
        public IEnumerable<string> Games { get; }

        [Display(Name = "Event creator")]
        public UserViewModel Creator { get; }

        [Display(Name = "Pending invitations")]
        public IEnumerable<UserViewModel> Invitees { get; }

        [Display(Name = "Participants")]
        public IEnumerable<UserViewModel> Participants { get; }

        public DescriptionTabViewModel DescriptionTab { get; }

        public bool IsCreator => _permission == GameEventPermission.Creator;

        public bool IsInvitePending => _permission == GameEventPermission.PendingInvitation;

        public GameEventViewModel(
            int id,
            string name,
            string place,
            DateTime? date,
            GameEventPermission permission,
            IEnumerable<string> games,
            UserViewModel creator,
            IEnumerable<UserViewModel> invitees,
            IEnumerable<UserViewModel> participants,
            DescriptionTabViewModel descriptionTab)
        {
            Id = id;
            Name = name;
            Place = place;
            Date = date;
            _permission = permission;
            Games = games;
            Creator = creator;
            Invitees = invitees;
            Participants = participants;
            DescriptionTab = descriptionTab;
        }
    }
}