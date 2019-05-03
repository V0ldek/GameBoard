using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.GameEvents.Dtos;
using GameBoard.LogicLayer.UserSearch.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents
{
    class GameEventService : IGameEventService
    {
        private readonly IGameBoardRepository _repository;

        public GameEventService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public Task CreateGameEventAsync([NotNull] CreateGameEventDto requestedGameEvent)
        {
            throw new NotImplementedException();
        }

        public Task DeleteGameEventAsync(int gameEventId)
        {
            throw new NotImplementedException();
        }

        public Task EditGameEventAsync([NotNull] EditGameEventDto editedEvent)
        {
            throw new NotImplementedException();
        }

        public Task<GameEventListDto> GetAccessibleGameEventsAsync([NotNull] string userName)
        {
            var maciek = new UserDto("maciek", "NacMad", "macnad@gmail.com");
            var zochu = new UserDto("zochu", "Żochużochużochużochu", "żochu@żochu.żochu");
            var voldek = new UserDto("v0ldie", "V0ldek", "registermen@gmail.com");
            var johny = new UserDto("johny", "Johny", "johny@gmail.com");

            var gameEvents = new GameEventListDto(
                new List<GameEventListItemDto>
                {
                    new GameEventListItemDto(1, "Gra w gry", maciek),
                    new GameEventListItemDto(1, "Gra w gry", maciek),
                    new GameEventListItemDto(1, "Gra w gry", maciek),
                    new GameEventListItemDto(1, "Gra w gry", maciek),
                    new GameEventListItemDto(1, "Gra w gry", maciek),
                    new GameEventListItemDto(1, "Gra w gry", maciek),
                    new GameEventListItemDto(1, "Gra w gry", maciek),
                    new GameEventListItemDto(1, "Gra w gry", maciek),
                    new GameEventListItemDto(1, "Gra w gry", maciek),
                    new GameEventListItemDto(1, "Gra w gry", maciek),
                    new GameEventListItemDto(1, "Gra w gry", maciek),
                    new GameEventListItemDto(1, "Gra w gry", maciek),
                    new GameEventListItemDto(1, "Gra w gry", maciek),
                    new GameEventListItemDto(1, "Gra w gry", maciek),
                    new GameEventListItemDto(2, "Gra w grę", maciek)
                },
                new List<GameEventListItemDto>
                {
                    new GameEventListItemDto(3, "Będziemy grali w grę", johny)
                },
                new List<GameEventListItemDto>
                {
                    new GameEventListItemDto(4, "Moja gra", voldek)
                });

            return Task.FromResult(gameEvents);
        }

        public Task<GameEventDto> GetGameEventAsync(int gameEventId)
        {
            var maciek = new UserDto("maciek", "NacMad", "macnad@gmail.com");
            var zochu = new UserDto("zochu", "Żochużochużochużochu", "żochu@żochu.żochu");
            var voldek = new UserDto("v0ldie", "V0ldek", "registermen@gmail.com");
            var johny = new UserDto("johny", "Johny", "johny@gmail.com");

            switch (gameEventId)
            {
                case 1:
                    return Task.FromResult(
                        new GameEventDto(
                            1,
                            "Gra w gry",
                            new DateTime(2017, 05, 22),
                            "u Nadola",
                            new List<string>
                            {
                                "Gra o Tron",
                                "Iąę"
                            },
                            maciek,
                            new List<UserDto>(),
                            new List<UserDto>
                            {
                                voldek
                            }));
                case 2:
                    return Task.FromResult(
                        new GameEventDto(
                            2,
                            "Gra w grę",
                            new DateTime(2016, 10, 14),
                            "u Nadola",
                            new List<string>
                            {
                                "Terraformacja Marka",
                            },
                            maciek,
                            new List<UserDto>
                            {
                                zochu,
                                zochu,
                                zochu,
                                zochu,
                                zochu,
                                zochu,
                                zochu,
                                zochu,
                                zochu,
                                zochu,
                                zochu,
                                zochu,
                                zochu,
                                zochu,
                                zochu,
                                zochu,
                                zochu
                            },
                            new List<UserDto>
                            {
                                voldek,
                                voldek,
                                voldek,
                                voldek,
                                voldek,
                                voldek,
                                voldek,
                                voldek,
                                voldek,
                                voldek,
                                voldek,
                                voldek,
                                voldek,
                                voldek,
                                voldek,
                                voldek,
                                voldek,
                                johny
                            }));
                case 3:
                    return Task.FromResult(
                        new GameEventDto(
                            3,
                            "Będziemy grali w grę",
                            new DateTime(2017, 05, 22),
                            "Szobera 6/114",
                            new List<string>
                            {
                                "Twilight Imperium"
                            },
                            johny,
                            new List<UserDto>
                            {
                                voldek
                            },
                            new List<UserDto>
                            {
                                zochu,
                                maciek
                            }));
                case 4:
                    return Task.FromResult(
                        new GameEventDto(
                            4,
                            "Będziemy grali w grę",
                            new DateTime(2017, 05, 22),
                            "Moja gra",
                            new List<string>
                            {
                                "Trójkowy Konflikt"
                            },
                            voldek,
                            new List<UserDto>
                            {
                                maciek
                            },
                            new List<UserDto>
                            {
                                johny
                            }));
                default:
                    throw new NotImplementedException();
            }
        }

        public Task<GameEventPermission> GetGameEventPermissionByUserAsync(int gameEventId, [NotNull] string userName)
        {
            throw new NotImplementedException();
        }

        public Task RejectGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserName)
        {
            throw new NotImplementedException();
        }

        public Task AcceptGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserName)
        {
            throw new NotImplementedException();
        }

        public Task SendGameEventInvitationAsync(int gameEventId, [NotNull] string userName)
        {
            throw new NotImplementedException();
        }
    }
}