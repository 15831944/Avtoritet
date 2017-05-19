namespace NewLauncher.ServiceReference
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using System.Threading;

    [Serializable, DebuggerStepThrough, GeneratedCode("System.Runtime.Serialization", "4.0.0.0"), DataContract(Name="AccountModel", Namespace="http://schemas.datacontract.org/2004/07/RelayServer.Entities")]
    public class AccountModel : IExtensibleDataObject, INotifyPropertyChanged
    {
        [NonSerialized]
        private ExtensionDataObject extensionDataField;
        [OptionalField]
        private bool IsOccupiedField;
        [OptionalField]
        private string NameField;
        [OptionalField]
        private string PasswordField;
        [OptionalField]
        private DateTime? SessionTimeField;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [Browsable(false)]
        public ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [DataMember]
        public bool IsOccupied
        {
            get
            {
                return this.IsOccupiedField;
            }
            set
            {
                if (!this.IsOccupiedField.Equals(value))
                {
                    this.IsOccupiedField = value;
                    this.RaisePropertyChanged("IsOccupied");
                }
            }
        }

        [DataMember]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                if (!object.ReferenceEquals(this.NameField, value))
                {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        [DataMember]
        public string Password
        {
            get
            {
                return this.PasswordField;
            }
            set
            {
                if (!object.ReferenceEquals(this.PasswordField, value))
                {
                    this.PasswordField = value;
                    this.RaisePropertyChanged("Password");
                }
            }
        }

        [DataMember]
        public DateTime? SessionTime
        {
            get
            {
                return this.SessionTimeField;
            }
            set
            {
                if (!this.SessionTimeField.Equals(value))
                {
                    this.SessionTimeField = value;
                    this.RaisePropertyChanged("SessionTime");
                }
            }
        }
    }
}

