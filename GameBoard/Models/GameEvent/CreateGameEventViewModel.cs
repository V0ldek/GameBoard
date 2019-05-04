using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GameBoard.LogicLayer.GameEvents.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Models.GameEvent
{
    public class CreateGameEventViewModel
    {
        internal const int MaxGameStringLength = 128;

        internal const int MaxGames = 10;

        [HiddenInput]
        public string CreatorUserName { get; set; }

        [Display(Name = "Name")]
        [MaxLength(48)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Date")]
        public DateTime? Date { get; set; }

        [Display(Name = "Place")]
        [MaxLength(64)]
        public string Place { get; set; }

        [Display(Name = "Planned games")]
        [Remote("IsGameListValid", "GameEvent", HttpMethod = "Get")]
        [Required]
        public string Games { get; set; }

        internal static IEnumerable<string> NormalizeGameList(string gameList) =>
            gameList.Split('\n').Select(g => g.Trim()).Where(g => !string.IsNullOrEmpty(g));

        internal CreateGameEventDto ToDto() =>
            new CreateGameEventDto(CreatorUserName, Name, Date, Place, NormalizeGameList(Games));
    }
}