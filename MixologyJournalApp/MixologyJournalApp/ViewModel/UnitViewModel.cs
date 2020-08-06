using MixologyJournalApp.Model;
using System;
using System.ComponentModel;
using System.Linq;

namespace MixologyJournalApp.ViewModel
{
    internal class UnitViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly Unit _model;

        public String Name
        {
            get
            {
                return _model.Name;
            }
        }

        public String Plural
        {
            get
            {
                return _model.Plural;
            }
        }

        private readonly Char[] Vowels = new Char[] { 'a', 'e', 'i', 'o', 'u' };
        public String SingularArticle
        {
            get
            {
                if (Vowels.Contains(_model.Name.ToLower()[0]))
                {
                    return "An";
                }
                else
                {
                    return "A";
                }
            }
        }

        public String Id
        {
            get
            {
                return _model.Id;
            }
        }

        internal Unit Model
        {
            get
            {
                return _model;
            }
        }

        public UnitViewModel(Unit model)
        {
            _model = model;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return !(obj is UnitViewModel other) ? false : other.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
