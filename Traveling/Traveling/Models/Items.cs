using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Traveling.Models
{
    class Items : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string _ID;
        [JsonProperty("id")]
        public string Id {
            get => _ID;
            set {
                if (_ID == value) return;

                _ID = value;

                HandlePropertyChanged();
            }
        }

        string _name;
        [JsonProperty("name")]
        public string Name {
            get => _name;
            set {
                if (_name == value) return;

                _name = value;

                HandlePropertyChanged();
            }
        }

        string _description;
        [JsonProperty("description")]
        public string Description {
            get => _description;
            set {
                if (_description == value) return;

                _description = value;

                HandlePropertyChanged();
            }
        }

        private void HandlePropertyChanged([CallerMemberName]string propertyName = "")
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}
