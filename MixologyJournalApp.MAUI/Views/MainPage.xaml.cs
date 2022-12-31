using MixologyJournalApp.MAUI.Data;
using MixologyJournalApp.MAUI.Model;
using System.Collections.ObjectModel;

namespace MixologyJournalApp.MAUI.Views
{
    public partial class MainPage : ContentPage
    {
        private LocalDatabase _database = null;
        public ObservableCollection<Unit> Units { get; set; } = new();
        public ObservableCollection<Unit> TestUnits { 
            get {
                return new ObservableCollection<Unit>
                {
                    new Unit
                    {
                        Name = "Test Unit"
                    },
                    new Unit
                    {
                        Name = "Test Unit 2"
                    }
                };
            }
        }

        public MainPage()
        {
            InitializeComponent();
            this._database = new LocalDatabase();
            BindingContext = this;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            List<Unit> items = await this._database.GetUnitsAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                this.Units.Clear();
                foreach (Unit item in items)
                {
                    this.Units.Add(item);
                }
            });
        }
    }
}