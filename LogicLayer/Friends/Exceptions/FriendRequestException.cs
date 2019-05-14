using System;
using System.Runtime.Serialization;

namespace GameBoard.LogicLayer.Friends.Exceptions
{
    [Serializable]
    public class FriendRequestException : ApplicationException
    {
        public FriendRequestException()
        {
        }

        public FriendRequestException(string message) : base(message)
        {
        }

        public FriendRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Required by the serializable pattern. When adding stuff to this class consult:
        // https://blogs.msdn.microsoft.com/agileer/2013/05/17/the-correct-way-to-code-a-custom-exception-class/.
        protected FriendRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}