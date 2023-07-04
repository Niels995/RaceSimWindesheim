using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFWindow
{
    /// <summary>
    /// Interaction logic for Comp.xaml
    /// </summary>
    public partial class Comp : Window
    {
        public Comp()
        {
            InitializeComponent();
        }
        public Comp(DataContext dc)
        {
            DataContext = dc;
            InitializeComponent();
        }
    }
}
