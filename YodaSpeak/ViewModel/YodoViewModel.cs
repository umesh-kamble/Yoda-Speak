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
            string transaletText = string.Empty;
            if (InternetConnection.IsConnectedToInternet())
            {
                 transaletText = YodoService.Instance.GetStringFromApi(OriginalText);
                if (!string.Equals(transaletText, "Error"))
                {
                    YodaSpeakDBHelper.Instance.addRecord(OriginalText, transaletText);
                    TransalatedText = transaletText;
                }
            }
            else
                TransalatedText = YodaSpeakDBHelper.Instance.getRecord(OriginalText);

            if (string.IsNullOrEmpty(TransalatedText) || string.Equals(transaletText , "Error"))
                    TransalatedText = "Somethig went wrong...";
            
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
