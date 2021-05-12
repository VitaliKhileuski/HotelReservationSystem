using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class TokenModel
    {
        public string Token { get; set; }
        public virtual UserModel User { get; set; }
    }
}
