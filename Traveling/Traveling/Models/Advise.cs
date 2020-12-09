using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Traveling.Models
{
    class Advise : INotifyPropertyChanged
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

        int _caseId;
        [JsonProperty("case_id")]
        public int caseId {
            get => _caseId;
            set {
                if (_caseId == value) return;

                _caseId = value;

                HandlePropertyChanged();
            }
        }

        string _caseName;
        [JsonProperty("case_name")]
        public string caseName
        {
            get => _caseName;
            set
            {
                if (_caseName == value) return;

                _caseName = value;

                HandlePropertyChanged();
            }
        }

        string _userId;
        [JsonProperty("user_id")]
        public string userId {
            get => _userId;
            set {
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

        string _userTalk;
        [JsonProperty("user_talk")]
        public string userTalk
        {
            get => _userTalk;
            set
            {
                if (_userTalk == value) return;

                _userTalk = value;

                HandlePropertyChanged();
            }
        }

        string _serviceId;
        [JsonProperty("service_id")]
        public string serviceId
        {
            get => _serviceId;
            set
            {
                if (_serviceId == value) return;

                _serviceId = value;

                HandlePropertyChanged();
            }
        }

        string _serviceName;
        [JsonProperty("service_name")]
        public string serviceName
        {
            get => _serviceName;
            set
            {
                if (_serviceName == value) return;

                _serviceName = value;

                HandlePropertyChanged();
            }
        }

        string _serviceTalk;
        [JsonProperty("service_talk")]
        public string serviceTalk
        {
            get => _serviceTalk;
            set
            {
                if (_serviceTalk == value) return;

                _serviceTalk = value;

                HandlePropertyChanged();
            }
        }

        int _isReply;
        [JsonProperty("is_reply")]
        public int isReply
        {
            get => _isReply;
            set
            {
                if (_isReply == value) return;

                _isReply = value;

                HandlePropertyChanged();
            }
        }

        int _needReply;
        [JsonProperty("need_reply")]
        public int needReply
        {
            get => _needReply;
            set
            {
                if (_needReply == value) return;

                _needReply = value;

                HandlePropertyChanged();
            }
        }
        
        int _isSolved;
        [JsonProperty("is_solved")]
        public int isSolved {
            get => _isSolved;
            set {
                if (_isSolved == value) return;

                _isSolved = value;

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
