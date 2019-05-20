using System.Threading.Tasks;
using GameBoard.LogicLayer.DescriptionTabs.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.DescriptionTabs
{
    public interface IDescriptionTabService
    {
        Task EditDescriptionTabAsync([NotNull] EditDescriptionTabDto editDescriptionTabDto);

        [NotNull]
        Task<DescriptionTabDto> GetDescriptionTabAsync(int id);
    }
}