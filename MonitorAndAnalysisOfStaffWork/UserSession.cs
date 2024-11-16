using MonitorAndAnalysisOfStaffWork.Entities;
using System.ComponentModel;

namespace MonitorAndAnalysisOfStaffWork
{
    public class UserSession : INotifyPropertyChanged
    {
        private static UserEntity? _currentUser;
        private static readonly object _lock = new object();

        // Событие для уведомления об изменениях
        public static event PropertyChangedEventHandler? StaticPropertyChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        // Приватный конструктор
        private UserSession() { }

        // Метод для получения или установки текущего пользователя
        public static UserEntity? CurrentUser
        {
            get
            {
                lock (_lock)
                {
                    return _currentUser;
                }
            }
            set
            {
                lock (_lock)
                {
                    if (_currentUser != value)
                    {
                        _currentUser = value;
                        OnStaticPropertyChanged(nameof(CurrentUser));
                    }
                }
            }
        }

         //Метод для вызова события при изменении свойства
        private static void OnStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        // Метод для проверки, авторизован ли пользователь
        public static bool IsAuthenticated => _currentUser != null;
    }

}
