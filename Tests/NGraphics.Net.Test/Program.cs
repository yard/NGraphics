using System;
using System.IO;
using NGraphics.Test;
using System.Linq;
using System.Threading;

namespace NGraphics.Net.Test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var sdir = System.IO.Path.GetDirectoryName (Environment.GetCommandLineArgs () [0]);
			while (Directory.GetFiles (sdir, "NGraphics.sln").Length == 0)
				sdir = System.IO.Path.GetDirectoryName (sdir);
			PlatformTest.ResultsDirectory = System.IO.Path.Combine (sdir, "TestResults");
			PlatformTest.Platform = Platforms.Current;
			Environment.CurrentDirectory = PlatformTest.ResultsDirectory;

			var tat = typeof(NUnit.Framework.TestAttribute);
      var tfat = typeof(NUnit.Framework.TestFixtureAttribute);
      var testSetupAttr = typeof(NUnit.Framework.SetUpAttribute);

			var types = typeof (DrawingTest).Assembly.GetTypes ();
			var testFixtures = types.Where (t => t.GetCustomAttributes (tfat, false).Length > 0);

			foreach (var testFixture in testFixtures) {
				var testFixtureInstance = Activator.CreateInstance (testFixture);
        var tests = testFixture.GetMethods().Where(m => m.GetCustomAttributes(tat, true).Length > 0);
        var testSetup = testFixture.GetMethods().Where(m => m.GetCustomAttributes(testSetupAttr, true).Length > 0).ToList();

				foreach (var test in tests) {

          if (testSetup.Any())
          {
            testSetup.First().Invoke(testFixtureInstance, null);
          }

          //if (test.Name.Equals("Smile"))
          //{
            Console.WriteLine("Running {0}...", test);

            test.Invoke(testFixtureInstance, null);
          //}
				
				}
			}

			Console.WriteLine ("Done...");
      Thread.Sleep(TimeSpan.FromSeconds(1));
		}
	}
}
