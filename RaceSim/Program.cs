// See https://aka.ms/new-console-template for more information
using Controller;
using RaceSim;

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


for (; ; )
{
    Thread.Sleep(100);
}