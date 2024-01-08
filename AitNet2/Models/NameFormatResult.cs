using System;

namespace AitNet2.Models
{
    public class NameFormatResult
    {
        public string FormattedName { get; set; }
        public string ErrorMessage { get; set; }

        public void WriteResult()
        {
            if (ErrorMessage.Length == 0)
            {
                Console.WriteLine("No error: " + FormattedName.ToString());
            }
            else
            {
                Console.WriteLine(ErrorMessage.ToString());
            }
        }
    }
}
