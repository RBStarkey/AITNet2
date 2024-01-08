using AitNet2.Models;
using System.Text.RegularExpressions;

namespace AitNet2.Utilities
{
    public class FormatName
    {
        public NameFormatResult Format(string rawName)
        {
            NameFormatResult result = new NameFormatResult();
            result.ErrorMessage = string.Empty;

            string formattedName = string.Empty;
            string postNominal = string.Empty;
            if (rawName == null || rawName.Length == 0)
            {
                result.FormattedName = string.Empty;
                result.ErrorMessage = "Name is null";
                return result;
            }
            // Assume that first names will only ever be a single name or two names joined by a hyphen and lastnames will 
            // only ever be three elements prefix (St, von) followed by Name followed by postnominal (Jr, Sr, III)

            // Remove all chars except letters, hyphens, apostrophes and spaces and make lower case
            formattedName = CleanName(rawName);

            if (formattedName.Length == 0)
            {
                // Assume error
                result.FormattedName = rawName;
                result.ErrorMessage = "Name is not well formed";
                return result;
            }
            if (formattedName.Length == 1)
            {
                // Assume error
                result.FormattedName = formattedName;
                result.ErrorMessage = "Name is 1 character?";
                return result;
            }
            string[] elements = formattedName.Split(' '); // Assume the spaces are in the right place
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i] = elements[i].Trim();
            }
            switch (elements.Length)
            {
                case 0: // Error
                    result.FormattedName = string.Empty;
                    result.ErrorMessage = "No Name found";
                    break;

                case 1: // Single name, might be hyphenated
                    if (elements[0].Contains("-"))
                    {
                        result = FormatSingleNameWithHyphen(elements[0]);
                    }
                    else
                    {
                        result = FormatSingleName(elements[0]);
                    }

                    break;
                case 2:// prefix name, or name postnominal
                    result = FormatDoubleName(elements[0], elements[1]);
                    break;

                case 3: // prefix name postnominal
                    result = FormatTripleName(elements[0], elements[1], elements[2]);
                    break;

                default: // Error
                    result.FormattedName = rawName;
                    result.ErrorMessage = "Can't determine elements";
                    break;
            }
            return result;

        }
        private static string CleanName(string rawName)
        {
            string formattedName = Regex.Replace(rawName, @"[^A-Za-z\- ']+", "");
            // Remove control chars: tabs, newlines etc
            formattedName = Regex.Replace(formattedName, @"/\c\*/g", "");
            // Get rid of double, triple hyphens
            formattedName = formattedName.Replace("---", "-").Replace("--", "-");
            // Get rid of hyphens with spaces
            formattedName = formattedName.Replace("- ", "-").Replace(" -", "-");
            // Get rid of double, triple apostrophes
            formattedName = formattedName.Replace("''", "'");
            // Replace em dash with a hyphen
            formattedName = formattedName.Replace("—", "-");
            // Replace double spaces
            formattedName = formattedName.Replace("  ", " ");
            formattedName = formattedName.ToLower();
            return formattedName;

        }
        private static string FormatPrefix(string lhs)
        {
            string result = string.Empty;

            Regex regex = new Regex("^(von|el|la|ap|dell[a,e]|ben|d[a,e,i,u]|de[lr]|l[o,e]|van)");
            Match match = regex.Match(lhs);

            if (match.Success) //this is a prefix
            {
                if (lhs == "st")
                {
                    lhs = lhs.Substring(0, 1).ToUpper() + lhs.Substring(1);
                }
                result = lhs;
            }
            return result;
        }
        private static string FormatPostNominal(string rhs)
        {
            Regex regex = new Regex("^(jr|sr|i|ii|iii|iv|v)");
            Match matchRHS = regex.Match(rhs);
            if (matchRHS.Success)  // Is the rhs a postNominal e.g. Jr, Sr, III etc
            {
                if (rhs == "jr" || rhs == "sr")
                {
                    rhs = rhs.Substring(0, 1).ToUpper() + rhs.Substring(1);
                }
                else
                {
                    rhs = rhs.ToUpper();
                }
                return rhs;
            }
            return string.Empty;
        }
        private static NameFormatResult FormatSingleName(string rawName)
        {
            NameFormatResult result = new NameFormatResult();
            result.ErrorMessage = string.Empty;

            if (rawName.Length == 1)
            {
                rawName = rawName.Substring(0, 1).ToUpper();
            }
            else
            {
                rawName = rawName.Substring(0, 1).ToUpper() + rawName.Substring(1);
                // Is this a Mc or a Mac?
                if (rawName.Length > 1)
                {
                    if (rawName.Length > 2 && rawName.StartsWith("Mc"))
                    {
                        rawName = rawName.Substring(0, 2) + rawName.Substring(2, 1).ToUpper() + rawName.Substring(3);
                    }
                    if (rawName.Length > 3 && rawName.StartsWith("Mac"))
                    {
                        // Is this Mace or Mack etc?
                        // Exclude names ending in aciozj - typically these are Polish or Italian
                        Regex regex = new Regex("Mac[A-Za-z]{2,}[^aciozj]/gm");
                        Match match = regex.Match(rawName);
                        if (!match.Success)
                        {
                                // This is a Mac name
                            rawName = rawName.Substring(0, 3) + rawName.Substring(3, 1).ToUpper() + rawName.Substring(4);
                        }
                    }
                }
            }
            result.FormattedName = rawName;
            return result;
        }
        private static NameFormatResult FormatDoubleName(string lhs, string rhs)
        {
            NameFormatResult result = new NameFormatResult();
            result.ErrorMessage = string.Empty;
            string prefixResult = FormatPrefix(lhs);
            string postNominalResult = FormatPostNominal(rhs);

            if (prefixResult.Length > 0) // e.g. von Neumann
            {
                // RHS must be a Name does it have a hyphen?
                if (rhs.Contains('-'))
                {
                    result = FormatSingleNameWithHyphen(rhs);
                }
                else
                {
                    result = FormatSingleName(rhs);
                }
                result.FormattedName = prefixResult + " " + result.FormattedName;
                return result;
            }
            if (postNominalResult.Length > 0) // e.g. Smith-Tonks Jr
            {
                // LHS must be a Name does it have a hyphen?
                if (lhs.Contains('-'))
                {
                    result = FormatSingleNameWithHyphen(lhs);
                }
                else
                {
                    result = FormatSingleName(lhs);
                }
                result.FormattedName = result.FormattedName + " " + postNominalResult;
                return result;
            }
            if(lhs.ToLower() == "mc")
            {
                string firstLetter = lhs[0].ToString();
                lhs = firstLetter.ToUpper() + lhs[1].ToString();
                firstLetter = rhs[0].ToString();
                rhs = firstLetter.ToUpper() + rhs.Substring(1);
                result.FormattedName = lhs + rhs;
                return result;
            }
            else
            {
                result.ErrorMessage = "Two names no hyphen";
            }
            result.FormattedName = lhs + " " + rhs;
            return result;
        }
        private static NameFormatResult FormatTripleName(string lhs, string rawName, string rhs)
        {
            // Is this von Neumann III?
            NameFormatResult result = new NameFormatResult();
            result.ErrorMessage = string.Empty;

            string prefixResult = FormatPrefix(lhs);

            if (prefixResult.Length == 0)
            {
                result.FormattedName = lhs + " " + rawName + " " + rhs;
                result.ErrorMessage = "Left hand prefix not recognised";
                return result;
            }
            // If we get this far, is this a Name with postnominals?
            string postNominalResult = FormatPostNominal(rhs);
            if (postNominalResult.Length == 0)
            {
                result.FormattedName = lhs + " " + rawName + " " + rhs;
                result.ErrorMessage = "Right-hand post-nominals not recognised";
                return result;
            }
            if (rawName.Contains("-"))
            {
                result = FormatSingleNameWithHyphen(rawName);
            }
            else
            {
                result = FormatSingleName(rawName);
            }

            result.FormattedName = prefixResult + " " + result.FormattedName + " " + postNominalResult;
            return result;
        }
        private static NameFormatResult FormatSingleNameWithHyphen(string rawName)
        {
            NameFormatResult result = new NameFormatResult();
            result.ErrorMessage = string.Empty;

            string[] names = rawName.Split('-');
            if (names[0].Length == 0)
            {
                result.FormattedName = rawName;
                result.ErrorMessage = "Starts with a hyphen";
                return result;
            }
            if (names[1].Length == 0)
            {
                result.FormattedName = rawName;
                result.ErrorMessage = "Ends with a hyphen";
                return result;
            }
            names[0] = (names[0].Length > 0) ? names[0].Trim() : string.Empty;
            if (names.Length > 1)
            {
                names[1] = (names[1].Length > 0) ? names[1].Trim() : string.Empty;
            }
            string lhs = names[0].Substring(0, 1).ToUpper() + names[0].Substring(1);
            string rhs = string.Empty;

            if (names[1].Length == 1)
            {
                rhs = names[1].Substring(0, 1).ToUpper();
            }
            if (names[1].Length > 1)
            {
                rhs = names[1].Substring(0, 1).ToUpper() + names[1].Substring(1);
                // Is this O'Malley?
                int position = rhs.IndexOf("'");
                if (position > 0)
                {
                    rhs = rhs.Substring(0, position + 1)
                        + rhs.Substring(position + 1, 1).ToUpper()
                        + rhs.Substring(position + 1 + 1);
                }
            }
            if (rhs.Length == 0)
            {
                result.FormattedName = lhs;
            }
            else
            {
                result.FormattedName = lhs + "-" + rhs;
            }
            return result;
        }
    }
}
