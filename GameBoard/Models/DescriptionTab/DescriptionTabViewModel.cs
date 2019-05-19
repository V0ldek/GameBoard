using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace GameBoard.Models.EventTab
{
    public class DescriptionTabViewModel
    {
        [CanBeNull]
        public string Description { get; set; }
    }
}
