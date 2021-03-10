using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyLibraryForDD
{
    public class WorkWithText
    {
        private Dictionary<string, int> GetDictionary (String allText)
        {
            Regex reg_exp = new Regex("[^a-zA-Zа-яА-Я]");
            allText = reg_exp.Replace(allText, " ");

            string[] words = allText.Split(
            new char[] { ' ' },
            StringSplitOptions.RemoveEmptyEntries);
            var wordList = new Dictionary<string, int>();
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = words[i].ToLower();
                if (wordList.ContainsKey(words[i]))
                {
                    wordList[words[i]]++;
                }
                else
                {
                    wordList.Add(words[i], 1);
                }
            }

            wordList = wordList.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return wordList;
        }
    }
}
