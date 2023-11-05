using System;
using TelegramClient.MVVM.Core;

namespace TelegramClient.Services;

public interface INavigationService
{
    BaseViewModel CurrentView { get; }
    void NavigateTo<T>() where T : BaseViewModel;
}

public class NavigationService : ObservableObject, INavigationService
{
    private BaseViewModel _currentView;
    private Func<Type, BaseViewModel> _viewModelFactory;

    public BaseViewModel CurrentView
    {
        get => _currentView;
        private set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public NavigationService(Func<Type, BaseViewModel> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel
    {
        BaseViewModel viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
        CurrentView = viewModel;
    }
}
