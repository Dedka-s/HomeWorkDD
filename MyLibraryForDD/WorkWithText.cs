using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibraryForDD
{
    public class WorkWithText
    {
             static object locker = new object();
             static Dictionary<string, int> wordListStream = new Dictionary<string, int>();
        public static void GetOnThread(object paramObj)
        {
            Regex reg_exp = new Regex("[^a-zA-Zа-яА-Я]");
            String p = (String)paramObj;
            p = reg_exp.Replace(p, " ");
            string[] words = p.Split(
            new char[] { ' ' },
            StringSplitOptions.RemoveEmptyEntries);
            
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = words[i].ToLower();
                lock (locker)
                {
                    if (wordListStream.ContainsKey(words[i]))
                    {
                        wordListStream[words[i]]++;
                    }
                    else
                    {
                       
                        wordListStream.Add(words[i], 1);
                    }
                }
            }
        }

        public Dictionary<string, int> GetDictionaryOnThread(String allText)
        {
            int i = allText.Length / 2;
            while (allText[i] != ' ') {
                i++;
            }

            Thread myThread1 = new Thread(new ParameterizedThreadStart(GetOnThread));
            myThread1.Start(allText.Substring(0, allText.Length - i));
            
            
            Thread myThread2 = new Thread(new ParameterizedThreadStart(GetOnThread));
            myThread2.Start(allText.Substring(i));

            myThread1.Join();
            myThread2.Join();

            wordListStream = wordListStream.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return wordListStream;
        }

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
