using System.Collections.Generic;

namespace HelpDesk.Web.Services
{
    public static class TextAnalizer
    {
        /// <summary>
        /// Get string from text.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>string</returns>
        public static string Between(this string value, string a, string b)
        {
            int startIndex1 = value.IndexOf(a);
            int startIndex2 = startIndex1 + a.Length;

            var tempIndex = startIndex2;

                int num = value.IndexOf(b, tempIndex);
                if (startIndex1 == -1 || num == -1)
                    return "";

                if (startIndex2 >= num)
                    return "";

            return value.Substring(startIndex2, num - startIndex2);
        }

        /// <summary>
        /// Get all similar bloks from text.                
        /// </summary>
        /// <param name="enterField"></param>
        /// <param name="exitField"></param>
        /// <param name="message"></param>
        /// <returns>List blocks</returns>        
        public static List<string> FindAllBlocks(this string message, string enterField, string exitField)
        {
            List<string> blocks = new();
            bool find = true;

            string tempString = message;

            while (find)
            {
                if (tempString.Contains(enterField))
                {
                    var firstblock = tempString.Substring(0, tempString.IndexOf(enterField));
                    blocks.Add(firstblock);
                    var block = tempString.Between(enterField, exitField);
                    blocks.Add(enterField + block + exitField);
                    tempString = tempString.Substring(tempString.IndexOf(block) + block.Length + exitField.Length);
                }
                else
                {
                    blocks.Add(tempString);
                    find = false;
                }
            }
            return blocks;
        }
    }
}
