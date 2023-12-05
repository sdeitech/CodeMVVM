using GalaSoft.MvvmLight.Command;
using System;
using System.Net.NetworkInformation;
using ViewModel.Model;
using ViewModel.Repository;
using System.Windows;

namespace ViewModel.ViewModel
{
    //ViewModel class for handling setup-related operations.
    public class SetupViewModel : Base.ViewModelBase
    {

        #region Base Methods
        // Override method for initializing collections (not implemented in this case)
        public override void IntitializeCollections()
        {
        }
        #endregion

        #region Constructor
        //Initializes a new instance of the<see cref="SetupViewModel"/> class.
        public SetupViewModel()
        {

        }
        #endregion

        #region Properties
  
        // Properties for company setup information
        public string CompanyName
        {

            get { return GetValue(() => this.CompanyName); }
            set { SetValue(() => this.CompanyName, value); }
        }

        public string UserName
        {

            get { return GetValue(() => this.UserName); }
            set { SetValue(() => this.UserName, value); }
        }

        public string PassWord
        {

            get { return GetValue(() => this.PassWord); }
            set { SetValue(() => this.PassWord, value); }
        }


        #endregion

        #region Commands
        // Commands for saving and canceling company information
        private RelayCommand saveCompanyCommand;
        /// Gets the command for saving company information.
        public RelayCommand SaveCompanyCommand
        {
            get
            {
                return saveCompanyCommand ?? (saveCompanyCommand = new RelayCommand(SaveCompanyInfo));
            }
        }

        private RelayCommand cancelCompanyCommand;

        //Gets the command for canceling company information.
        public RelayCommand CancelCompanyCommand
        {
            get { return cancelCompanyCommand ?? (cancelCompanyCommand = new RelayCommand(CancelCompanyInfo)); }
        }

        #endregion

        // Method for saving company information
        public void SaveCompanyInfo()
        {
            // Implementation for saving company information
            string machineAddress = GetMacAddress();
            RequestModel companySetupModel = new RequestModel();
            companySetupModel.CompanyName = CompanyName;
            companySetupModel.UserName = UserName;
            companySetupModel.Password = PassWord;
            if (companySetupModel.CompanyName != null || companySetupModel.UserName != null || companySetupModel.Password != null)
            {
                var companyDetails = SetupRepository.GetCompanyForSetup(companySetupModel);
                string JWEToken = companyDetails.Data.JWEToken;
                string CompanyId = EncryptClass.Encrypt(Convert.ToString(companyDetails.Data.CompanyId));
                ApplicationSettings applicationSettings = new ApplicationSettings();
                applicationSettings.JWEToken = companyDetails.Data.JWEToken;
                applicationSettings.CompanyId = CompanyId;
                applicationSettings.CompanyName = companyDetails.Data.CompanyName;
                applicationSettings.MachineName = System.Environment.MachineName;
                applicationSettings.MachineAddress = machineAddress;

                FileManagement.CreateXML(applicationSettings);

                ClearControl();

            }
            else
            {
                
            }
        }
        // Method for clearing input controls
        private void CancelCompanyInfo()
        {
            // Implementation for clearing input controls
            ClearControl();
        }
        public void ClearControl()
        {
            CompanyName = string.Empty;
            UserName = string.Empty;
            PassWord = string.Empty;
        }
        // Method for getting the MAC address of the machine
        public static string GetMacAddress()
        {
            // Implementation for obtaining MAC address
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = "";
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed && !String.IsNullOrEmpty(tempMac) && tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                {
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }

            return macAddress;
        }
        
    }
}
