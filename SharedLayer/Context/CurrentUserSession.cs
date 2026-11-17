using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.Context
{
    public class CurrentUserSession
    {
        public int? UserID {get; private set;}
        public string UserName {get; private set;}

        internal CurrentUserSession()
        {
            Clear();
        }

        public bool IsAuthenticated => UserID.HasValue;

        internal void SetUser(int UserID, string UserName)
        {
            this.UserID = UserID;
            this.UserName = UserName;
        }

        public void Clear()
        {
            UserID = null;
            UserName = null;
        }
    }
}
