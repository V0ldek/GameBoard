using System;
using System.Runtime.Serialization;

namespace GameBoard.LogicLayer.Notifications.Exceptions
{
    [Serializable]
    public class SendGridNotificationSendException : ApplicationException
    {
        public SendGridNotificationSendException()
        {
        }

        public SendGridNotificationSendException(string message) : base(message)
        {
        }

        public SendGridNotificationSendException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Required by the serializable pattern. When adding stuff to this class consult:
        // https://blogs.msdn.microsoft.com/agileer/2013/05/17/the-correct-way-to-code-a-custom-exception-class/.
        protected SendGridNotificationSendException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}