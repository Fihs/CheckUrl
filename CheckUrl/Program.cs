using System;
using System.IO;
using System.Net.Http;

namespace CheckUrl
{
    internal class Program
    {
        #region Fields

        private const string defaultPath = "data.txt";

        #endregion Fields

        // regex: "http.[^",]*"

        #region Methods

        private static void Main(string[] args)
        {
            if (args is null || args.Length == 0)
            {
                args = new string[] { defaultPath };
            }

            Check(args);

            Console.Write("Finish!");
            Console.ReadKey(true);
        }

        private static void Check(string[] paths)
        {
            if (paths is null)
            {
                throw new ArgumentNullException(nameof(paths));
            }

            using (HttpClient client = new HttpClient())
            {
                foreach (string path in paths)
                {
                    if (File.Exists(path))
                    {
                        string[] data = File.ReadAllLines(path);

                        Console.WriteLine(path);

                        for (int i = 0; i < data.Length; i++)
                        {
                            System.Threading.Thread.Sleep(200);

                            using (HttpResponseMessage response = client.GetAsync(data[i]).GetAwaiter().GetResult())
                            {
                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine(data[i] + " " + response.StatusCode);
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(data[i] + " " + response.StatusCode);
                                    Console.ResetColor();
#if DEBUG
                                    continue;
#endif
                                    ConsoleKeyInfo key = default;

                                    while (key == default || (key.Key != ConsoleKey.Y && key.Key != ConsoleKey.N))
                                    {
                                        Console.WriteLine("Continue? Y/N");

                                        key = Console.ReadKey(true);
                                    }

                                    if (key.Key == ConsoleKey.Y)
                                    {
                                        continue;
                                    }
                                    else if (key.Key == ConsoleKey.N)
                                    {
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        #endregion Methods
    }
}