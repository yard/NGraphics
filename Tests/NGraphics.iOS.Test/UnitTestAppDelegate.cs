using System;
using System.Linq;
using System.Collections.Generic;

using Foundation;
using UIKit;
using NGraphics.Test;
using System.IO;
using System.Diagnostics;
using NGraphics.Custom;

namespace NGraphics.iOS.Test
{
	[Register ("UnitTestAppDelegate")]
	public partial class UnitTestAppDelegate : UIApplicationDelegate
	{
		UIWindow window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			var tat = typeof(NUnit.Framework.TestAttribute);
			var tfat = typeof(NUnit.Framework.TestFixtureAttribute);

			var types = typeof (DrawingTest).Assembly.GetTypes ();
			var tfts = types.Where (t => t.GetCustomAttributes (tfat, false).Length > 0);

			var documentsDirectory = Environment.GetFolderPath
				(Environment.SpecialFolder.Personal);

			var resultDir = Path.Combine (documentsDirectory, "iOS");

			if (Directory.Exists (resultDir)) {
				Directory.Delete (resultDir, true);
			}

			Directory.CreateDirectory (resultDir);

			PlatformTest.ResultsDirectory = documentsDirectory;
			PlatformTest.Platform = Platforms.Current;
			Environment.CurrentDirectory = PlatformTest.ResultsDirectory;

			foreach (var t in tfts) {
				var test = Activator.CreateInstance (t);
				var ms = t.GetMethods ().Where (m => m.GetCustomAttributes (tat, true).Length > 0);
				foreach (var m in ms) {

					try{
//						if (m.Name.Contains ("Path")) {
						m.Invoke (test, null);
							//					}
					}catch(Exception e){
						Debug.WriteLine (e.ToString());
					}
				}
			}


			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

