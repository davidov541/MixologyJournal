﻿using MixologyJournalApp.Platform;
using Newtonsoft.Json;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace MixologyJournalApp.Droid.Platform
{
    internal class GoogleLoginMethod : ILoginMethod
    {
        private readonly IGoogleClientManager _googleService;

        public String Name
        {
            get
            {
                return "Google";
            }
        }

        public String Icon
        {
            get
            {
                return "ic_google";
            }
        }

        public String BackgroundColor
        {
            get
            {
                return "#F8F8F8";
            }
        }

        public String ForegroundColor
        {
            get
            {
                return "#000000";
            }
        }

        private Boolean _isLoggedIn = false;
        public Boolean IsLoggedIn
        {
            get
            {
                return _isLoggedIn;
            }
            private set
            {
                _isLoggedIn = value;
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new Command(Login);
            }
        }

        public GoogleLoginMethod()
        {
            _googleService = CrossGoogleClient.Current;
            IsLoggedIn = !String.IsNullOrEmpty(_googleService.ActiveToken);
        }

        private async void Login()
        {
            try
            {
                if (!string.IsNullOrEmpty(_googleService.ActiveToken))
                {
                    //Always require user authentication
                    _googleService.Logout();
                }

                _googleService.OnLogin += LoginProcessCompleted;

                await _googleService.LoginAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async void LoginProcessCompleted(object sender, GoogleClientResultEventArgs<GoogleUser> e)
        {
            switch (e.Status)
            {
                case GoogleActionStatus.Completed:
                    String googleUserString = JsonConvert.SerializeObject(e.Data);
                    Console.WriteLine($"Google Logged in succesfully: {googleUserString}");
                    IsLoggedIn = true;
                    break;
                case GoogleActionStatus.Canceled:
                    await App.Current.MainPage.DisplayAlert("Google Auth", "Canceled", "Ok");
                    break;
                case GoogleActionStatus.Error:
                    await App.Current.MainPage.DisplayAlert("Google Auth", "Error", "Ok");
                    break;
                case GoogleActionStatus.Unauthorized:
                    await App.Current.MainPage.DisplayAlert("Google Auth", "Unauthorized", "Ok");
                    break;
            }

            _googleService.OnLogin -= LoginProcessCompleted;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}