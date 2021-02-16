using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Tester
{
    /// <summary>
    /// Template for all Test Running classes.
    /// </summary>
    /// <remarks>used fo creating different Test Runner classes with different input/output methods or specific run cases.</remarks>
    abstract class TestRunner
    {
        public readonly string ExecPath;

        /// <summary>
        /// Constructs a Test Runner object for a given program.
        /// </summary>
        /// <param name="path">Absolute path to program .exe file</param>
        public TestRunner(string path)
        {
            ExecPath = path;
        }
    }
}
