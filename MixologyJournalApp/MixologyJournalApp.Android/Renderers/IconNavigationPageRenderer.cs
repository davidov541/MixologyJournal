using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using MixologyJournalApp.Droid.Renderers;
using MixologyJournalApp.View;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(RootPage), typeof(IconNavigationPageRenderer))]
namespace MixologyJournalApp.Droid.Renderers
{
    public class IconNavigationPageRenderer : MasterDetailPageRenderer
    {
        private Context _context;

        public IconNavigationPageRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);

            Page currentPage = ((NavigationPage)((MasterDetailPage)Application.Current.MainPage).Detail).CurrentPage;

            if (currentPage is CreateDrinkPage || currentPage is CreateRecipePage)
            {
                Bitmap srcBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.cancel_48);
                int srcWidth = srcBitmap.Width;
                int srcHeight = srcBitmap.Height;
                int dstWidth = (int)(srcWidth * 0.6f);
                int dstHeight = (int)(srcHeight * 0.6f);
                Bitmap dstBitmap = Bitmap.CreateScaledBitmap(srcBitmap, dstWidth, dstHeight, true);

                SetNavigationIcon(dstBitmap);
            }
        }
        private void SetNavigationIcon(Bitmap icon)
        {
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            AppCompatImageButton navIcon = toolbar.NavigationIcon.Callback as AppCompatImageButton;

            navIcon?.SetImageBitmap(icon);
        }
    }
}