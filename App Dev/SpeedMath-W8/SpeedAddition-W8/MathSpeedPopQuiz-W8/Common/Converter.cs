using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;


namespace MathSpeedPopQuiz_W8.Common
{
    class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolValue = (bool)value;
            if (boolValue)
                return new SolidColorBrush(Colors.Green);
            return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            SolidColorBrush brushValue = value as SolidColorBrush;
            if (brushValue == new SolidColorBrush(Colors.Red))
                return 1;
            return 0;
        }
    }
}
