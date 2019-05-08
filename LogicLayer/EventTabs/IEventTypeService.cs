using System.Threading.Tasks;
using GameBoard.LogicLayer.EventTabs.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.EventTabs
{
    public interface IEventTypeService
    {
        Task CreateGameEventDescriptionTabAsync([NotNull] CreateDescriptionTabDto createDescriptionTabDto);

        //It's enough for current sprint, however this solution is not extensible.
        Task<DescriptionTabDto> GetDescriptionTab(int gameEventId);

        //This one looks more flexible
        Task<GameEventTabsDto> GetGameEventTabs(int gameEventId);
    }
}