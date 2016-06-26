using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using WebDriverRunner;
using WebDriverRunner.internals;

namespace E34TestAdapter
{
    [DefaultExecutorUri(E34TestExecutor.ExecutorUriString)]
    [FileExtension(".dll")]
    // [FileExtension(".exe")]
    public class E34TestDiscoverer : ITestDiscoverer
    {
        public static IMessageLogger LOGGER;

        public void DiscoverTests(IEnumerable<string> sources
            , IDiscoveryContext discoveryContext
            , Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging.IMessageLogger logger
            , ITestCaseDiscoverySink discoverySink)
        {
            LOGGER = logger;
            GetTests(sources, discoverySink, logger);
        }

        public static IEnumerable<TestCase> GetTests(IEnumerable<string> sourceFiles,
            ITestCaseDiscoverySink discoverySink, IMessageLogger logger)
        {
            var tests = new List<TestCase>();

            if (logger != null)
            {
                foreach (var sourceFile in sourceFiles)
                {
                    logger.SendMessage(TestMessageLevel.Informational, "Working on dll: " + sourceFile);
                }
            }

            foreach (var sourceFile in sourceFiles)
            {
                try
                {
                    Configuration config = new Configuration();
                    config.Dlls.Add(sourceFile);
                    Runner runner = new Runner(config);
                    runner.LoadTests();
                    List<TestMethodInstance> all = runner.GetAllTests();
                    logger.SendMessage(TestMessageLevel.Informational, "tests found :" + all.Count);


                    foreach (var test in all)
                    {

                       
                        /*var testCase = new TestCase(test, E34TestExecutor.ExecutorUri, sourceFile)
                        {
                            CodeFilePath = sourceFile,
                            
                            DisplayName = "test:" + test + "(chrome)",
                        };*/
                        var testCase = new TestCase(test.ToString(), E34TestExecutor.ExecutorUri, sourceFile)
                        {
                            CodeFilePath = test.Attr.File,
                            DisplayName = test.ToString(),
                            LineNumber = test.Attr.Line

                        };
                        if (discoverySink != null)
                        {
                            discoverySink.SendTestCase(testCase);
                        }
                        tests.Add(testCase);
                    }
                }
                catch (Exception e)
                {
                    logger.SendMessage(TestMessageLevel.Error, e.Message);
                }
                
            }
            return tests;
        }
    }
}