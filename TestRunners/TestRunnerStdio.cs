using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Tester.TestCases;

namespace Tester.TestRunners
{
    /// <summary>
    /// Test Runner for programs that use STDIO.
    /// </summary>
    class TestRunnerStdio : TestRunner
    {
        public List<TestCaseStdio> TestCases = new List<TestCaseStdio>();

        /// <summary>
        /// Constructs a Test Runner object for a given program.
        /// </summary>
        /// <param name="path">Absolute path to program .exe file</param>
        /// <seealso cref="TestRunner"/>
        public TestRunnerStdio(string path) : base(path)
        {
        }

        /// <summary>
        /// Runs a test on the program.
        /// </summary>
        /// <param name="testCase">Test case to use</param>
        /// <returns>Success of the test</returns>
        public TestResult RunTest(TestCaseStdio testCase)
        {
            testCase.LoadFiles();

            using (var app = new Process())
            {
                app.StartInfo.FileName = base.ExecPath;
                app.StartInfo.RedirectStandardInput = true;
                app.StartInfo.RedirectStandardOutput = true;
                app.StartInfo.UseShellExecute = false;
                app.Start();

                var appStdin = app.StandardInput;
                appStdin.WriteLine(testCase.InputData);

                var appStdout = app.StandardOutput;
                string appOutput = appStdout.ReadToEnd();

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
        public void AddTest(TestCaseStdio testCase)
        {
            TestCases.Add(testCase);
        }
    }
}
