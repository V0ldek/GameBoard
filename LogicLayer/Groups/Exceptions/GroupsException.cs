﻿using System;
using System.Runtime.Serialization;

namespace GameBoard.LogicLayer.Groups.Exceptions
{
    [Serializable]
    public class GroupsException : ApplicationException
    {
        public GroupsException()
        {
        }

        public GroupsException(string message) : base(message)
        {
        }

        public GroupsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Required by the serializable pattern. When adding stuff to this class consult:
        // https://blogs.msdn.microsoft.com/agileer/2013/05/17/the-correct-way-to-code-a-custom-exception-class/.
        protected GroupsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}