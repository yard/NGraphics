using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using NGraphics.Test;
using NUnit.Framework;
using Environment = System.Environment;
using Path = System.IO.Path;

namespace NGraphics.Android.Test
{
  [Activity(Label = "NGraphics.Android.Test", MainLauncher = true, Icon = "@drawable/icon")]
  public class MainActivity : Activity
  {
    private int count = 1;
    private TextView _textView;

    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      CreateResultsDir();

      SetContentView(Resource.Layout.Gallery);
      // Set our view from the "main" layout resource
      _textView = FindViewById<TextView>(Resource.Id.testStatusTextView);

      //var mainLayout = FindViewById<LinearLayout>(Resource.Id.mainLayout);
     
      //mainLayout.SetBackgroundColor(Color.White);

      Task.Run(async () => { await RunUnitTests(); });
    }

    private void CreateResultsDir()
    {
      var ngd = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      PlatformTest.ResultsDirectory = Path.Combine(ngd, "TestResults");
      PlatformTest.Platform = Platforms.Current;

      var resultDir = Path.Combine(PlatformTest.ResultsDirectory, "Android");
      Directory.CreateDirectory(resultDir);
    }

    private async Task RunUnitTests()
    {
      var tat = typeof (TestAttribute);
      var tfat = typeof (TestFixtureAttribute);

      var types = typeof (DrawingTest).Assembly.GetTypes();
      var tfts = types.Where(t => t.GetCustomAttributes(tfat, false).Length > 0);
      //System.Environment.CurrentDirectory = PlatformTest.ResultsDirectory;

      RunOnUiThread(() => { _textView.Text = "Running Tests..."; });

      foreach (var t in tfts)
      {
        var test = Activator.CreateInstance(t);
        var ms = t.GetMethods().Where(m => m.GetCustomAttributes(tat, true).Length > 0);
        foreach (var m in ms)
        {
          //if (m.Name.Equals("ErulisseuiinSpaceshipPack"))
          //{
          try
          {
            RunOnUiThread(() => { _textView.Text = string.Format("Running {0} test...", m.Name); });
            var r = m.Invoke(test, null);
            var ta = r as Task;
            if (ta != null)
              await ta;
          }
          catch (Exception ex)
          {
            RunOnUiThread(() => { _textView.Text = "Exception occured...Check out the Output window for more details."; });

            Console.WriteLine(ex);
          }
          //}
        }
      }

      RunOnUiThread(() => { _textView.Text = "Done..."; });

      await
        Task.Delay(TimeSpan.FromSeconds(2))
          .ContinueWith(result => { RunOnUiThread(() =>
          {
            _textView.Visibility = ViewStates.Gone;
            var gallery = FindViewById<Gallery>(Resource.Id.gallery);

            gallery.Adapter = new ImageAdapter(this);

            gallery.ItemClick +=
              delegate(object sender, AdapterView.ItemClickEventArgs args)
              {
                Toast.MakeText(this, args.Position.ToString(), ToastLength.Short).Show();
              };

          }); });

    
    }
  }
}

