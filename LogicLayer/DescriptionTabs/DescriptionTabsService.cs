using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.DescriptionTabs.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.LogicLayer.DescriptionTabs
{
    internal class DescriptionTabsService : IDescriptionTabsService
    {
        private readonly IGameBoardRepository _repository;

        public DescriptionTabsService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task EditDescriptionTabAsync(EditDescriptionTabDto editDescriptionTabDto)
        {
            var descriptionTab = await _repository.DescriptionTabs
                .SingleAsync(dt => dt.Id == editDescriptionTabDto.Id);

            descriptionTab.Description = editDescriptionTabDto.Description;

            await _repository.SaveChangesAsync();
        }

        public Task<DescriptionTabDto> GetDescriptionTabAsync(int id) => _repository.DescriptionTabs
            .Where(dt => dt.Id == id)
            .Select(dt => dt.ToDescriptionTabDto())
            .SingleAsync();
    }
}