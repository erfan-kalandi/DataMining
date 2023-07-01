using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
namespace Final_Project
{
    public class Program
    {
        public static Stopwatch s = new Stopwatch();
        public static double memory = 0.0;
        public static List<List<string>> PapersLines;
        public static Dictionary<string, List<string>> GenreLines;
        public static void Main()
        {
            string input = "-1";

            Storage(out PapersLines, out GenreLines);
            while (input != "0")
            {
                Console.WriteLine("Please Enter part Id to run(1 to 8): ");
                input = Console.ReadLine();
                Process proc = Process.GetCurrentProcess();
                switch (input)
                {
                    case "0":
                        input = "0";
                        break;
                    case "1":
                        Main1();
                        break;
                    case "2":
                        Main2();
                        break;
                    case "3":
                        Main3();
                        break;
                    case "4":
                        Main4();
                        break;
                    case "5":
                        Main5();
                        break;
                    case "6":
                        Main6();
                        break;
                    case "7":
                        Main7();
                        break;
                }
                if (s.Elapsed.TotalSeconds >= 0.00111)
                {
                    memory = Math.Round((double)proc.PrivateMemorySize64 / (1024 * 1024), 2);
                    proc.Dispose();
                    Console.WriteLine($"Elapsed Time = {s.Elapsed.TotalSeconds} Seconds");
                    Console.WriteLine($"Memory Usage = {memory} Megabyte");
                    Console.WriteLine("********************************************");
                }
                s.Reset();
            }
        }

        public static void Main1()
        {
            Console.WriteLine("word to search:");
            string word = Console.ReadLine();
            Console.WriteLine("Minimum number of iterations:");
            int min = int.Parse(Console.ReadLine());
            s.Start();
            Console.WriteLine($"Output = {Part_1(word, min).Count()}");
        }
        public static void Main2()
        {
            Console.WriteLine("Genre to search:");
            string Genre = Console.ReadLine();

            s.Start();
            Console.WriteLine($"Output = {Part_2(Genre, 0, 100000).Count}");
        }
        public static void Main3()
        {
            Console.WriteLine("reference paper:");
            int Paper = int.Parse(Console.ReadLine());
            Console.WriteLine("Number of common words(N):");
            int N = int.Parse(Console.ReadLine());
            Console.WriteLine("Intended genre(G):");
            string Genre = Console.ReadLine();

            s.Start();
            var toks = Part_3(Paper, Genre, N, 5, 10000);
            foreach (var t in toks)
                Console.Write(string.Join(" ", t) + " ");
            Console.WriteLine($"Output = {toks.Count}");
        }
        public static void Main4()
        {
            Console.WriteLine("reference paper:");
            int Paper = int.Parse(Console.ReadLine());
            Console.WriteLine("Number of common words(N):");
            int N = int.Parse(Console.ReadLine());

            s.Start();
            var toks = Part_4(Paper, N);
            foreach (var t in toks)
                Console.Write(string.Join(" ", t) + " ");
            Console.WriteLine($"Output = {toks.Count}");
        }

        public static void Main5()
        {
            Console.WriteLine("reference paper:");
            int Paper = int.Parse(Console.ReadLine());
            Console.WriteLine("Number of common words(N):");
            int N = int.Parse(Console.ReadLine());
            Console.WriteLine("Number of common genres(M):");
            int M = int.Parse(Console.ReadLine());
            s.Start();
            var toks = Part_5(Paper, N, M, 5, 1000);
            Console.WriteLine($"Output = {toks.Count}");
        }

        public static void Main6()
        {
            Console.WriteLine("genre:");
            string Genre = Console.ReadLine();
            Console.WriteLine("Number of common words:");
            int N = int.Parse(Console.ReadLine());
            s.Start();
            var output = Part_6(N, Genre);
            foreach (var v in output)
            {
                Console.WriteLine(string.Join(" ", v));
            }
            Console.WriteLine($"Output = {output.Count}");
        }

        public static void Main7()
        {
            Console.WriteLine("genre:");
            string Genre = Console.ReadLine();
            Console.WriteLine("Number of common words:");
            int N = int.Parse(Console.ReadLine());
            Console.WriteLine("word to search:");
            string word = Console.ReadLine();
            Console.WriteLine("Minimum number of iterations:");
            int min = int.Parse(Console.ReadLine());
            s.Start();
            Part_7(Genre, N, word, min);
        }


        private static void Storage(out List<List<string>> PapersLines, out Dictionary<string, List<string>> GenreLines)
        {
            PapersLines = new List<List<string>>();
            GenreLines = new Dictionary<string, List<string>>();
            List<string> genres = new List<string> { "Colors", "Crime", "food", "computer", "Organs","Adjectives","animal",
            "BinaryNumber","CubeNumber","Fruits","Furnitures","Music","Politics","PrimeNumber","Sports","SquareNumber","temperature","time"};

            for (int i = 0; i < 100000; i++)
                PapersLines.Add(File.ReadAllLines(@$"..\DataSet\Text_{i}.txt").ToList());

            foreach (var g in genres)
                GenreLines[g] = File.ReadAllText(@$"..\Genres\{g}.txt").Split("\r\n").ToList();
        }

        private static List<int> Part_1(string word, int number)
        {
            List<int> p = new List<int>();
            for (int i = 0; i < 100000; i++)
            {
                var toks = PapersLines[i];
                int amount = 0;
                int idx = toks.FindIndex(l => l.Contains(word));
                if (idx != -1)
                {
                    for (int j = idx; j < Math.Min(idx + 11, 99); j++)
                    {
                        amount += toks[j].Split().ToList().FindAll(w => w == word).Count();
                        if (amount >= number)
                        {
                            p.Add(i);
                            break;
                        }
                    }
                }
            }
            return p;
        }

        private static List<int> Part_2(string genre, int number, int c)
        {
            List<int> papers = new List<int> { };
            var genre_word = GenreLines[genre];

            for (int i = number * c; i < (number + 1) * c; i++)
            {
                var toks = PapersLines[i].Skip(44).First().Split();
                var s1 = toks[0]; var s2 = toks[1]; var s3 = toks[2]; var s4 = toks[3];
                if (genre_word.Contains(s1) & genre_word.Contains(s2)
                & genre_word.Contains(s3) & genre_word.Contains(s4))
                    papers.Add(i);
            }
            return papers;
        }

        private static List<int> Part_3(int Paper, string Genre, int n, int number, int c)
        {
            List<int> adjacent = new List<int> { };
            string[] words = FindWord(Paper, Genre);
            if (n <= 0)
            {
                for (int i = number * c; i < (number + 1) * c; i++)
                {
                    adjacent.Add(i);
                }
            }

            else
            {
                Dictionary<string, int> keys = new Dictionary<string, int> { };
                foreach (var w in words)
                {
                    if (keys.ContainsKey(w))
                        keys[w] += 1;
                    else
                        keys[w] = 1;
                }

                for (int i = number * c; i < (number + 1) * c; i++)
                {
                    var new_words = FindWord(i, Genre).ToList();
                    int counter = 0;
                    foreach (var w in keys.Keys)
                    {
                        if (new_words.Contains(w))
                        {
                            counter += Math.Min(keys[w], new_words.FindAll(c => c == w).Count);
                        }
                    }
                    if (counter >= n)
                    {
                        adjacent.Add(i);
                    }
                }
            }
            return adjacent;
        }

        private static List<int> Part_4(int Paper, int N)
        {
            var adjacent = new List<int>();
            foreach (var g in GenreLines.Keys)
            {
                var toks = Part_3(Paper, g, N, 4, 3000);
                adjacent.AddRange(toks);
            }
            return adjacent;
        }

        private static List<int> Part_5(int paper, int n, int m, int number, int c)
        {
            List<int> adjacent = new List<int>();
            
            for (int i = number * c; i <= (number + 1) * c; i++)
            {
                int count = 0;
                foreach (var g in GenreLines.Keys)
                {
                    if (IsCheckAdjacency(paper, i, g, n))
                        count++;
                    if (count > m)
                        break;
                }
                if (count == m)
                {
                    adjacent.Add(i);
                }
            }
            return adjacent;
        }

        private static List<List<int>> Part_6(int N, string Genre)
        {
            HashSet<int> V = new HashSet<int>(500);
            List<int>[] Graph = new List<int>[500];
            for (int i = 0; i < 500; i++)
                Graph[i] = new List<int>();
            Dictionary<int, Dictionary<string, int>> CommonWords = new Dictionary<int, Dictionary<string, int>>(500);
            for (int idx = 2500; idx < 3000; idx++)
            {
                var result = FindWord(idx, Genre);
                Dictionary<string, int> keys = new Dictionary<string, int> { };
                foreach (var w in result)
                {
                    if (keys.ContainsKey(w))
                        keys[w] += 1;
                    else
                        keys[w] = 1;
                }
                CommonWords[idx] = keys;
                V.Add(idx);
                for (int i = idx + 1; i < 3000; i++)
                {
                    var new_words = FindWord(i, Genre).ToList();
                    int counter = 0;
                    foreach (var w in keys)
                    {
                        if (new_words.Contains(w.Key))
                        {
                            counter += Math.Min(w.Value, new_words.FindAll(c => c == w.Key).Count);
                        }
                    }
                    if (counter >= N)
                    {
                        Graph[idx - 2500].Add(i - 2500);
                        Graph[i - 2500].Add(idx - 2500);
                    }
                }
            }
            List<List<int>> books = new List<List<int>>();
            while (V.Count > 0)
            {
                int Start = 0;
                foreach (int v in V)
                {
                    Start = v;
                    break;
                }
                Queue<int> BFS = new Queue<int>();
                BFS.Enqueue(Start - 2500);
                V.Remove(Start);
                var newbook = new List<int>();
                while (BFS.Count > 0)
                {
                    Start = BFS.Dequeue();
                    newbook.Add((Start + 2500));
                    foreach (int v in Graph[Start])
                    {
                        if (V.Contains(v + 2500))
                        {
                            BFS.Enqueue(v);
                            V.Remove(v + 2500);
                        }
                    }
                }
                if (newbook.Count > 1)
                    books.Add(newbook);
            }
            return books;
        }

        private static void Part_7(string? genre, int n, string? word, int min)
        {
            var allbooks = Part_6(n, genre);
            var output = BooksWithWord(allbooks, word, n);
            foreach (var v in output)
            {
                Console.WriteLine(string.Join(" ", v));
            }
            Console.WriteLine($"Output = {output.Count}");
        }

        private static List<List<int>> BooksWithWord(List<List<int>> allbooks, string word, int n)
        {
            List<List<int>> books = new List<List<int>>();
            foreach (var b in allbooks)
            {
                int ischecked = 0;
                foreach (var p in b)
                {
                    var toks = PapersLines[p];
                    int amount = 0;
                    int idx = toks.FindIndex(l => l.Contains(word));
                    if (idx != -1)
                    {
                        for (int j = idx; j < Math.Min(idx + 11, 99); j++)
                        {
                            amount += toks[j].Split().ToList().FindAll(w => w == word).Count();
                        }
                        ischecked += amount;
                    }
                }
                if (ischecked >= n)
                    books.Add(b);

            }
            return books;
        }

        private static bool IsCheckAdjacency(int paper, int i, string g, int n)
        {
            string[] words = FindWord(paper, g);
            Dictionary<string, int> keys = new Dictionary<string, int> { };
            foreach (var w in words)
            {
                if (keys.ContainsKey(w))
                    keys[w] += 1;
                else
                    keys[w] = 1;
            }

            var new_words = FindWord(i, g).ToList();
            int counter = 0;
            foreach (var w in keys.Keys)
            {
                if (new_words.Contains(w))
                {
                    counter += Math.Min(keys[w], new_words.FindAll(c => c == w).Count);
                }
            }
            if (counter >= n)
            {
                return true;
            }
            return false;
        }

        private static string[] FindWord(int Paper, string Genre)
        {
            string[] word = new string[] { };
            var toks = PapersLines[Paper];
            var Genre_word = GenreLines[Genre];
            for (int i = 0; i <= 88; i += 11)
            {
                var Line = toks[i].Split();
                var s1 = Line[0]; var s2 = Line[1]; var s3 = Line[2]; var s4 = Line[4]; var s5 = Line[5];

                if (Genre_word.Contains(s1) & Genre_word.Contains(s2)
                & Genre_word.Contains(s3) & Genre_word.Contains(s4) & Genre_word.Contains(s5))
                {
                    word = toks.Skip(i).Take(11).SelectMany(s => s.Split()).ToArray();
                }
            }
            return word;
        }
    }
}