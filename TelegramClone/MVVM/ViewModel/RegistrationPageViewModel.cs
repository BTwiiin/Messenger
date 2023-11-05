using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramClient.MVVM.Core;
using TelegramClient.MVVM.Model;
using TelegramClient.Net;
using TelegramClient.Services;

namespace TelegramClient.MVVM.ViewModel
{
    public class RegistrationPageViewModel : Core.BaseViewModel
    {
        private INavigationService _navigation { get; set; }
        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand NavigateHomeCommand { get; set; }   

        public RegistrationPageViewModel(INavigationService naviagation)
        {
            Navigation = naviagation;
            NavigateHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<MainWindowViewModel>(); }, o => true);
        }
    }
}
