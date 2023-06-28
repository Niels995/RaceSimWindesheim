// See https://aka.ms/new-console-template for more information
using Controller;
using RaceSim;

Data.Initialize();
Data.NextRace();



Console.WriteLine(Data.CurrentRace.Track.Name);

Visiualize.DrawTrack(Data.CurrentRace.Track);





for (; ; )
{
    Thread.Sleep(100);
}