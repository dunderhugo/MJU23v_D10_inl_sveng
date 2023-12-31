﻿using System.Data;
using System.Threading.Channels;

namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        static List<SweEngGloss> dictionary;
        class SweEngGloss
        {
            public string word_swe, word_eng;
            public SweEngGloss(string word_swe, string word_eng)
            {
                this.word_swe = word_swe; this.word_eng = word_eng;
            }
            public SweEngGloss(string line)
            {
                string[] words = line.Split('|');
                this.word_swe = words[0]; this.word_eng = words[1];
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the dictionary app! Type 'help' for commands.");
            string DefaultFile = ChooseFile();
            do
            {
                Console.Write("> ");
                string[] argument = Console.ReadLine().ToLower().Split();
                string command = argument[0];
                if (command == "quit")
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
                else if (command == "help") HelpCommand();
                else if (command == "load") FileToLoad(DefaultFile, argument);
                else if (command == "list") ListWords();
                else if (command == "new") AddNewWord(argument);
                else if (command == "delete") DeleteTranslatedWord(argument);
                else if (command == "translate") TranslateWordsMethod(argument);
                else Console.WriteLine($"Unknown command: '{command}'");
            }
            while (true);
        }
        static string ChooseFile()
            //TODO, do so that you dont need to use the "load" command before you use "list"
        {
            Console.Write("Type what file you want to load: ");
            string fileLoad = Console.ReadLine();
            string DefaultFile = "..\\..\\..\\dict\\" + fileLoad + ".lis";
            return DefaultFile;
        }
        static void FileToLoad(string DefaultFile, string[] argument)
        {
            //FIXME, om fil ej finns
            //TODO, refactor method, duplicatged code
            try
            {
                if (argument.Length == 2)
                {
                    DefaultFile = argument[1];
                    using (StreamReader sr = new StreamReader("..\\..\\..\\dict\\" + DefaultFile + ".lis"))
                    {
                        dictionary = new List<SweEngGloss>(); // Empty it!
                        string line = sr.ReadLine();
                        while (line != null)
                        {
                            SweEngGloss gloss = new SweEngGloss(line);
                            dictionary.Add(gloss);
                            line = sr.ReadLine();
                        }
                    }
                }
                else if (argument.Length == 1)
                {
                    using (StreamReader sr = new StreamReader(DefaultFile))
                    {
                        dictionary = new List<SweEngGloss>(); // Empty it!
                        string line = sr.ReadLine();
                        while (line != null)
                        {
                            SweEngGloss gloss = new SweEngGloss(line);
                            dictionary.Add(gloss);
                            line = sr.ReadLine();
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found, try another filename!");
            }
        }
        static void TranslateWordsMethod(string[] argument)
        {
            if (argument.Length == 2)
            {
                string WordToTranslate = argument[1];
                TranslateWord(WordToTranslate);
            }
            else if (argument.Length == 1)
            {
                Console.WriteLine("Write word to be translated: ");
                string WordToTranslate = Console.ReadLine();
                TranslateWord(WordToTranslate);
            }
        }
        static void TranslateWord(string WordToTranslate)
        {
            bool found = false;
            foreach (SweEngGloss gloss in dictionary)
            {
                if (gloss.word_swe == WordToTranslate)
                {
                    Console.WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}");
                    found = true;
                }
                if (gloss.word_eng == WordToTranslate)
                {
                    Console.WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}");
                    found = true;
                }
            }
            if (!found)
            Console.WriteLine(WordToTranslate + " could not be found in this dictonary, try another word");
        }
        static void HelpCommand()
        {
            Console.WriteLine(" -----------------------------------------------------------------------------------------");
            Console.WriteLine("|                                        COMMANDS                                         |");
            Console.WriteLine("|  * quit      |-   Quits the program                                                     |");
            Console.WriteLine("|  * help      |-   List of the commands                                                  |");
            Console.WriteLine("|  * load      |-   Loads the files with translated words                                 |");
            Console.WriteLine("|  * list      |-   Lists the swedish & English words in console                          |");
            Console.WriteLine("|  * new       |-   Adds new word to translate, make sure you spell right!!               |");
            Console.WriteLine("|  * delete    |-   Deletes word in list                                                  |");
            Console.WriteLine("|  * translate |-   Translates word from swedish to english, or from english to swedish   |");
            Console.WriteLine("|                                                                                         |");
            Console.WriteLine(" -----------------------------------------------------------------------------------------");
        }
        static void AddNewWord(string[] argument)
        {
            //FIXME: You can input different characters, not only letters, for example: ¤%&/()=
            if (argument.Length == 3)
            {
                dictionary.Add(new SweEngGloss(argument[1], argument[2]));
            }
            else if (argument.Length == 1)
            {
                Console.WriteLine("Write word in Swedish: ");
                string SwedishWord = Console.ReadLine();
                Console.Write("Write word in English: ");
                string EnglishWord = Console.ReadLine();
                dictionary.Add(new SweEngGloss(SwedishWord, EnglishWord));
            }
        }

        static void DeleteTranslatedWord(string[] argument)
        {
            //TODO: Bekräfta för användaren vad som har tagits bort
            //TODO: Duplicatged code, refactor
            //FIXME: Felstavning av översättnigarna crashar programmet, även den perfekta användaren kan råka göra fel 
            if (argument.Length == 3)
            {
                int index = -1;
                for (int i = 0; i < dictionary.Count; i++)
                {
                    SweEngGloss gloss = dictionary[i];
                    if (gloss.word_swe == argument[1] && gloss.word_eng == argument[2])
                        index = i;
                }
                dictionary.RemoveAt(index);
            }
            else if (argument.Length == 1)
            {
                Console.WriteLine("Write word in Swedish: ");
                string SwedishInput = Console.ReadLine();
                Console.Write("Write word in English: ");
                string EnglishInput = Console.ReadLine();
                int index = -1;
                for (int i = 0; i < dictionary.Count; i++)
                {
                    SweEngGloss gloss = dictionary[i];
                    if (gloss.word_swe == SwedishInput && gloss.word_eng == EnglishInput)
                        index = i;
                }
                dictionary.RemoveAt(index);
            }
        }
        static void ListWords()
        {
            try
            {
                foreach (SweEngGloss gloss in dictionary)
                {
                    Console.WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("No file is loaded. Type 'load' on command line\n" +
                    "before you use 'list' command!");
            }

        }
    }
}