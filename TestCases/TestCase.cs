using System;
using System.Collections.Generic;
using System.Text;

namespace Tester
{
    public enum TestResult
    {
        NotTested,
        TimedOut,
        BadExitCode,
        Incorrect,
        Correct,
        EndOfLine
    }

    /// <summary>
    /// Class used for storing data of one test case and its results.
    /// </summary>
    abstract class TestCase
    {
        public string Name { get; }
        public string InputFilePath { get; }
        public string OutputFilePath { get; }
        public string ProgramOutput { get; set; }
        public TestResult Results { get; set; }

        /// <summary>
        /// Constructs a new TestCase object. Creates a test dataset from files.
        /// </summary>
        /// <param name="inputFilePath">Absolute path to test case input data file.</param>
        /// <param name="outputFilePath">Absolute path to test case expected output data file.</param>
        public TestCase(string inputFilePath, string outputFilePath, string name)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            Name = name;
            Results = TestResult.NotTested;
        }

        /// <summary>
        /// Adds program output and checks if it matches the provided one.
        /// </summary>
        /// <param name="programOutput">Program output</param>
        /// <returns>True, if the output matches the expected one.</returns>
        abstract public bool Answer(string programOutput);
    }
}
