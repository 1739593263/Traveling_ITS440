using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Traveling.Models
{
    class HotelTrans : INotifyPropertyChanged
    {
        string _id;
        [JsonProperty("id")]
        public string Id
        {
            get => _id;
            set
            {
                if (_id == value) return;

                _id = value;

                HandlePropertyChanged();
            }
        }

        string _name;
        [JsonProperty("Name")]
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;

                _name = value;

                HandlePropertyChanged();
            }
        }

        string _district;
        [JsonProperty("District")]
        public string District
        {
            get => _district;
            set
            {
                if (_district == value) return;

                _district = value;

                HandlePropertyChanged();
            }
        }

        string _date;
        [JsonProperty("date")]
        public string date
        {
            get => _date;
            set
            {
                if (_date == value) return;

                _date = value;

                HandlePropertyChanged();
            }
        }

        int _days;
        [JsonProperty("days")]
        public int days
        {
            get => _days;
            set
            {
                if (_days == value) return;

                _days = value;

                HandlePropertyChanged();
            }
        }

        string _description;
        [JsonProperty("description")]
        public string description
        {
            get => _description;
            set
            {
                if (_description == value) return;

                _description = value;

                HandlePropertyChanged();
            }
        }

        string _picture;
        [JsonProperty("picture")]
        public string picture
        {
            get => _picture;
            set
            {
                if (_picture == value) return;

                _picture = value;

                HandlePropertyChanged();
            }
        }

        string _roomType;
        [JsonProperty("room_type")]
        public string roomType
        {
            get => _roomType;
            set
            {
                if (_roomType == value) return;

                _roomType = value;

                HandlePropertyChanged();
            }
        }

        double _price;
        [JsonProperty("price")]
        public double price
        {
            get => _price;
            set
            {
                if (_price == value) return;

                _price = value;

                HandlePropertyChanged();
            }
        }

        string _userId;
        [JsonProperty("user_id")]
        public string userId
        {
            get => _userId;
            set
            {
                if (_userId == value) return;

                _userId = value;

                HandlePropertyChanged();
            }
        }

        string _userName;
        [JsonProperty("user_name")]
        public string userName
        {
            get => _userName;
            set
            {
                if (_userName == value) return;

                _userName = value;

                HandlePropertyChanged();
            }
        }

        int _isPaid;
        [JsonProperty("is_paid")]
        public int isPaid
        {
            get => _isPaid;
            set
            {
                if (_isPaid == value) return;

                _isPaid = value;

                HandlePropertyChanged();
            }
        }

        string _city;
        [JsonProperty("city")]
        public string city
        {
            get => _city;
            set
            {
                if (_city == value) return;

                _city = value;

                HandlePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void HandlePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }
    
    }
}
