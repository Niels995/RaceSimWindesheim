using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFWindow
{
    public class DataContext : INotifyPropertyChanged
    {
        public ObservableCollection<RowCompetitions> CompetitionStats { get; set; } = new();
        public ObservableCollection<RowRacer> RaceDrivers { get; set; } = new();
        public ObservableCollection<CarPerformance> RaceDriversDriverInfo { get; set; } = new();
        public DataContext()
        {
            Data.CurrentRace.DriversChanged += OnDriverChanged;
            
            Property1 = Data.CurrentRace.Track.Name;
            OnPropertyChanged(nameof(Property1));
        }

        public Competition competition = Data.Comp1;
        public string Property1 { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void DataContextReset()
        {
            Data.CurrentRace.DriversChanged += OnDriverChanged;
            UpdateCompetitionInfo();
            UpdateRaceDrivers();

            Property1 = Data.CurrentRace.Track.Name;
            OnPropertyChanged(nameof(Property1));
        }

        private void OnDriverChanged(object sender, EventArgs e)
        {
            competition = Data.Comp1;

            UpdateRaceDriverInfo();
            UpdateCompetitionInfo();
            UpdateRaceDrivers();
        }

        private void UpdateCompetitionInfo()
        {
            CompetitionStats = new();
            CompetitionStats.Add(new RowCompetitions(Data.CurrentRace.Track.Name, Data.CurrentRace.Rounds, competition.Participants.Count(), Data.CurrentRace.Track.Sections.Count()));
            OnPropertyChanged(nameof(CompetitionStats));
        }
        private void UpdateRaceDriverInfo()
        {
            RaceDriversDriverInfo = new();
            competition.Participants.ForEach(i =>
            {
                RaceDriversDriverInfo.Add(new CarPerformance(i.Lapped, i.Quality, i.Performance, i.Speed, i.IsBroken));
            });
            OnPropertyChanged(nameof(RaceDriversDriverInfo));
        }
        private void UpdateRaceDrivers()
        {
            RaceDrivers = new();
            competition.Participants.Where(s => Data.CurrentRace.Participants.Contains(s))
                .ToList()
                .ForEach(i => RaceDrivers.Add(new RowRacer(i.Name, i.Points, i.TeamColor.ToString())));
            OnPropertyChanged(nameof(RaceDrivers));
        }
    }
    public class RowRacer
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public string TeamColor { get; set; }

        public RowRacer(string name, int points, string teamColor)
        {
            Name = name;
            Points = points;
            TeamColor = teamColor;
        }
    }
    public class RowCompetitions
    {
        public string Name { get; set; }
        public int Laps { get; set; }
        public int TotalParticipants { get; set; }
        public int TrackLength { get; set; }
        public RowCompetitions(string name, int laps, int totalParticipants, int trackLength)
        {
            Name = name;
            Laps = laps;
            TotalParticipants = totalParticipants;
            TrackLength = trackLength;
        }
    }
    public class CarPerformance
    {
        public int Laps { get; set; }
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool Broken { get; set; }

        public CarPerformance(int lapCount, int quality, int performance, int speed, bool broken)
        {
            Laps = lapCount;
            Quality = quality;
            Performance = performance;
            Speed = speed;
            Broken = broken;
        }
    }
}
