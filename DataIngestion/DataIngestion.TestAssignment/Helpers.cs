namespace DataIngestion.TestAssignment
{
    public static class Helpers
    {
        /// To Avoid empty strings values that can break the cast
        public static string[] ReplaceEmptyStringsWithNull(string[] array)
        {
            int index = 0;

            foreach (var value in array)
            {
                if (value == "")
                    array[index] = null;

                index++;
            }

            return array;
        }
    }
}