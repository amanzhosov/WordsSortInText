using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WordsSortInText
{
    class WordsSortInText
    {
        static private string[] splits = { " - ", " ", ".", ",", ":", ";", "!", "?", "(", ")", "\"", "\r\n" };

        static void Main(string[] args)
        {
            Dictionary<string, int> countedWords = new Dictionary<string, int>();        

            try
            {
                string sourcePath, resultPath;
                int sortType;
                WordsSortInText.getDataFromUser(out sourcePath, out resultPath, out sortType);
                countedWords = WordsSortInText.getCountedWords(sourcePath);
                WordsSortInText.sortAndWriteWords(resultPath, sortType, countedWords);                

                Console.WriteLine("Sorting is succesfully completted.");
                Console.WriteLine("Press any key.");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Sorting is failed: " + e.Message);
                Console.ReadKey();
            }
        }

        static void getDataFromUser(out string source, out string result, out int type)
        {
            Console.WriteLine("Enter source file or press Enter for default value (./files/source.txt):");
            source = (source = Console.ReadLine().Trim()).Length > 0 ? source : @"./files/source.txt";
            Console.WriteLine("Enter result file or press Enter for default value (./files/result.txt):");
            result = (result = Console.ReadLine().Trim()).Length > 0 ? result : @"./files/result.txt";
            Console.WriteLine("Enter sorting type:");
            Console.WriteLine("1 - Sorting by Count ASC");
            Console.WriteLine("2 - Sorting by Count DESC");
            Console.WriteLine("3 - Sorting by Name ASC");
            Console.WriteLine("4 - Sorting by Name DESC");
            while (!int.TryParse(Console.ReadLine().Trim(), out type) || type < 1 || type > 4)
            {
                Console.WriteLine("You must type number from 1 to 4!");
            }
        }

        static Dictionary<string, int> getCountedWords(string filePath)
        {
            Dictionary<string, int> cWords = new Dictionary<string, int>();

            using (StreamReader sr = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read), Encoding.Default))
            {
                string text = sr.ReadToEnd().Trim();
                string[] words = text.ToLower().Split(WordsSortInText.splits, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    bool isRepeat = false;
                    if (cWords.ContainsKey(word))
                    {
                        cWords[word]++;
                        isRepeat = true;
                    }

                    if (!isRepeat)
                    {
                        cWords.Add(word, 1);
                    }
                }
            }
            return cWords;
        }

        static void sortAndWriteWords(string filePath, int sortType, Dictionary<string, int> countedWords)
        {
            using (StreamWriter sw = new StreamWriter(new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write), Encoding.Default))
            {
                switch (sortType)
                {
                    case 1:
                        countedWords = countedWords.OrderBy(word => word.Value).ThenBy(word => word.Key).ToDictionary(word => word.Key, word => word.Value);
                        break;
                    case 2:
                        countedWords = countedWords.OrderByDescending(word => word.Value).ThenBy(word => word.Key).ToDictionary(word => word.Key, word => word.Value);
                        break;
                    case 3:
                        countedWords = countedWords.OrderBy(word => word.Key).ToDictionary(word => word.Key, word => word.Value);
                        break;
                    case 4:
                        countedWords = countedWords.OrderByDescending(word => word.Key).ToDictionary(word => word.Key, word => word.Value);
                        break;
                }
                foreach (var countedWord in countedWords)
                {
                    sw.WriteLine(countedWord.Key + " - " + countedWord.Value);
                }
            }
        }
    }
}
