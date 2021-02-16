using System;
using Tester.TestRunners;
using Tester.TestCases;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            UI.Greeting();
            UI.DefineProgram();
            UI.DefineTests();
            UI.RunTests();
        }
    }
}
