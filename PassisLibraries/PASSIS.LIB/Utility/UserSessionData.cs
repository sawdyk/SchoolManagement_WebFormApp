using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PASSIS.LIB.Utility
{
    [Serializable]
    public class UserSessionData
    {
        public Int64 UserId
        { get; set; }
    }

    [Serializable]
    public class UserAdmissionSessionData
    {
        public Int64 Id
        { get; set; }
    }
}
