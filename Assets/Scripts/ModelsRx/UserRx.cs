using System;
using System.ComponentModel;
using UniRx;


namespace ModelsRx
{
    public class UserRx : INotifyPropertyChanged
    {
        private string _userName = "";

        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName == value) return;
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private bool _isGuest = true;

        public bool IsGuest
        {
            get => _isGuest;
            set
            {
                if (_isGuest == value) return;
                _isGuest = value;
                OnPropertyChanged(nameof(IsGuest));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IObservable<EventPattern<PropertyChangedEventArgs>> Observable => UniRx.Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
            h => h.Invoke,
            h => PropertyChanged += h,
            h => PropertyChanged -= h
        );
    }
}