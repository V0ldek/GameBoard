using System.Threading.Tasks;
using GameBoard.LogicLayer.DescriptionTabs.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.DescriptionTabs
{
    public interface IDescriptionTabsService
    {
        Task EditDescriptionTabAsync([NotNull] EditDescriptionTabDto editDescriptionTabDto);

        Task<DescriptionTabDto> GetDescriptionTabAsync(int gameEventId);
    }
}