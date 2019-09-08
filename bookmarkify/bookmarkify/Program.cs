using System;

namespace bookmarkify
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bookmarkify");
            Console.WriteLine("-----------");
            Console.WriteLine();

            var mainPathInput = @"H:\Projects\bookmarkify-data";
            var mainPathOutput = @"H:\Projects\bookmarkify-output";

            var mainManager = new MainManager();
            mainManager.Run(mainPathInput, mainPathOutput);
        }
    }
}
