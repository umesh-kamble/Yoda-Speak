using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YodaSpeak.Database;
using YodaSpeak.Helper;
using YodaSpeak.Services;

namespace YodaSpeak.ViewModel
{
    public class YodoViewModel : INotifyPropertyChanged
    {
        #region Propertise & private variable
        private ICommand translateTextCommand;
        private string transalatedText;
        private string originalText;
        private bool _canExecute;
        public string TransalatedText
        {
            get { return transalatedText; }
            set
            {
                transalatedText = value;
                OnPropertyChanged();
            }
        }
        public string OriginalText
        {
            get
            {
                return originalText;
            }
            set
            {
                originalText = value;
                OnPropertyChanged();
                OnPropertyChanged("TranslateCommand");
            }
        }
        public bool TranslateCommand
        {
            get { return !string.IsNullOrWhiteSpace(originalText); }
        }
        public ICommand TranslateTextCommand
        {
            get
            {
                return translateTextCommand ?? (translateTextCommand = new RelayCommand(() => TranslateSentence(), _canExecute));
            }
        }
        #endregion
        #region Constructor
        public YodoViewModel()
        {
            _canExecute = true;
        }
        #endregion
        #region Methods
        private void TranslateSentence()
        {
            TransalatedText = string.Empty;
            if (InternetConnection.IsConnectedToInternet())
            {
                TransalatedText = YodoService.Instance.GetStringFromApi(OriginalText);
                YodaSpeakDBHelper.Instance.addRecord(OriginalText, TransalatedText);
            }
            else
                TransalatedText = YodaSpeakDBHelper.Instance.getRecord(OriginalText);

            if (string.IsNullOrEmpty(TransalatedText))
                    TransalatedText = "Record not found.";
            
            OriginalText = string.Empty;
        }
        #endregion
        #region InotifyPropertyChangedEvent
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string name = "") =>
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
    }
}
