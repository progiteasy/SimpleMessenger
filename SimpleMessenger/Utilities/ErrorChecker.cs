using System.Collections.Generic;
using System.Linq;

namespace SimpleMessenger.Utilities
{
    public static class ErrorChecker
    {
        public static string GetFirstErrorMessageOrDefault(Dictionary<string, bool> errorsToCheck)
            => errorsToCheck.FirstOrDefault(error => error.Value).Key;
    }
}
