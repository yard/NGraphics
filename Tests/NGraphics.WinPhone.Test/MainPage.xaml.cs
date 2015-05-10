using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using NGraphics.Test;
using NGraphics.WinPhone.Test.Models;
using NUnit.Framework;

namespace NGraphics.WinPhone.Test
{
  public sealed partial class MainPage : Page
  {
    public MainPage()
    {
      InitializeComponent();

      ImageGridView.ItemsSource = SampleImages;
      NavigationCacheMode = NavigationCacheMode.Required;
    }

    private ObservableCollection<SampleImage> _sampleImages;

    public ObservableCollection<SampleImage> SampleImages
    {
      get { return _sampleImages ?? (_sampleImages = new ObservableCollection<SampleImage>()); }
    }

    /// <summary>
    ///   Invoked when this page is about to be displayed in a Frame.
    /// </summary>
    /// <param name="e">
    ///   Event data that describes how this page was reached.
    ///   This parameter is typically used to configure the page.
    /// </param>
    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
      PlatformSetup();
      await RunTests();
    }

    private void PlatformSetup()
    {
      PlatformTest.ResultsDirectory = "TestResults";
      PlatformTest.Platform = Platforms.Current;

      PlatformTest.OpenStream = path => new FileMemoryStream {Path = path};
      PlatformTest.CloseStream = async (stream, name) =>
      {
        //await UploadToLocalhost(name, stream, client);

        using (var memStream =
          stream.AsRandomAccessStream())
        {
          var bitmapImage = new BitmapImage();
          memStream.Seek(0);
          bitmapImage.SetSource(memStream);

          SampleImages.Add(new SampleImage
          {
            Name = name,
            ImageSource = bitmapImage
          });
        }
      };
    }

    private static async Task RunTests()
    {
      var tat = typeof(TestAttribute);
      var tfat = typeof(TestFixtureAttribute);
      var testSetupAttr = typeof(SetUpAttribute);

      var types = typeof(DrawingTest).GetTypeInfo().Assembly.ExportedTypes;
      var testFixtures = types.Where(t => t.GetTypeInfo().GetCustomAttributes(tfat, false).Any());

      foreach (var testFixture in testFixtures)
      {
        var testFixtureInstance = Activator.CreateInstance(testFixture);
        var tests = testFixture.GetRuntimeMethods().Where(m => m.GetCustomAttributes(tat, true).Any());
        var testSetup =
          testFixture.GetRuntimeMethods().Where(m => m.GetCustomAttributes(testSetupAttr, true).Any()).ToList();

        foreach (var test in tests)
        {
          if (testSetup.Any())
          {
            testSetup.First().Invoke(testFixtureInstance, null);
          }

          //if (test.Name.Contains("PathData"))
          //{
          Debug.WriteLine("Running {0}...", test);

          test.Invoke(testFixtureInstance, null);
          //}
        }
      }

      Debug.WriteLine("Done...");
    }

    private class FileMemoryStream : MemoryStream
    {
      public string Path;
    }
  }
}
