using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpeedyCoding
{
    public static class SpeedyCoding_WPF
    {
        public static T SetGridPos<T>(
            this T src
            , int row
            , int col )
            where T : Control
        {
            Grid.SetRow( src , row );
            Grid.SetColumn( src , col );
            return src;
        }
    }
}
