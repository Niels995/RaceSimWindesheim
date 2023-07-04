//// See https://aka.ms/new-console-template for more information
//using Controller;
//using RaceSim;

using Controller;
using RaceSim;

namespace Race_Simulator
{
    internal class Program
    {
        private static void Main()
        {

            Data.Initialize();
            Data.NextRace();



            Console.WriteLine(Data.CurrentRace.Track.Name);

            Visiualize.Initialize(Data.CurrentRace);

            Data.CurrentRace.DriversChanged += (sender, e) =>
            {
                Visiualize.DrawTrack(e.track);
            };


            Data.CurrentRace.DriversFinished += (sender, e) =>
            {
                Visiualize.EventHandlerDriversFinished();
            };

            Data.CurrentRace.DriversFinished += (sender, e) =>
            {
                Race race = Data.CurrentRace;
            };

            Thread thread = new Thread(new ThreadStart(() =>
            {
                // Replace 'WPF' with the namespace of your WPF application
                WPFWindow.MainWindow app = new WPFWindow.MainWindow();
                app.InitializeComponent();
                app.ShowDialog(); // This will show your window
            }));

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            for (; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}