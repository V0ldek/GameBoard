//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Text;
//using GameBoard.DataLayer.Entities;
//using GameBoard.DataLayer.Repositories;
//using Xunit;

//namespace GameBoard.LogicLayer.UserSearch
//{
//    public class UserSearchServiceTests
//    {
//        public static TheoryData<ApplicationUser> UserData => new TheoryData<ApplicationUser>
//            {
//                {
//                    new ApplicationUser();
//                },
//                {
//                    new Duck("Jacuś", Color.Red);
//                },
//                {
//                    new Duck("Yellow", Color.Yellow);
//                },
//                {
//                    new Duck("Yellow", Color.Green);
//                }
//            }

//        private const string ConnectionString = "UserID=posgres;Password=postgres;Host=localhost;Port=5432;Database=duck_db";

//        [Fact]
//        public void SearchEngineSpeedTest()
//        {
//            ARRANGE
//            using (IGameBoardRepository repository = new GameBoardDbContext(ConnectionString))
//            {
//                repository.ApplicationUsers.Add(new ApplicationUser { UserName = "ZiOmEk", Email = "jj@jj" });
//                var search = new UserSearchService(repository);

//                var expectedName = duck.Name;
//                var color = duck.Color;

//                repository.GetRandomDuck().Returns(duck);

//                var systemUnderTest = new DuckGuesser(repository);

//                ACT

//               var resultName = systemUnderTest.GuessColor(color);

//                ASSERT
//                Assert.Equal(expectedName, resultName);
//            }
//        }

//        [Theory]
//        [MemberData(nameof(DuckData))]
//        public void GuessColor_WhenColorDoesNotMatchWithGivenDuck_ThrowsApplicationException(Duck duck)
//        {
//            ARRANGE
//           var color = duck.Color == Color.Yellow ? Color.Green : Color.Yellow;

//            var repository = Substitute.For<IRepository>();
//            repository.GetRandomDuck().Returns(duck);

//            var systemUnderTest = new DuckGuesser(repository);

//            ACT & ASSERT
//            Assert.Throws<ApplicationException>(() => systemUnderTest.GuessColor(color));
//        }
//    }
//}
