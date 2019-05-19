using System.Threading.Tasks;
using GameBoard.LogicLayer.EventTabs.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.EventTabs
{
    public interface IEventTypeService
    {
        Task EditDescriptionTab([NotNull] EditDescriptionTabDto editDescriptionTabDto);
    }
}