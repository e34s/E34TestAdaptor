using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using WebDriverRunner;
using WebDriverRunner.internals;

namespace E34TestAdapter
{
    [ExtensionUri(E34TestExecutor.ExecutorUriString)]
    public class E34TestExecutor : ITestExecutor
    {
        private bool canceled = false;

        public const string ExecutorUriString = "executor://e34executor/v2";
        public static readonly Uri ExecutorUri = new Uri(E34TestExecutor.ExecutorUriString);


        public void Cancel()
        {
            canceled = true;
        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            var tests = E34TestDiscoverer.GetTests(sources, null, null);
            RunTests(tests, runContext, frameworkHandle);
        }

        private void Log(String message)
        {
            if (E34TestDiscoverer.LOGGER != null)
            {
                E34TestDiscoverer.LOGGER.SendMessage(TestMessageLevel.Informational, "E34TestExecutor::RunTests");
            }
        }

        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            var olock = new Object();
            var cache = new Dictionary<string, string>();

            


            foreach (var test in tests)
            {
                Log("E34TestExecutor::RunTests=" + test.Source);
               
                var result = new TestResult(test);
                result.Outcome = TestOutcome.Failed;
                result.ErrorMessage = "Not implemented."+test.Source+" -- "+test.CodeFilePath;
                frameworkHandle.RecordResult(result);
            }

          


            /* */
        }
    }
}