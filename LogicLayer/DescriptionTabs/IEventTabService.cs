using System.Threading.Tasks;
using GameBoard.LogicLayer.EventTabs.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.EventTabs
{
    public interface IEventTabService
    {
        Task EditDescriptionTab([NotNull] EditDescriptionTabDto editDescriptionTabDto);
    }
}