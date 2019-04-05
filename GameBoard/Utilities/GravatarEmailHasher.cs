using System.Security.Cryptography;
using System.Text;

namespace GameBoard.Utilities
{
    public static class GravatarEmailHasher
    {
        private static readonly MD5 Md5Hash = MD5.Create();

        public static string CalculateGravatarEmailHash(string email)
        {
            var data = Encoding.UTF8.GetBytes(email.Trim());
            var hashBytes = Md5Hash.ComputeHash(data);

            var builder = new StringBuilder();

            foreach(var @byte in hashBytes)
            {
                builder.Append(@byte.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
