using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VMCTur.Common.Resources;

namespace VMCTur.Common.Validation
{
    public class PhoneNumberAssertionConcern
    {
        public static void AssertIsValid(string fone)
        {
            if (!Regex.IsMatch(fone, @"^(\(11\) [9][0-9]{4}-[0-9]{4})|(\(1[2-9]\) [5-9][0-9]{3}-[0-9]{4})|(\([2-9][1-9]\) [1-9][0-9]{3}-[0-9]{4})$", RegexOptions.IgnoreCase))
                throw new Exception(Errors.InvalidPhoneNumber);
        }
    }
}
