using System;

namespace ClassifierAlgorithms.GUI.ViewModel
{
    public partial class MainWindowViewModel
    {
        #region IDataErrorInfo

        public string this[string columnName]
        {
            get { throw new NotImplementedException(); }
        }

        public string Error { get; }

        #endregion
    }
}