using System;
using Xamarin.Forms;

namespace MixologyJournalApp.View
{
    public class MasterMenuItem<T>: IMasterMenuItem where T: ContentPage
    {
        public string Title 
        { 
            get; 
            private set; 
        }

        public Type TargetType 
        { 
            get
            {
                return typeof(T);
            }
        }

        public MasterMenuItem(String title)
        {
            Title = title;
        }
    }
}