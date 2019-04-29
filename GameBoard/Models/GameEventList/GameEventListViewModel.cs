using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBoard.Models.GameEventList
{
    public sealed class GameEventListViewModel
    {
        public IEnumerable<GameEventListItemViewModel> Accepted { get; }

        public IEnumerable<GameEventListItemViewModel> Pending { get; }

        public IEnumerable<GameEventListItemViewModel> Created { get; }

        public GameEventListViewModel(
            IEnumerable<GameEventListItemViewModel> accepted,
            IEnumerable<GameEventListItemViewModel> pending,
            IEnumerable<GameEventListItemViewModel> created)
        {
            Accepted = accepted;
            Pending = pending;
            Created = created;
        }
    }
}