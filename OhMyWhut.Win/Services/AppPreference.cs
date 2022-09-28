using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;

namespace OhMyWhut.Win.Services
{
    public class AppPreference : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSetUserInfo { get => UserName != string.Empty && Password != string.Empty; }

        public bool IsSetMeterInfo { get => MeterId != string.Empty && FactoryCode != string.Empty; }

        private DateTimeOffset _termStartDay = DateTimeOffset.Now;

        public DateTimeOffset TermStartDay
        {
            get => _termStartDay;
            set => Save(ref _termStartDay, value);
        }

        private string _username = string.Empty;

        public string UserName
        {
            get => _username;
            set => Save(ref _username, value);
        }

        private string _password = string.Empty;

        public string Password
        {
            get => _password;
            set => Save(ref _password, value);
        }

        private string _realName = string.Empty;

        public string RealName
        {
            get => _realName;
            set => Save(ref _realName, value);
        }

        private string _roomId = string.Empty;

        public string RoomId
        {
            get => _roomId;
            set => Save(ref _roomId, value);
        }

        private string _meterId = string.Empty;

        public string MeterId
        {
            get => _meterId;
            set => Save(ref _meterId, value);
        }

        private string _factoryCode = string.Empty;

        public string FactoryCode
        {
            get => _factoryCode;
            set => Save(ref _factoryCode, value);
        }

        private string _dormitory = string.Empty;

        public string Dormitory
        {
            get => _dormitory;
            set => Save(ref _dormitory, value);
        }

        private TimeSpan _querySpanCourses = TimeSpan.FromDays(7);

        public TimeSpan QuerySpanCourses
        {
            get => _querySpanCourses;
            set => Save(ref _querySpanCourses, value);
        }

        private TimeSpan _querySpanElectricFee = TimeSpan.FromHours(6);

        public TimeSpan QuerySpanElectricFee
        {
            get => _querySpanCourses;
            set => Save(ref _querySpanElectricFee, value);
        }

        private TimeSpan _querySpanBooks = TimeSpan.FromDays(1);

        public TimeSpan QuerySpanBooks
        {
            get => _querySpanBooks;
            set => Save(ref _querySpanElectricFee, value);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private bool Save<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            ApplicationData.Current.LocalSettings.Values[propertyName] = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public void Load()
        {
            foreach (var propertyInfo in GetType().GetProperties())
            {
                var value = ApplicationData.Current.LocalSettings.Values[propertyInfo.Name];
                if (value == null)
                {
                    continue;
                }
                propertyInfo.SetValue(this, value);
            }
        }
    }
}
