using System;
using System.Runtime.Serialization;

namespace GameBoard.LogicLayer.Friends.Exceptions
{
    [Serializable]
    public class FriendRequestAlreadyFinalizedException : ApplicationException
    {
        public FriendRequestAlreadyFinalizedException()
        {
        }

        public FriendRequestAlreadyFinalizedException(string message) : base(message)
        {
        }

        public FriendRequestAlreadyFinalizedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Required by the serializable pattern. When adding stuff to this class consult:
        // https://blogs.msdn.microsoft.com/agileer/2013/05/17/the-correct-way-to-code-a-custom-exception-class/.
        protected FriendRequestAlreadyFinalizedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}