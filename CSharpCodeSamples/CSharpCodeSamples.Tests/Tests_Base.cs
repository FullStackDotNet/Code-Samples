namespace CSharpCodeSamples.Tests
{
    using System;
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public abstract class Tests_Base
    {
        private TimeSpan _testStartTS;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Trace.Flush();
            Trace.Close();
        }

        [TestInitialize]
        public void Initialize()
        {
            _testStartTS = DateTime.Now.TimeOfDay;
            Debug.WriteLine(new string('*', 50));
            Debug.WriteLine("* Beginning Test *");
            Debug.WriteLine(new string('*', 50));
            Debug.WriteLine("-> Test Started At " + _testStartTS);
        }

        [TestCleanup]
        public void Cleanup()
        {
            TimeSpan testEndTS = DateTime.Now.TimeOfDay;
            TimeSpan testDurationTS = testEndTS - _testStartTS;
            Debug.WriteLine(new string('*', 50));
            Debug.WriteLine("* Ending Test *");
            Debug.WriteLine(new string('*', 50));
            Debug.WriteLine("-> Test Ended At " + testEndTS);
            Debug.WriteLine("-> Test Duration " + testDurationTS);

        }
    }
}
