using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Tester.TestRunners;
using Tester.TestCases;

namespace Tester
{
    /// <summary>
    /// Type for storing program IO type.
    /// </summary>
    enum ProgramType
    {
        Stdio,
        File
    }

    /// <summary>
    /// Logic for all Console UI operations.
    /// </summary>
    static class UI
    {
        static ProgramType Type;
        static TestRunnerStdio runnerStdio;
        static TestRunnerFile runnerFile;

        /// <summary>
        /// Greets the user with a message.
        /// </summary>
        public static void Greeting()
        {
            Console.WriteLine("Hello, Tester!");
        }

        /// <summary>
        /// Prompts user for program exe path and IO type. Can ask for more, depending on the type.
        /// </summary>
        public static void DefineProgram()
        {
            string exePath = "";
            bool fileCheck = false;
            do
            {
                Console.Write("Full path to .exe: ");
                exePath = Console.ReadLine();
                fileCheck = File.Exists(exePath);
                if (!fileCheck)
                {
                    Console.WriteLine("No such file.");
                }
            } while (!fileCheck);

            string option = "";
            Console.WriteLine(" 1) STDIO");
            Console.WriteLine(" 2) File based IO");
            do
            {
                Console.Write("Program IO type: ");
                option = Console.ReadLine();
            } while (!(option.Equals("1") || option.Equals("2")));

            if (option.Equals("1"))
            {
                Type = ProgramType.Stdio;
                runnerStdio = new TestRunnerStdio(exePath);
            }
            else if (option.Equals("2"))
            {
                Type = ProgramType.File;
                Console.Write("Input  file basename: ");
                string inputBasename = Console.ReadLine();
                Console.Write("Output file basename: ");
                string outputBasename = Console.ReadLine();
                runnerFile = new TestRunnerFile(exePath, inputBasename, outputBasename);
            }
        }

        /// <summary>
        /// Promts user for test cases.
        /// </summary>
        public static void DefineTests()
        {
            Console.Clear();
            string testDir = "";
            bool dirCheck = false;
            do
            {
                Console.Write("Full path test dir: ");
                testDir = Console.ReadLine();
                dirCheck = Directory.Exists(testDir);
                if (!dirCheck)
                {
                    Console.WriteLine("No such directory.");
                }
            } while (!dirCheck);

            if (!testDir.EndsWith(@"\"))
            {
                testDir += @"\";
            }

            string[] testFiles = Directory.EnumerateFiles(testDir).ToArray();
            if (testFiles.Count() == 0)
            {
                Console.WriteLine("Directory is empty.");
                return;
            }

            for (int i = 0; i < testFiles.Length; i++)
            {
                testFiles[i] = Path.GetFileName(testFiles[i]);
            }

            string[] inputEndings = { ".in", ".input" };
            string[] outputEndings = { ".out", ".sol", ".rez", "rez.txt" };

            string inEnding = PredictEnding(inputEndings, testFiles);
            string outEnding = PredictEnding(outputEndings, testFiles);

            if (String.IsNullOrEmpty(inEnding))
            {
                Console.Write($"Input test file ending: *");
                inEnding = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Input file ending detected as {0}", inEnding);
            }

            if (String.IsNullOrEmpty(outEnding))
            {
                Console.Write($"Output test file ending: *");
                outEnding = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Output file ending detected as {0}", outEnding);
            }
            

            foreach(string filename in testFiles)
            {
                if (filename.EndsWith(inEnding))
                {
                    string inputFile = filename;
                    string outputFile = filename.Remove(filename.Length - inEnding.Length) + outEnding;
                    if (testFiles.Contains(outputFile))
                    {
                        AddTestCase(testDir + inputFile, testDir + outputFile, inputFile);
                        Console.WriteLine("Added test case {0}", inputFile);
                    }
                }
            }
        }

        /// <summary>
        /// Tries to match the endings used in test file array.
        /// </summary>
        /// <param name="endings">String array with wanted endings</param>
        /// <param name="testFiles">String array with test filenames</param>
        /// <returns>Matched ending string or empty, if no matches were found</returns>
        private static string PredictEnding(string[] endings, string[] testFiles)
        {
            string detected = "";

            foreach (string ending in endings)
            {
                if (testFiles[0].Contains(ending))
                {
                    detected = ending;
                    break;
                }
            }
            
            // Check second file for endings.
            if (String.IsNullOrEmpty(detected))
            {
                foreach (string ending in endings)
                {
                    if (testFiles[1].Contains(ending))
                    {
                        detected = ending;
                        break;
                    }
                }
            }

            return detected;
        }

        /// <summary>
        /// Adds a test case to the corresponding runner.
        /// </summary>
        /// <param name="inPath">Absolute path to test case input data file.</param>
        /// <param name="outPath">Absolute path to test case output data file.</param>
        /// <param name="name">Name of the test case. Display only.</param>
        private static void AddTestCase(string inPath, string outPath, string name)
        {
            if (Type == ProgramType.Stdio)
            {
                runnerStdio.AddTest(new TestCaseStdio(inPath, outPath, name));
            }
            else if (Type == ProgramType.File)
            {
                runnerFile.AddTest(new TestCaseFile(inPath, outPath, name));
            }
        }

        /// <summary>
        /// Runs all of the tests.
        /// </summary>
        public static void RunTests()
        {
            Console.WriteLine("Press Enter to run tests");
            Console.ReadLine();

            int score = 0;
            int ran = 0;
            int total = 0;

            if (Type == ProgramType.Stdio)
            {
                score = 0;
                ran = 0;
                total = runnerStdio.TestCases.Count;
                foreach (TestCaseStdio test in runnerStdio.TestCases)
                {
                    TestResult result = runnerStdio.RunTest(test);
                    ran++;
                    if (result == TestResult.Correct)
                    {
                        Console.WriteLine(" PASS  {0}/{1}  {2}", ran, total, test.Name);
                        score++;
                    }
                    else
                    {
                        Console.WriteLine(" FAIL  {0}/{1}  {2}  {3}", ran, total, test.Name, result);
                    }
                }
            }
            else if (Type == ProgramType.File)
            {
                score = 0;
                ran = 0;
                total = runnerFile.TestCases.Count;
                foreach (TestCaseFile test in runnerFile.TestCases)
                {
                    TestResult result = runnerFile.RunTest(test);
                    ran++;
                    if (result == TestResult.Correct)
                    {
                        Console.WriteLine(" PASS  {0}/{1}  {2}", ran, total, test.Name);
                        score++;
                    }
                    else
                    {
                        Console.WriteLine(" FAIL  {0}/{1}  {2}  {3}", ran, total, test.Name, result);
                    }
                }
            }

            Console.WriteLine("--------------------");
            Console.WriteLine("{0}/{1} Test cases passed", score, total);
            Console.ReadLine();
        }
    }
}
