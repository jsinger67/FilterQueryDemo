using System;
using System.IO;
using FilterQueryDemo;

namespace FilterQueryDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please provide a file name as first parameter!");
                return;
            }

            string fileName = args[0];
            string input = File.ReadAllText(fileName);
            var actions = new FilterQueryDemoUserActions();

            try
            {
                FilterQueryDemoParser.Parse(input, fileName, actions);
                actions.EvaluateParsedQuery();
                Console.WriteLine("Success!");
                Console.WriteLine(actions.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
