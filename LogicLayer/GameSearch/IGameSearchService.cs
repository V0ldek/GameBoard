using System.Collections.Generic;
using System.Threading.Tasks;
using GameBoard.LogicLayer.GameSearch.Dtos;
using JetBrains.Annotations;


namespace GameBoard.LogicLayer.GameSearch
{
    public interface IGameSearchService
    {
       
        Task<IEnumerable<GameDto>> GetGamesInEventAsync([NotNull] string gameEventId);
        
        Task<IEnumerable<GameDto>> GetGamesByName([NotNull] string gameNameInput);
    }
}
