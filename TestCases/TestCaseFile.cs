using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Tester.TestCases
{

    /// <summary>
    /// Stores input/output dataset for a file-based test case.
    /// </summary>
    class TestCaseFile : TestCase
    {
        public string OutputData { get; private set; }

        /// <summary>
        /// Creates a test dataset to be used with File IO.
        /// </summary>
        /// <param name="inputFilePath">Absolute path to test case input data file.</param>
        /// <param name="outputFilePath">Absolute path to test case expected output data file.</param>
        /// <param name="name">Name of the test case. Display only.</param>
        public TestCaseFile(string inputFilePath, string outputFilePath, string name) : base(inputFilePath, outputFilePath, name)
        {
        }

        public void LoadFiles(string expectedInputPath)
        {
            if (File.Exists(expectedInputPath))
            {
                File.Delete(expectedInputPath);
            }

            File.Copy(base.InputFilePath, expectedInputPath);
            OutputData = File.ReadAllText(base.OutputFilePath);
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
