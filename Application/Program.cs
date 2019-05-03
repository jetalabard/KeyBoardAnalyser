using System;

namespace Application
{
    class Program
    {

        private Program()
        {

        }

        static void Main(string[] args)
        {
            new Common.Main().Analyse();
            Console.ReadLine();
        }
    }
}
