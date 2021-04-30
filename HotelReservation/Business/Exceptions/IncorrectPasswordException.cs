using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Exceptions
{
   public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException(string message)
            : base(message)
        { }
    }
}
