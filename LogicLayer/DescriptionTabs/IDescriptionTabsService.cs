using System.Threading.Tasks;
using GameBoard.LogicLayer.EventTabs.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.EventTabs
{
    public interface IEventTypeService
    {
        Task CreateGameEventDescriptionTabAsync([NotNull] CreateDescriptionTabDto createDescriptionTabDto);

        Task<DescriptionTabDto> GetDescriptionTabAsync(int gameEventId);

        Task AddDescriptionTabAsync([NotNull] CreateDescriptionTabDto createDescriptionTabDto);

        Task EditDescriptionTab([NotNull] EditDescriptionTabDto editDescriptionTabDto);

    }
}