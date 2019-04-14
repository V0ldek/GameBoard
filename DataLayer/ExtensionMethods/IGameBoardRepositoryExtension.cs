using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBoard.DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.DataLayer.ExtensionMethods
{
    public static class IGameBoardRepositoryExtension
    {
        public static Task<string> GetUserIdByUsername(this IGameBoardRepository repository, string username)
        {
            var normalizedUserName = username.ToUpper();

            return repository.ApplicationUsers
                .Where(u => u.NormalizedUserName == normalizedUserName)
                .Select(u => u.Id)
                .SingleAsync();
        }
    }
}
