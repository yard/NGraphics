using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using NGraphics.Test;
using NGraphics.WindowsStore.Test.Models;
using NUnit.Framework;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NGraphics.WindowsStore.Test
{
  /// <summary>
  ///   An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page
  {
    private ObservableCollection<SampleImage> _sampleImages;

    public MainPage()
    {
      InitializeComponent();
      ImageGridView.ItemsSource = SampleImages;
    }

    public ObservableCollection<SampleImage> SampleImages
    {
      get { return _sampleImages ?? (_sampleImages = new ObservableCollection<SampleImage>()); }
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
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
      var tat = typeof (TestAttribute);
      var tfat = typeof (TestFixtureAttribute);
      var testSetupAttr = typeof (SetUpAttribute);

      var types = typeof (DrawingTest).GetTypeInfo().Assembly.ExportedTypes;
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

          //if (test.Name.Equals("ErulisseuiinSpaceshipPack"))
          //{
          Debug.WriteLine("Running {0}...", test);

          test.Invoke(testFixtureInstance, null);
          //}
        }
      }

      Debug.WriteLine("Done...");
    }

    private static async Task UploadToLocalhost(string name, Stream stream)
    {
      var httpClient = new HttpClient();
      var url = string.Format("http://192.168.1.146/webapidatastreaming/filestreaming/uploadstream?fileName={0}.png",
        name);

      Debug.WriteLine("POSTING " + url);
      stream.Position = 0;

      var content = new StreamContent(stream);
      await httpClient.PostAsync(url, content);
    }

    private class FileMemoryStream : MemoryStream
    {
      public string Path;
    }
  }
}