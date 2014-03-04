using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTracManager
{
    public class RateLock
    {
        public DateTime LockDate;
        public DateTime RelockDate;
        public int LockTerm;
        public DateTime Expiration;
        public string LockBy;
    }
}
