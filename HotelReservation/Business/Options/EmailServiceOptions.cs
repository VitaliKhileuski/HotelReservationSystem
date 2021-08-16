using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class EmailServiceOptions
    {
        public const string EMAIL_SERVICE_OPTIONS = "EmailServiceOptions";
        public string AppEmailLogin { get; set; }
        public string AppEmailPassword { get; set; }
        public int VerificationCodeLifeTime { get; set; }
        public int VerificationCodeLength { get; set; }
        public string GoogleServer { get; set; }
    }
}
