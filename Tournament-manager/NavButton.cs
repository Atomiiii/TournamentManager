using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tournament_manager
{
    public class NavButton : ButtonBase
    {
        static NavButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavButton), new FrameworkPropertyMetadata(typeof(NavButton)));
        }

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(NavButton), new PropertyMetadata(null));
        public static readonly DependencyProperty NavUriProperty =
            DependencyProperty.Register("NavUri", typeof(Uri), typeof(NavButton), new PropertyMetadata(null));

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        public Uri NavUri
        {
            get { return (Uri)GetValue(NavUriProperty); }
            set { SetValue(NavUriProperty, value); }
        }
    }
}
