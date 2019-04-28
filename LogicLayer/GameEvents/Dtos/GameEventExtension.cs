using GameBoard.DataLayer.Entities;
using GameBoard.LogicLayer.UserSearch.Dtos;
using System.Threading.Tasks;
using System.Linq;



namespace GameBoard.LogicLayer.GameEventLogic.Dtos
{
    public static class GameEventExtension
    {
        public static GameEventDto ToGameEventDto(this GameEvent gameEvent)
        {
            return new GameEventDto(
            gameEvent.Id.ToString(),
            gameEvent.EventName.ToString(),
            gameEvent.MeetingTime,
            gameEvent.Place.ToString(),
            gameEvent.Games.Select(g => new string(g.Name)),
            gameEvent.Participations.Select(inv => inv.Paticipant.ToUserDto())
            );
        }
 
    }
}
