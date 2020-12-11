using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Traveling.Models
{
    class Places: INotifyPropertyChanged
    {
        string _Id;
        [JsonProperty("id")]
        public string Id
        {
            get => _Id;

            set
            {
                if (_Id == value) return;

                _Id = value;

                HandlePropertyChanged();
            }
        }

        string _place;
        [JsonProperty("Place")]
        public string place {
            get => _place;

            set {
                if (_place == value) return;

                _place = value;

                HandlePropertyChanged();
            }
        }

        int _isflight;
        [JsonProperty("isFlight")]
        public int isflight {
            get => _isflight;

            set {
                if (_isflight == value) return;

                _isflight = value;

                HandlePropertyChanged();
            }
        }

        int _istrain;
        [JsonProperty("isTrain")]
        public int istrain {
            get => _istrain;

            set {
                if (_istrain == value) return;

                _istrain = value;

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
