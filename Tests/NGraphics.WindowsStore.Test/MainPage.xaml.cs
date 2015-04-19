using NGraphics.Test;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Reflection;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;
using NGraphics.Parsers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NGraphics.WindowsStore.Test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async Task<BitmapImage> ByteArrayToBitmapImage(byte[] byteArray)
        {
          var bitmapImage = new BitmapImage();

          var stream = new InMemoryRandomAccessStream();
          await stream.WriteAsync(byteArray.AsBuffer());
          stream.Seek(0);

          bitmapImage.SetSource(stream);
          return bitmapImage;
        }

		private async void Page_Loaded (object sender, RoutedEventArgs e)
		{
      var path = "NGraphics.Test.Inputs.Smile.svg";
      var svgStream = typeof(PathTests).GetTypeInfo().Assembly.GetManifestResourceStream(path);

      var valuesParser = new ValuesParser();
      var reader = new NGraphics.Parsers.SvgReader(new StreamReader(svgStream), new StylesParser(valuesParser), valuesParser);
		  var graphics = reader.Graphic;

		  var canvas = new WinRTPlatform().CreateImageCanvas(graphics.Size);

      graphics.Draw(canvas);

      var image = (WICBitmapSourceImage)canvas.GetImage();


        int stride = image.Bitmap.Size.Width * 4;
		  using (var buffer = new SharpDX.DataStream(image.Bitmap.Size.Height*stride, true, true))
		  {
		    // Copy the content of the WIC to the buffer
		    image.Bitmap.CopyPixels(stride, buffer);

        var bitmapImage = new BitmapImage();

        var data = buffer.ReadRange<uint>(image.Bitmap.Size.Height * image.Bitmap.Size.Width);

		    var stream = buffer.AsRandomAccessStream();

		      Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
		      {
            
            stream.Seek(0);
             bitmapImage.SetSource(stream);
            sampleImage.Source = bitmapImage;
		      });
      
		      

		  }

      //var byteArray = new byte[100000];
      //image.Bitmap.CopyPixels(byteArray, 1);

      //image.Bitmap
      

		  //await RunUnitTests ();
		}
		
		async Task RunUnitTests ()
		{
			var tat = typeof (NUnit.Framework.TestAttribute);
			var tfat = typeof (NUnit.Framework.TestFixtureAttribute);

			var types = typeof (DrawingTest).GetTypeInfo ().Assembly.ExportedTypes;
			var tfts = types.Where (t => t.GetTypeInfo ().GetCustomAttributes (tfat, false).Any ());

			var ngd = "";
		  var pictures = KnownFolders.PicturesLibrary;
      PlatformTest.ResultsDirectory = Path.Combine(pictures.Path, "TestResults");
			PlatformTest.Platform = Platforms.Current;
      
      foreach (var t in tfts)
      {
        var test = Activator.CreateInstance(t);
        var ms = t.GetRuntimeMethods().Where(m => m.GetCustomAttributes(tat, true).ToList().Count > 0);
        foreach (var m in ms)
        {

          try
          {
            //						if (m.Name.Contains ("Path")) {
            m.Invoke(test, null);
            //					}
          }
          catch (Exception e)
          {
            Debug.WriteLine(e.ToString());
          }
        }
      }

		}
    }
}
