using System;
using System.Collections.Generic;
using System.Text;

namespace Tester.TestCases
{

    /// <summary>
    /// Stores STDIO input/output dataset for a test case.
    /// </summary>
    class TestCaseStdio : TestCase
    {
        public string InputData { get; private set; }
        public string OutputData { get; private set; }

        /// <summary>
        /// Creates a test dataset to be used with STDIO.
        /// </summary>
        /// <param name="inputFilePath">Absolute path to test case input data file.</param>
        /// <param name="outputFilePath">Absolute path to test case expected output data file.</param>
        /// /// <param name="name">Name of the test case. Display only.</param>
        public TestCaseStdio(string inputFilePath, string outputFilePath, string name) : base(inputFilePath, outputFilePath, name)
        {
        }

        /// <summary>
        /// Reads input and output files and stores the data in the object.
        /// </summary>
        public void LoadFiles()
        {
            InputData = System.IO.File.ReadAllText(base.InputFilePath);
            OutputData = System.IO.File.ReadAllText(base.OutputFilePath);
        }

        /// <summary>
        /// Adds program output and checks if it matches the provided one.
        /// </summary>
        /// <param name="programOutput">Program output</param>
        /// <returns>True, if the output matches the expected one.</returns>
        public override bool Answer(string programOutput)
        {
            base.ProgramOutput = programOutput;
            return OutputData.Trim().Equals(programOutput.Trim());
        }
    }
}
