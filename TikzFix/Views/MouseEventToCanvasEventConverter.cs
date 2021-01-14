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

            System.Windows.Point p = e.GetPosition((IInputElement)e.Source);
            var modKey = Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);

            if (value is MouseButtonEventArgs b)
            {                
                if (b.LeftButton == MouseButtonState.Pressed)
                    return new CanvasEventArgs(new Model.Tool.Point(p), MouseState.DOWN, modKey);

                if (b.LeftButton == MouseButtonState.Released)
                    return new CanvasEventArgs(new Model.Tool.Point(p), MouseState.UP, modKey);
            }

            return new CanvasEventArgs(new Model.Tool.Point(p), MouseState.MOVE, modKey);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Not needed
            throw new NotImplementedException();
        }
    }
}
