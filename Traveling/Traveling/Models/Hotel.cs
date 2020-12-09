using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Traveling.Models
{
    class Hotel : INotifyPropertyChanged
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
        public string Name {
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
        public string District {
            get => _district;
            set
            {
                if (_district == value) return;

                _district = value;

                HandlePropertyChanged();
            }
        }

        int _isClean;
        [JsonProperty("is_clean")]
        public int isClean
        {
            get => _isClean;
            set
            {
                if (_isClean == value) return;

                _isClean = value;

                HandlePropertyChanged();
            }
        }

        string _vacantDate;
        [JsonProperty("vacant_date")]
        public string vacantDate
        {
            get => _vacantDate;
            set
            {
                if (_vacantDate == value) return;

                _vacantDate = value;

                HandlePropertyChanged();
            }
        }

        string _description;
        [JsonProperty("description")]
        public string description {
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

        int _doubleroom;
        [JsonProperty("Double_room")]
        public int doubleroom
        {
            get => _doubleroom;
            set
            {
                if (_doubleroom == value) return;

                _doubleroom = value;

                HandlePropertyChanged();
            }
        }

        double _price1;
        [JsonProperty("price1")]
        public double price1
        {
            get => _price1;
            set
            {
                if (_price1 == value) return;

                _price1 = value;

                HandlePropertyChanged();
            }
        }

        int _Quaroom;
        [JsonProperty("Quadruple_room")]
        public int Quaroom
        {
            get => _Quaroom;
            set
            {
                if (_Quaroom == value) return;

                _Quaroom = value;

                HandlePropertyChanged();
            }
        }

        double _price2;
        [JsonProperty("price2")]
        public double price2
        {
            get => _price2;
            set
            {
                if (_price2 == value) return;

                _price2 = value;

                HandlePropertyChanged();
            }
        }

        int _suit;
        [JsonProperty("Suit")]
        public int suit
        {
            get => _suit;
            set
            {
                if (_suit == value) return;

                _suit = value;

                HandlePropertyChanged();
            }
        }

        double _price3;
        [JsonProperty("price3")]
        public double price3
        {
            get => _price3;
            set
            {
                if (_price3 == value) return;

                _price3 = value;

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
