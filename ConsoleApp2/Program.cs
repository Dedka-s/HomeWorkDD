using System;
using System.Collections.Generic;
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

            var workWithText = new WorkWithText();

            var workWithTextType = typeof(WorkWithText);

                        var methods = workWithTextType.GetMethods(BindingFlags.Instance
                    | BindingFlags.Static
                    | BindingFlags.Public
                    | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            object[] argsFor = new object[1];
            argsFor[0] = allText;
     
                Dictionary<string, int> wordList = (Dictionary<string, int>)methods[0].Invoke(workWithText, argsFor);

                using (FileStream fs = new FileStream("E:\\textResalt.txt", FileMode.OpenOrCreate))
                {
                    using (TextWriter tw = new StreamWriter(fs))

                        foreach (KeyValuePair<string, int> kvp in wordList)
                        {
                            tw.WriteLine(string.Format("{0};{1}", kvp.Key, kvp.Value));
                        }
                }
                                
            }
            
    }
}