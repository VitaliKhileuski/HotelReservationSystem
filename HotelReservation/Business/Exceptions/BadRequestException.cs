using System;
using System.Collections.Generic;
using System.Text;
using Org.BouncyCastle.Asn1.X509;

namespace Business.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        { }
    }
}
