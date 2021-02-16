using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using Tester.TestCases;

namespace Tester.TestRunners
{
    /// <summary>
    /// Test Runned for programs that use files for I/O.
    /// </summary>
    class TestRunnerFile : TestRunner
    {
        public List<TestCaseFile> TestCases = new List<TestCaseFile>();
        private string InputFileName;
        private string OutputFileName;
        private string TemporaryDirectory;

        /// <summary>
        /// Constructs a Test Runner object for a given program.
        /// </summary>
        /// <param name="path">Absolute path to program .exe file</param>
        /// <param name="inputFile">Input file basename</param>
        /// <param name="outputFile">Output file basename</param>
        /// <seealso cref="TestRunner"/>
        public TestRunnerFile(string path, string inputFile, string outputFile) : base(path)
        {
            InputFileName = inputFile;
            OutputFileName = outputFile;

            TemporaryDirectory = Path.GetTempPath() + "Tester/";
            if (!Directory.Exists(TemporaryDirectory))
            {
                Directory.CreateDirectory(TemporaryDirectory);
            }
        }

        /// <summary>
        /// Runs a test on the program.
        /// </summary>
        /// <param name="testCase">Test case to use</param>
        /// <returns>Success of the test</returns>
        public TestResult RunTest(TestCaseFile testCase)
        {
            testCase.LoadFiles(TemporaryDirectory + InputFileName);

            using (var app = new Process())
            {
                app.StartInfo.FileName = base.ExecPath;
                app.StartInfo.WorkingDirectory = TemporaryDirectory;

                app.StartInfo.UseShellExecute = false;
                app.Start();

                var processExited = app.WaitForExit(5000);

                if (processExited == false)
                {
                    testCase.Results = TestResult.TimedOut;
                    return testCase.Results;
                }
                else if (app.ExitCode != 0)
                {
                    testCase.Results = TestResult.BadExitCode;
                    return testCase.Results;
                }

                string appOutput = File.ReadAllText(TemporaryDirectory + OutputFileName);

                bool correct = testCase.Answer(appOutput);
                if (correct)
                {
                    testCase.Results = TestResult.Correct;
                }
                else
                {
                    testCase.Results = TestResult.Incorrect;
                }

                return testCase.Results;
            }
        }

        /// <summary>
        /// Adds a Test Case to the program.
        /// </summary>
        /// <param name="testCase">TestCase to add to the list.</param>
        /// <remarks>No need to preload test files, this will be done later by RunTest method.</remarks>
        public void AddTest(TestCaseFile testCase)
        {
            TestCases.Add(testCase);
        }
    }
}
