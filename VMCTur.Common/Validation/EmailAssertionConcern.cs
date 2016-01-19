using System;
using System.Text.RegularExpressions;
using VMCTur.Common.Resources;

namespace VMCTur.Common.Validation
{
    public class EmailAssertionConcern
    {
        public static void AssertIsValid(string email)
        {
            if (!Regex.IsMatch(email, @"^(\(11\) [9][0-9]{4}-[0-9]{4})|(\(1[2-9]\) [5-9][0-9]{3}-[0-9]{4})|(\([2-9][1-9]\) [5-9][0-9]{3}-[0-9]{4})$", RegexOptions.IgnoreCase))
                throw new Exception(Errors.InvalidEmail);
        }
    }
}
