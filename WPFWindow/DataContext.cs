using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFWindow
{
    public class DataContext : INotifyPropertyChanged
    {

        public ObservableCollection<CompetitionRow> CompetitionStats { get; set; } = new();
        public ObservableCollection<DriverRow> RaceDrivers { get; set; } = new();
        public ObservableCollection<DriverInfo> RaceDriversDriverInfo { get; set; } = new();
        public DataContext()
        {
            Data.CurrentRace.DriversChanged += OnDriverChanged;
        }

        public Competition competition = Data.Comp1;
        public string Property1 { get; set; }

        public int Property2 { get; set; }

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
        }

        private void OnDriverChanged(object sender, EventArgs e)
        {
            competition = Data.Comp1;

            UpdateRaceDriverInfo();
            UpdateCompetitionInfo();
            UpdateRaceDrivers();


            Property1 = Data.CurrentRace.Track.Name;
            OnPropertyChanged(nameof(Property1));
        }

        private void UpdateCompetitionInfo()
        {
            CompetitionStats = new();
            CompetitionStats.Add(new CompetitionRow(Data.CurrentRace.Track.Name, Data.CurrentRace.Rounds, competition.Participants.Count(), Data.CurrentRace.Track.Sections.Count()));
            OnPropertyChanged(nameof(CompetitionStats));
        }
        private void UpdateRaceDriverInfo()
        {
            RaceDriversDriverInfo = new();
            competition.Participants.ForEach(i =>
            {
                RaceDriversDriverInfo.Add(new DriverInfo(i.Lapped, i.Quality, i.Performance, i.Speed, i.IsBroken));
            });
            OnPropertyChanged(nameof(RaceDriversDriverInfo));
        }
        private void UpdateRaceDrivers()
        {
            RaceDrivers = new();
            competition.Participants.Where(s => Data.CurrentRace.Participants.Contains(s))
                .ToList()
                .ForEach(i => RaceDrivers.Add(new DriverRow(i.Name, i.Points, i.TeamColor.ToString())));
            OnPropertyChanged(nameof(RaceDrivers));
        }
    }
    public class DriverRow
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public string TeamColor { get; set; }

        public DriverRow(string name, int points, string teamColor)
        {
            Name = name;
            Points = points;
            TeamColor = teamColor;
        }
    }
    public class CompetitionRow
    {
        public string Name { get; set; }
        public int Laps { get; set; }
        public int TotalParticipants { get; set; }
        public int TrackLength { get; set; }
        public CompetitionRow(string name, int laps, int totalParticipants, int trackLength)
        {
            Name = name;
            Laps = laps;
            TotalParticipants = totalParticipants;
            TrackLength = trackLength;
        }
    }
    public class DriverInfo
    {
        public int Laps { get; set; }
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool Broken { get; set; }

        public DriverInfo(int lapCount, int quality, int performance, int speed, bool broken)
        {
            Laps = lapCount;
            Quality = quality;
            Performance = performance;
            Speed = speed;
            Broken = broken;
        }
    }
}
