using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MixologyJournal.Persistence;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MixologyJournal.View
{
    public sealed partial class ImageList : UserControl, INotifyPropertyChanged
    {
        public event EventHandler ImageChosen;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<String> Pictures
        {
            get { return (ObservableCollection<String>)GetValue(PicturesProperty); }
            set { SetValue(PicturesProperty, value); }
        }

        public static DependencyProperty PicturesProperty =
            DependencyProperty.Register("Pictures", typeof(ObservableCollection<String>), typeof(ImageList),
                new PropertyMetadata(new ObservableCollection<String>(), PictureChanged));

        private ObservableCollection<String> _picturePaths;
        public ObservableCollection<String> PicturePaths
        {
            get
            {
                return _picturePaths;
            }
        }

        public ImageViewer ImageViewer
        {
            get { return (ImageViewer)GetValue(ImageViewerProperty); }
            set { SetValue(ImageViewerProperty, value); }
        }

        public static DependencyProperty ImageViewerProperty =
            DependencyProperty.Register("ImageViewer", typeof(ImageViewer), typeof(ImageList),
                new PropertyMetadata(null));

        public BaseAppPersister Persister
        {
            get { return (BaseAppPersister)GetValue(PersisterProperty); }
            set { SetValue(PersisterProperty, value); }
        }

        public static DependencyProperty PersisterProperty =
            DependencyProperty.Register("Persister", typeof(BaseAppPersister), typeof(ImageList),
                new PropertyMetadata(null, PersisterChanged));

        private bool _busy;
        public bool Busy
        {
            get
            {
                return _busy;
            }
            set
            {
                _busy = value;
                OnPropertyChanged(nameof(Busy));
            }
        }

        public ImageList()
        {
            _picturePaths = new ObservableCollection<string>();
            InitializeComponent();
        }

        private static async void PersisterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageList list = d as ImageList;
            await list.UpdatePicturesAsync();
        }

        private static void PictureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageList list = d as ImageList;
            if (list.Persister != null)
            {
                list.UpdatePicturesAsync().ContinueWith(t =>
                {
                    if (e.OldValue != null)
                    {
                        (e.OldValue as ObservableCollection<String>).CollectionChanged -= list.Pictures_CollectionChanged;
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private async Task UpdatePicturesAsync()
        {
            if (Busy)
            {
                // This prevents race conditions where we are updating twice.
                return;
            }
            Busy = true;
            if (Persister != null)
            {
                _picturePaths.Clear();
                Pictures.CollectionChanged += Pictures_CollectionChanged;
                List<String> picturesCached = Pictures.ToList();
                foreach (String identifier in picturesCached)
                {
                    _picturePaths.Add(await Persister.LoadFileAsync(identifier));
                }
                OnPropertyChanged(nameof(PicturePaths));
            }
            Busy = false;
        }

        private async void Pictures_CollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
            await UpdatePicturesAsync();
        }

        private void Pictures_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            String image = ImagesList.SelectedItem as String;
            if (image != null)
            {
                ImageViewer.Picture = image;
                ImageViewer.Visibility = Visibility.Visible;
                ImageViewer.Closed += ImageViewer_Closed;
                ImageChosen?.Invoke(this, new EventArgs());
            }
        }

        private void ImageViewer_Closed(Object sender, EventArgs e)
        {
            ImageViewer.Visibility = Visibility.Collapsed;
            ImagesList.SelectedItem = null;
        }

        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
