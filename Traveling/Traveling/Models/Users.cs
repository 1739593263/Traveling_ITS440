using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Traveling.Models
{
    class Users : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string _ID;
        [JsonProperty("id")]
        public string Id
        {
            get => _ID;
            set
            {
                if (_ID == value) return;

                _ID = value;

                HandlePropertyChanged();
            }
        }

        string _lastname;
        [JsonProperty("LastName")]
        public string Lastname
        {
            get => _lastname;
            set {
                if (_lastname == value) return;

                _lastname = value;

                HandlePropertyChanged();
            }
        }

        string _firstname;
        [JsonProperty("FirstName")]
        public string Firstname {
            get => _firstname;
            set {
                if (_firstname == value) return;

                _firstname = value;

                HandlePropertyChanged();
            }
        }

        string _gender;
        [JsonProperty("Gender")]
        public string Gender {
            get => _gender;
            set {
                if (_gender == value) return;

                _gender = value;

                HandlePropertyChanged();
            }
        }


        string _username;
        [JsonProperty("username")]
        public string username {
            get => _username;
            set {
                if (_username == value) return;

                _username = value;

                HandlePropertyChanged();
            }
        }

        string _password;
        [JsonProperty("password")]
        public string password {
            get => _password;
            set {
                if (_password == value) return;

                _password = value;

                HandlePropertyChanged();
            }
        }

        string _profession;
        [JsonProperty("profession")]
        public string profession
        {
            get => _profession;
            set {
                if (_profession == value) return;

                _profession = value;

                HandlePropertyChanged();
            }
        }

        string _birthday;
        [JsonProperty("birthday")]
        public string birthday {
            get => _birthday;
            set {
                if (_birthday == value) return;

                _birthday = value;

                HandlePropertyChanged();
            }
        }

        string _location;
        [JsonProperty("location")]
        public string location {
            get => _location;
            set {
                if (_location == value) return;

                _location = value;

                HandlePropertyChanged();
            }
        }

        string _mail;
        [JsonProperty("mail")]
        public string mail {
            get => _mail;
            set {
                if (_mail == value) return;

                _mail = value;

                HandlePropertyChanged();
            }
        }
        private void HandlePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}
