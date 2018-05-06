using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSnak
{
    public class PositionViewModel:INotifyPropertyChanged
    {
        private int _X;

        public int X
        {
            get { return _X; }
            set { _X = value;OnPropertyChanged("X"); }
        }
        private int _Y;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Y
        {
            get { return _Y; }
            set { _Y = value; OnPropertyChanged("Y"); }
        }
        public override string ToString()
        {
            return string.Format("{0}{1}",X,Y);
        }
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
