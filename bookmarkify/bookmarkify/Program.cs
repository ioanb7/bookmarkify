using System;

namespace bookmarkify
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var mainManager = new MainManager();
            mainManager.Run();
        }
    }
}
