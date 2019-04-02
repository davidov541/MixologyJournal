using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MixologyJournal.View
{
    public sealed partial class ImageViewer : UserControl
    {
        public event EventHandler Closed;

        public String Picture
        {
            get { return (String)GetValue(PictureProperty); }
            set { SetValue(PictureProperty, value); }
        }

        public static DependencyProperty PictureProperty =
            DependencyProperty.Register("Picture", typeof(String), typeof(ImageViewer),
                new PropertyMetadata(String.Empty));

        public ImageViewer()
        {
            InitializeComponent();
        }

        private void FullImage_LayoutUpdated(Object sender, Object e)
        {
            (FullImage.RenderTransform as CompositeTransform).CenterX = FullImage.ActualWidth / 2.0;
            (FullImage.RenderTransform as CompositeTransform).CenterY = FullImage.ActualHeight / 2.0;
            double imageWidth, imageHeight;
            if (FullImage.ActualWidth > FullImage.ActualHeight)
            {
                // Image is horizontal.
                if (ActualWidth > ActualHeight)
                {
                    // Screen/window is horizontal.
                    imageWidth = FullImage.ActualWidth;
                    imageHeight = FullImage.ActualHeight;
                    (FullImage.RenderTransform as CompositeTransform).Rotation = 0;
                }
                else
                {
                    // Screen/window is vertical.
                    imageHeight = FullImage.ActualWidth;
                    imageWidth = FullImage.ActualHeight;
                    (FullImage.RenderTransform as CompositeTransform).Rotation = 90;
                }
            }
            else
            {
                // Image is portrait.
                if (ActualWidth > ActualHeight)
                {
                    // Screen/window is horizontal.
                    imageHeight = FullImage.ActualWidth;
                    imageWidth = FullImage.ActualHeight;
                    (FullImage.RenderTransform as CompositeTransform).Rotation = 90;
                }
                else
                {
                    // Screen/window is vertical.
                    imageWidth = FullImage.ActualWidth;
                    imageHeight = FullImage.ActualHeight;
                    (FullImage.RenderTransform as CompositeTransform).Rotation = 0;
                }
            }
            double heightScale = imageHeight == 0 ? 1.0 : ActualHeight / imageHeight;
            double widthScale = imageWidth == 0 ? 1.0 : ActualWidth / imageWidth;
            double scale = Math.Min(heightScale, widthScale);
            (FullImage.RenderTransform as CompositeTransform).ScaleX = scale;
            (FullImage.RenderTransform as CompositeTransform).ScaleY = scale;
        }

        private void ClosePictureButton_Click(Object sender, RoutedEventArgs e)
        {
            Closed?.Invoke(this, new EventArgs());
        }
    }
}
