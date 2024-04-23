namespace PatientInfo.API.Utilities
{
    public static class CsvUtil
    {
        private const int HEADER_LENGTH = 5;

        public static bool HeaderValidation(string[] headers)
        {
            if (headers.Length == HEADER_LENGTH &&
                headers[0] == "Patient Name" &&
                headers[1] == "SSN" &&
                headers[2] == "Age" &&
                headers[3] == "Phone Number" &&
                headers[4] == "Status")
                return true;

            return false;
        }

        public static string[] SplitString(string input)
        {
            List<string> result = new();
            bool closingQuoteFlag = true;
            int startIndex = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '"')
                {
                    closingQuoteFlag = !closingQuoteFlag; // Toggle the flag everytime we see a double quote.
                }
                else if (input[i] == ',' && closingQuoteFlag)
                {
                    // If it's a comma and the double quote has been closed, add it to the list.
                    result.Add(input.Substring(startIndex, i - startIndex));
                    startIndex = i + 1;
                }
            }

            // Add the last substring.
            result.Add(input.Substring(startIndex));

            // Remove quotes for each item in the list.
            for (int i = 0; i < result.Count; i++)
            {
                result[i] = result[i].Trim('"');
            }

            return result.ToArray();
        }
    }
}
