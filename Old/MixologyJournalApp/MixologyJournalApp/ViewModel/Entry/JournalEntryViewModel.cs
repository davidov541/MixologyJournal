using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using MixologyJournal.Persistence;
using MixologyJournal.SourceModel.Entry;
using MixologyJournal.ViewModel.App;

namespace MixologyJournal.ViewModel.Entry
{
    internal abstract class JournalEntryViewModel : IJournalEntryViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BaseMixologyApp _app;
        public BaseMixologyApp App
        {
            get
            {
                return _app;
            }
        }

        private bool _busy = false;
        public bool Busy
        {
            get
            {
                return _busy;
            }
            protected set
            {
                _busy = value;
                OnPropertyChanged(nameof(Busy));
            }
        }

        public virtual String Caption
        {
            get
            {
                return CreationDate.ToString();
            }
        }

        public DateTime CreationDate
        {
            get
            {
                if (Model == null)
                {
                    return new DateTime();
                }
                return Model.CreationDate;
            }
        }

        public virtual Symbol? Icon
        {
            get
            {
                return null;
            }
        }

        public int ID
        {
            get
            {
                return Model.ID;
            }
        }

        private List<String> _addedPictures = new List<string>();
        private ObservableCollection<String> _pictures = new ObservableCollection<string>();
        public ObservableCollection<String> Pictures
        {
            get
            {
                return _pictures;
            }
            private set
            {
                _pictures = value;
                OnPropertyChanged(nameof(Pictures));
            }
        }

        private String _title;
        public virtual String Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private SourceModel.Entry.JournalEntry _model;
        internal JournalEntry Model
        {
            get
            {
                return _model;
            }
        }

        private String _notes = String.Empty;
        public String Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                _notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }

        protected JournalEntryViewModel(SourceModel.Entry.JournalEntry model, BaseMixologyApp app)
        {
            _app = app;
            AttachToModel(model);
        }

        protected void AttachToModel(SourceModel.Entry.JournalEntry model)
        {
            _model = model;
            OnPropertyChanged(nameof(Model));
            if (_model != null)
            {
                Reset();
            }
        }

        protected void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task AddNewPictureAsync()
        {
            Busy = true;
            String identifier = await (_app.Persister as BaseAppPersister).SaveNewPictureAsync();
            if (identifier != null)
            {
                AddPicture(identifier);
            }
            Busy = false;
        }

        public async Task AddExistingPictureAsync()
        {
            Busy = true;
            String identifier = await (_app.Persister as BaseAppPersister).SaveExistingPictureAsync();
            if (identifier != null)
            {
                AddPicture(identifier);
            }
            Busy = false;
        }

        private void AddPicture(String pictureIdentifier)
        {
            _pictures.Add(pictureIdentifier);
            _addedPictures.Add(pictureIdentifier);
        }

        public virtual async Task SaveAsync()
        {
            Busy = true;
            if (_model != null)
            {
                _model.Title = Title;
                foreach (String picturePath in _addedPictures)
                {
                    _model.AddPicture(picturePath);
                }
                _model.Notes = Notes;
            }
            await _app.Persister.SaveAsync(_app.Journal);
            Busy = false;
        }

        public void Cancel()
        {
            Reset();
        }

        protected virtual void Reset()
        {
            _pictures = new ObservableCollection<String>();
            if (_model != null)
            {
                Title = _model.Title;
                _addedPictures = new List<String>();
                foreach (String pictureIdentifier in _model.Pictures)
                {
                    _pictures.Add(pictureIdentifier);
                }
                Notes = _model.Notes;
            }
            else
            {
                Title = String.Empty;
                Notes = String.Empty;
            }
        }

        public async Task DeleteAsync()
        {
            Busy = true;
            await _app.Journal.RemoveEntryAsync(this);
            foreach (String pic in Pictures)
            {
                await (_app.Persister as BaseAppPersister).DeleteFileAsync(pic);
            }
            Busy = false;
        }
    }
}
