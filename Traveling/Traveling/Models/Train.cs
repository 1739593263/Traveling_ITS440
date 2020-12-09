using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Traveling.Models
{
    class Train : INotifyPropertyChanged
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

        string _source;
        [JsonProperty("source")]
        public string source
        {
            get => _source;
            set
            {
                if (_source == value) return;

                _source = value;

                HandlePropertyChanged();
            }
        }

        string _sourceTime;
        [JsonProperty("source_time")]
        public string sourceTime
        {
            get => _sourceTime;

            set
            {
                if (_sourceTime == value) return;

                _sourceTime = value;

                HandlePropertyChanged();
            }
        }

        string _destination;
        [JsonProperty("destination")]
        public string destination
        {
            get => _destination;

            set
            {
                if (_destination == value) return;

                _destination = value;

                HandlePropertyChanged();
            }
        }

        string _destinationTime;
        [JsonProperty("destination_time")]
        public string destinationTime
        {
            get => _destinationTime;

            set
            {
                if (_destinationTime == value) return;

                _destinationTime = value;

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

        string _company;
        [JsonProperty("company")]
        public string company
        {
            get => _company;

            set
            {
                if (_company == value) return;

                _company = value;

                HandlePropertyChanged();
            }
        }

        string _line;
        [JsonProperty("line")]
        public string line
        {
            get => _line;

            set
            {
                if (_line == value) return;

                _line = value;

                HandlePropertyChanged();
            }
        }

        int _isAvailable;
        [JsonProperty("isAvailable")]
        public int isAvailable
        {
            get => _isAvailable;

            set
            {
                if (_isAvailable == value) return;

                _isAvailable = value;

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

        public event PropertyChangedEventHandler PropertyChanged;

        private void HandlePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}
