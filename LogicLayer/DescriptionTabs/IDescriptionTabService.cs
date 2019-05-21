using System.Threading.Tasks;
using GameBoard.LogicLayer.DescriptionTabs.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.DescriptionTabs
{
    public interface IDescriptionTabService
    {
        
        Task EditDescriptionTabAsync([NotNull] EditDescriptionTabDto editDescriptionTabDto);
        
        [ItemNotNull]
        // Throws an InvalidOperationException if Description is not found.
        Task<DescriptionTabDto> GetDescriptionTabAsync(int id);
    }
}