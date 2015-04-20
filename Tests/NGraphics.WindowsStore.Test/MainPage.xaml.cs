using System;
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
using NUnit.Framework;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NGraphics.WindowsStore.Test
{
  /// <summary>
  ///   An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page
  {
    public MainPage()
    {
      InitializeComponent();
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
      await RunUnitTests();
    }

    private async Task RunUnitTests()
    {
      var tat = typeof (TestAttribute);
      var tfat = typeof (TestFixtureAttribute);

      var types = typeof (DrawingTest).GetTypeInfo().Assembly.ExportedTypes;
      var tfts = types.Where(t => t.GetTypeInfo().GetCustomAttributes(tfat, false).Any());

      var ngd = "";
      PlatformTest.ResultsDirectory = Path.Combine(ngd, "TestResults");
      PlatformTest.Platform = Platforms.Current;

      PlatformTest.OpenStream = path => new FileMemoryStream {Path = path};
      PlatformTest.CloseStream = async (stream, name) =>
      {
        //await UploadToLocalhost(name, stream, client);

        using (stream)
        {
          using (var memStream =
            stream.AsRandomAccessStream())
          {
            var bitMapImage = new BitmapImage();
            memStream.Seek(0);
            bitMapImage.SetSource(memStream);

            ImagesStackPanel.Children.Add(new Image {Source = bitMapImage});
          }
        }
      };

      foreach (var t in tfts)
      {
        var test = Activator.CreateInstance(t);
        var ms = t.GetRuntimeMethods().Where(m => m.GetCustomAttributes(tat, true).Any());
        foreach (var m in ms)
        {
          try
          {
            if (m.Name.Contains("PathData"))
            {
              var r = m.Invoke(test, null);
              var ta = r as Task;
              if (ta != null)
                await ta;
            }
          }
          catch (Exception ex)
          {
            Debug.WriteLine(ex);
          }
        }
      }
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