using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.Context
{
    public static class AppContext
    {
        private static readonly CurrentUserSession _CurrentUser = new CurrentUserSession();
        public static CurrentUserSession CurrentUser => _CurrentUser;

        public static void SetUser(int UserID, string UserName)
        {
            _CurrentUser.SetUser(UserID, UserName);
        }
    }
}
