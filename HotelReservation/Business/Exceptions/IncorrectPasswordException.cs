using System;

namespace Business.Exceptions
{
   public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException(string message)
            : base(message)
        { }
    }
}