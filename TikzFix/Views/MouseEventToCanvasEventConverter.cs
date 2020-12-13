using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

using TikzFix.Model.Tool;

namespace TikzFix.Views
{
    public class MouseEventToCanvasEventConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not MouseEventArgs e)
                throw new Exception("Wrong value type");

            Point p = e.GetPosition((IInputElement)e.Source);

            if (value is MouseButtonEventArgs b)
            {
                if (b.LeftButton == MouseButtonState.Pressed)
                    return new CanvasEventArgs((int)Math.Floor(p.X), (int)Math.Floor(p.Y), MouseState.DOWN);

                if (b.LeftButton == MouseButtonState.Released)
                    return new CanvasEventArgs((int)Math.Floor(p.X), (int)Math.Floor(p.Y), MouseState.UP);
            }

            return new CanvasEventArgs((int)Math.Floor(p.X), (int)Math.Floor(p.Y), MouseState.MOVE);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Not needed
            throw new NotImplementedException();
        }
    }
}
