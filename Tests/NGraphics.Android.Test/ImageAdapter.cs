using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace NGraphics.Android.Test
{
  public class ImageAdapter : BaseAdapter
  {
    private readonly Context context;
    private readonly List<string> _pngs;

    public ImageAdapter(Context c)
    {
      context = c;
      const string resultDir = "/data/data/NGraphics.Android.Test/files/TestResults/Android";
      _pngs = Directory.GetFiles(resultDir, "*.png").ToList();
    }

    public override int Count
    {
      get { return _pngs.Count; }
    }

    public override Object GetItem(int position)
    {
      return null;
    }

    public override long GetItemId(int position)
    {
      return 0;
    }

    // create a new ImageView for each item referenced by the Adapter
    public override View GetView(int position, View convertView, ViewGroup parent)
    {
      var imageView = new ImageView(context);

      var imagePath = _pngs[position];

      if (File.Exists(imagePath))
      {
        using (var imageFile = new Java.IO.File(imagePath))
        {
          var options = new BitmapFactory.Options {InSampleSize = 2};
          using (var bitmap = BitmapFactory.DecodeFile(imageFile.AbsolutePath, options))
          {
            imageView.SetImageBitmap(bitmap);

            imageView.LayoutParameters = new Gallery.LayoutParams(1000, 1000);
            imageView.SetScaleType(ImageView.ScaleType.FitXy);
          }
        }
      }

      return imageView;
    }

  
  }
}