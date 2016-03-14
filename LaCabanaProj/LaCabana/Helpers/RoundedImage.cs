using Android.Graphics;
using Android.Graphics.Drawables;
using System;
using Android.Content;

namespace LaCabana
{
	public class RoundedImage : Drawable
	{
		private Bitmap _bitmap;
		private Paint _paint;
		private Rect _rectF;
		private int _bitmapWidth;
		private int _bitmapHeight;
		private static string _username;
		private Context _context;

		public RoundedImage (Bitmap bitmap, Context context, string username = "")
		{
			_bitmap = bitmap;
			_rectF = new Rect ();
			_paint = new Paint ();
			_paint.AntiAlias = true;
			_paint.Dither = true;
			_paint.FilterBitmap = true;
			_username = username;
			_context = context;
			//var shader = new BitmapShader(bitmap, Shader.TileMode.Clamp, Shader.TileMode.Clamp);
			//_paint.SetShader(shader);

			_bitmapWidth = _bitmap.Width;
			_bitmapHeight = _bitmap.Height;
		}

		public override void Draw (Canvas canvas)
		{
			//canvas.DrawOval(_rectF, _paint);
			//canvas.DrawCircle(50,50,50,_paint);

			canvas.DrawBitmap (CropBitmap (), _rectF, _rectF, _paint);
		}

		public override void SetAlpha (int alpha)
		{
			if (_paint.Alpha != alpha) {
				_paint.Alpha = alpha;
				InvalidateSelf ();
			}
		}

		public override void SetColorFilter (ColorFilter cf)
		{
			_paint.SetColorFilter (cf);
		}

		public override int Opacity {
			get { return (int)Format.Translucent; }
		}

		protected override void OnBoundsChange (Rect bounds)
		{
			base.OnBoundsChange (bounds);
			_rectF.Set (bounds);
		}

		public override int IntrinsicWidth {
			get { return _bitmapWidth; }
		}

		public override int IntrinsicHeight {
			get { return _bitmapHeight; }
		}

		public override void SetFilterBitmap (bool filter)
		{
			_paint.FilterBitmap = filter;
			InvalidateSelf ();
		}

		public override void SetDither (bool dither)
		{
			_paint.Dither = dither;
			InvalidateSelf ();
		}

		public Bitmap Bitmap {
			get { return _bitmap; }
		}

		public Bitmap CropBitmap ()
		{
			Bitmap secondBitmap;
			var radius = _bitmapWidth;

			if (_bitmapHeight != radius || _bitmapWidth != radius) {
				secondBitmap = Bitmap.CreateScaledBitmap (_bitmap, radius, radius, false);
			} else {
				secondBitmap = _bitmap;
			}

			Bitmap output;
			try {
				output = Bitmap.CreateBitmap (secondBitmap.Width, secondBitmap.Height, Bitmap.Config.Argb8888);
			} catch (Exception e) {
				var placeHolderOptions = new BitmapFactory.Options { InSampleSize = 4, InDither = false, InPurgeable = true };
				var placeHolder = BitmapFactory.DecodeResource (_context.Resources, Resource.Drawable.avatarplaceholder, placeHolderOptions);
				output = Bitmap.CreateBitmap (secondBitmap.Width, secondBitmap.Height, Bitmap.Config.Argb8888);

			}

			var canvas = new Canvas (output);
			var paint = new Paint ();
			var rect = new Rect (0, 0, _bitmapWidth, _bitmapHeight);
			var rectF = new RectF (rect);

			paint.AntiAlias = true;
			paint.Dither = true;
			paint.FilterBitmap = true;
			canvas.DrawARGB (0, 0, 0, 0);
			canvas.DrawOval (rectF, paint);
			paint.SetXfermode (new PorterDuffXfermode (PorterDuff.Mode.SrcIn));
			canvas.DrawBitmap (secondBitmap, rect, rect, paint);


			return output;
		}
	}
}