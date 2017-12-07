using CD4_Server.Communication;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CD4_Server.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private Server server;
        private const int port = 10100;
        private const string ip = "127.0.0.1";
        private bool isConnected = false;

        public MainViewModel()
        {
            ObservableCollection<string> Messages = new ObservableCollection<string>();
            ObservableCollection<string> Users = new ObservableCollection<string>();
            RelayCommand StartBtnClickCmd = new RelayCommand(
                           () =>
                           {
                              
                               server = new Server(ip, port, UpdateGuiWithNewMessage);
                               server.StartAccepting();
                               isConnected = true;
                           },
                           () => { return !isConnected; });


            StopBtnClickCmd = new RelayCommand(
                //action for execute
                () =>
                {
                    server.StopAccepting();
                    isConnected = false;
                },
                //can execute
                () => { return isConnected; });


            DropClientBtnClickCmd = new RelayCommand(() =>
            {
                server.DisconnectSpecificClient(SelectedUser);
                Users.Remove(SelectedUser); // remove from GUI listbox
            },
                () => { return (SelectedUser != null); });


        }
    }
}
       
