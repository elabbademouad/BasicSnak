using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSnak
{
    public class CellViewModel:INotifyPropertyChanged
    {
        private PositionViewModel _Position;

        public PositionViewModel Position
        {
            get { return _Position; }
            set { _Position = value; }
        }
        private StyleEnum _Style;

        public StyleEnum Style
        {
            get { return _Style; }
            set { _Style=value;OnPropertyChanged("Style"); }
        }
        private CellViewModel _Next;
                
        public CellViewModel Next
        {
            get { return _Next; }
            set { _Next = value; }
        }
        private CellViewModel _Previout;

        public event PropertyChangedEventHandler PropertyChanged;

        public CellViewModel Previout
        {
            get { return _Previout; }
            set { _Previout = value; }
        }
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




    }
}
