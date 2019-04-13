using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameSearch.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameSearch
{
    public sealed class GameSearchService : IGameSearchService
    {
        public Task<IEnumerable<GameDto>> GetGamesByName([NotNull] string gameNameInput)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GameDto>> GetGamesInEventAsync([NotNull] string gameEventId)
        {
            throw new NotImplementedException();
        }
    }
}
