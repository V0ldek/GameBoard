using System;
using System.Runtime.Serialization;

namespace GameBoard.LogicLayer.GameEventParticipations.Exceptions
{
    [Serializable]
    public class GameEventParticipationException : ApplicationException
    {
        public GameEventParticipationException()
        {
        }

        public GameEventParticipationException(string message) : base(message)
        {
        }

        public GameEventParticipationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Required by the serializable pattern. When adding stuff to this class consult:
        // https://blogs.msdn.microsoft.com/agileer/2013/05/17/the-correct-way-to-code-a-custom-exception-class/.
        protected GameEventParticipationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}