using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MixologyJournalApp.View.Controls
{
    public class DeleteButton : ImageButton
    {
        public DeleteButton()
        {
            Source = ImageSource.FromFile("ic_trash_48dp.png");
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            switch(propertyName)
            {
                case nameof(Command):
                    Command.CanExecuteChanged += Command_CanExecuteChanged;
                    UpdateBackground();
                    break;
            }
        }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            UpdateBackground();
        }

        private void UpdateBackground()
        {
            if (Command != null && Command.CanExecute(BindingContext))
            {
                BackgroundColor = Color.Red;
            }
            else
            {
                BackgroundColor = Color.Gray;
            }
        }
    }
}