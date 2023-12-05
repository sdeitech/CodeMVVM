using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using CommunityToolkit.Mvvm.Messaging;

namespace ViewModel
{
    public static class AppMessenger
    {
       

        public static void Send(AppMessage message)
        {
            Messenger.Default.Send<AppMessage>(message);
        }

        public static void Close(AppMessage message)
       
        {
            Messenger.Default.Send<AppMessage>(message);
            //Send(new AppMessage() { MessageType = AppMessageType.Close} );
        }

        public static bool ShowPopup(object MessageData = null, AppMessageType MessageType = AppMessageType.PopUpMessage)
        {
            Send(new AppMessage() { MessageType = MessageType, MessageData = MessageData });

            return ApplicationState.GetValueAndClear<bool>(AppVariables.DialogResult.ToString());
        }
    }
    public class AppMessage
    {
        public AppMessageType MessageType { get; set; }
        public object MessageData { get; set; }
    }
    public enum AppMessageType
    {
        
        PleaseEnterCredential,
        ConfirmMessage, ConfirmWarningMessage, PopUpMessage,
        Close, YesNOMessage,  PopupWarning, SaveDataToCDrive,
    }
    public enum AppVariables
    {
        DialogResult
    }
    public static class ApplicationState
    {
        private static Dictionary<string, object> _values =
                   new Dictionary<string, object>();
        public static void SetValue(string key, object value)
        {
            if (_values.ContainsKey(key))
            {
                _values.Remove(key);
            }
            _values.Add(key, value);
        }

        public static T GetValue<T>(string key)
        {
            if (_values.ContainsKey(key))
            {
                return (T)_values[key];
            }
            else
            {
                return default(T);
            }
        }

        public static T GetValueAndClear<T>(string key)
        {
            if (_values.ContainsKey(key))
            {
                T valueToReturn = (T)_values[key];
                ClearKey(key);
                return valueToReturn;
            }
            else
            {
                return default(T);
            }
        }

        public static void ClearData()
        {
            _values.Clear();
        }

        public static void ClearKey(string key)
        {
            if (_values.ContainsKey(key))
            {
                _values.Remove(key);
            }
        }
    }
}
