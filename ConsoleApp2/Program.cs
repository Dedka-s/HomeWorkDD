using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using MyLibraryForDD;


namespace ConsoleApp2
{
    class Program
    {

        static void Main(string[] args)
        {
            
            string allText = File.ReadAllText("E:\\text.txt");
            
            //Однопоточный режим
            long freq = Stopwatch.Frequency;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var workWithText = new WorkWithText();

            var workWithTextType = typeof(WorkWithText);

            var methods = workWithTextType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            object[] argsFor = new object[1];
            argsFor[0] = allText;
            Dictionary<string, int> wordList = null;
            for (int k = 0; k < methods.Length; k++)
            {
                if (methods[k].Name.Equals("GetDictionary"))
                {
                    wordList = (Dictionary<string, int>)methods[k].Invoke(workWithText, argsFor);
                }
            }
     
                using (FileStream fs = new FileStream("E:\\textResalt.txt", FileMode.OpenOrCreate))
                {
                    using (TextWriter tw = new StreamWriter(fs))

                        foreach (KeyValuePair<string, int> kvp in wordList)
                        {
                            tw.WriteLine(string.Format("{0};{1}", kvp.Key, kvp.Value));
                        }
                }
            stopwatch.Stop();
            double sec = (double)stopwatch.ElapsedTicks / freq;
            Console.WriteLine("Однопоточный режим - " + sec);

            //Многопоточный режим
            long freq1 = Stopwatch.Frequency;
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();

            var workWithText1 = new WorkWithText();

            Dictionary<string, int> wordList1 = workWithText1.GetDictionaryOnThread(allText);
            using (FileStream fs = new FileStream("E:\\textResalt1.txt", FileMode.OpenOrCreate))
            {
                using (TextWriter tw = new StreamWriter(fs))

                    foreach (KeyValuePair<string, int> kvp in wordList)
                    {
                        tw.WriteLine(string.Format("{0};{1}", kvp.Key, kvp.Value));
                    }
            }
            stopwatch1.Stop();
            double sec1 = (double)stopwatch1.ElapsedTicks / freq1;
            Console.WriteLine("Многопоточный режим - " + sec1);

            Console.ReadLine();
            }
            
    }
}