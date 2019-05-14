using System;
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

        public async Task EditDescriptionTab(EditDescriptionTabDto editDescriptionTabDto)
        {
                var oldDescriptionTab = await _repository.DescriptionTabs
                    .SingleAsync(dt => dt.GameEventId == editDescriptionTabDto.GameEventId);

                oldDescriptionTab.EditDescriptionTab(editDescriptionTabDto);

                await _repository.SaveChangesAsync();
        }

        //This method seems pointless.
        public Task<DescriptionTabDto> GetDescriptionTabAsync(int gameEventId) =>
            _repository.DescriptionTabs
                .Where(dt => dt.GameEventId == gameEventId)
                .Select(dt => dt.ToDescriptionTabDto())
                .SingleOrDefaultAsync();
    }
}