using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Common.Resources;

namespace VMCTur.Common.Validation
{
    public class BirthdayAssertionConcern
    {
        public static void AssertIsValid(DateTime birthdate)
        {
            if (birthdate > DateTime.Today)
                throw new Exception(Errors.InvalidBirthDate);
        }
    }
}
