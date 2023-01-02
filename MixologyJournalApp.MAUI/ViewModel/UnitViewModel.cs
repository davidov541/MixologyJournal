using MixologyJournalApp.MAUI.Model;
using System.ComponentModel;

namespace MixologyJournalApp.MAUI.ViewModel
{
    public class UnitViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public String Name
        {
            get
            {
                return Model.Name;
            }
        }

        public String Plural
        {
            get
            {
                return Model.Plural;
            }
        }

        public String Format
        {
            get
            {
                return Model.Format;
            }
        }

        private readonly Char[] Vowels = new Char[] { 'a', 'e', 'i', 'o', 'u' };
        public String SingularArticle
        {
            get
            {
                if (Vowels.Contains(Model.Name.ToLower()[0]))
                {
                    return "An";
                }
                else
                {
                    return "A";
                }
            }
        }

        private String Id
        {
            get
            {
                return Model.Id;
            }
        }

        internal Unit Model 
        {
            get;
        }

        internal UnitViewModel(Unit model)
        {
            Model = model;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return !(obj is UnitViewModel other) ? false : other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
