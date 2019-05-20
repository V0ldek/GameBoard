using System;
using System.Threading.Tasks;
using GameBoard.LogicLayer.EventTabs.Dtos;

namespace GameBoard.LogicLayer.EventTabs
{
    public class MockEventTabService : IEventTabService
    {
        public async Task EditDescriptionTab(EditDescriptionTabDto editDescriptionTabDto) =>
            await Console.Out.WriteAsync("Testy");
    }
}