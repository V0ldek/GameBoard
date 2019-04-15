using System;
using System.Runtime.Serialization;

namespace GameBoard.LogicLayer.Friends.Exceptions
{
    [Serializable]
    public class InvitingYourselfException : ApplicationException
    {
        public InvitingYourselfException()
        {
        }

        public InvitingYourselfException(string message) : base(message)
        {
        }

        public InvitingYourselfException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Required by the serializable pattern. When adding stuff to this class consult:
        // https://blogs.msdn.microsoft.com/agileer/2013/05/17/the-correct-way-to-code-a-custom-exception-class/.
        protected InvitingYourselfException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
