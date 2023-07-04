using Controller;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window Menu;
        private Window Comp;
        private DataContext DataContext;
        public MainWindow()
        {
            InitializeComponent();
            DataContext dataContext = new DataContext();


            Data.CurrentRace.UpdateRace += OnDriversFinished;
            this.DataContext = dataContext;
            Race_Name.Content = Data.CurrentRace.Track.Name;
        }

        private void Close_Application(object sender, RoutedEventArgs e)
        {
            //Closing all windows/Application
            Environment.Exit(0);
        }

        private void Not_Needed(object sender, RoutedEventArgs e)
        {
            Comp = new Comp(DataContext)
            {
                Owner = this
            };
            Comp.Show(); //Dit is veranderd 
         }
        private void Open_Data(object sender, RoutedEventArgs e)
        {
            Menu = new Menu(DataContext)
            {
                Owner = this
            };
            Menu.Show();
        }

        private void OnDriversFinished(object sender, EventArgs e)
        {
            DataContext.DataContextReset();
        }
    }
}
