using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace BasicSnak
{
    public class StyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StyleEnum style = (StyleEnum)value;
            switch (style)
            {
                case StyleEnum.Default:
                    return Brushes.White;
                    break;
                case StyleEnum.Entity:
                    return Brushes.Black;
                    break;
                case StyleEnum.Object:
                    return Brushes.Red;
                    break;
                case StyleEnum.Obstacle:
                    return Brushes.Brown;
                    break;
                default:
                    return Brushes.White;
             
            }
      
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
