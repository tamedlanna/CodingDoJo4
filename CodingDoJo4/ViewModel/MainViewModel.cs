using CodingDoJo4.Connection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CodingDoJo4.ViewModel
{
   
    public class MainViewModel : ViewModelBase
    {
        private Client clientcom;
        private bool isConnected = false;

       

        public MainViewModel()
        {
            string Message = "";
            ObservableCollection<string> ReceivedMessages = new ObservableCollection<string>();
            RelayCommand ConnectBtnClicked = new RelayCommand(() =>
             {
                 isConnected = true;
                 clientcom = new Client("127.0.0.1", 10100, new Action<string>(NewMessageReceived), ClientDisconnected);
             },
                       () =>
                       {
                           return !isConnected;
                       });
            RelayCommand SendBtnClickCmd = new RelayCommand(
                            () =>
                            {
                                clientcom.Send(ChatName + ": " + Message);
                    //write own message to GUI
                    ReceivedMessages.Add("YOU: " + Message);
                            }, () => { return (isConnected && Message.Length >= 1); });
        }

        private void NewMessageReceived(string message)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                ReceivedMessages.Add(message);
            });
        }
    

        private void ClientDisconnected()
        {
            isConnected = false;
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
       