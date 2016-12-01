using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCTur.Domain.Entities.Users
{
    public class UserLog
    {
        public int Id { get; private set; }
        public DateTime Date { get; private set; }
        public string ActionDescription { get; private set; }
        public string User { get; private set; }

        #region Ctor

        protected UserLog() { } 

        public UserLog(DateTime date, string actionDescription, string user)
        {
            Date = date;
            ActionDescription = actionDescription;
            User = user;

        }


        #endregion

    }
}
