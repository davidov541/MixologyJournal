using MixologyJournalApp.Model;
using System;
using System.ComponentModel;
using System.Linq;

namespace MixologyJournalApp.ViewModel
{
    internal class UnitViewModel: INotifyPropertyChanged
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

        public String Id
        {
            get
            {
                return Model.Id;
            }
        }

        internal Unit Model { get; }

        public UnitViewModel(Unit model)
        {
            Model = model;
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
