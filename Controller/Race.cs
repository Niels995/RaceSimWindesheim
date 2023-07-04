using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static System.Collections.Specialized.BitVector32;
using Section = Model.Section;
using Timer = System.Timers.Timer;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }
        private Random _random { get; set; }
        private Dictionary<Section, SectionData> _positions { get; set; } = new Dictionary<Section, SectionData>();
        public int Rounds { get; set; } = 1;
        public Timer _timer { get; set; } // public for testing

        public event EventHandler<DriversChangedEventArgs> DriversChanged;

        public event EventHandler<EventArgs> DriversFinished;

        public event EventHandler<EventArgs> UpdateRace;

        public bool AllPlayersFinished = false;

        public Race(Track track, List<IParticipant> participants)
        {
            StartTime = DateTime.Now;
            Track = track;
            Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            ResetParticipants();
            foreach (var participant in Participants)
            {
                RandomizeEquipment(participant);
            }
            PlaceParticipants();
            MakeTimer();
            AddPowerUp();
        }
        public void ResetParticipants() {
            foreach (IParticipant par in Participants) {
                par.Lapped = 0;
                par.Moved = false;
                par.IsBroken = false;
            }
        }

        public void MakeTimer()
        {
            // Create a timer with a half a second interval.
            _timer = new Timer(100);
            // Hook up the Elapsed event for the timer. 
            _timer.Elapsed += OnTimedEvent;

            _timer.AutoReset = false;
            _timer.Start();
        }
        public void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            foreach (IParticipant par in Participants)
            {
                par.Moved = false;
                if (_random.Next(3, par.Quality) >= 5) {
                    if (par.PowerUp > 0)
                    {
                        par.PowerUp -= 1;
                    }
                    else
                    {
                        par.IsBroken = true;
                    }
                }
                if (par.IsBroken && _random.Next(1, 8) <= 2) { 
                    par.IsBroken = false;
                    RandomizeEquipment(par);
                }
            }
            

            Track _track = MovePlayers();
            DriversChanged?.Invoke(sender, new DriversChangedEventArgs() { track = Track });
            if (!AllPlayersFinished)
            {
                _timer?.Start();
            }
            else {
                if (DriversChanged?.GetInvocationList() != null)
                {
                    foreach (Delegate d in DriversChanged.GetInvocationList())
                    {
                        DriversChanged -= (EventHandler<DriversChangedEventArgs>)d;
                    }
                }
                DriversFinished?.Invoke(this, new EventArgs());
                if (DriversFinished?.GetInvocationList() != null)
                {
                    foreach (Delegate d in DriversFinished.GetInvocationList())
                    {
                        DriversFinished -= (EventHandler<EventArgs>)d;
                    }
                }
                UpdateRace?.Invoke(this, new EventArgs());
                _timer?.Stop();
            }
        }
        public void RandomizeEquipment(IParticipant participant) { 
            participant.Performance = _random.Next(2, 10);
            participant.Speed = _random.Next(2, 6);
            participant.Quality = _random.Next(10, 50);
        }
        public Track MovePlayers() {
            Track _track = Track;
            int i = 0;
            foreach (Section sec in _track.Sections) {
                SectionData CurrentSectionData = GetSectionData(sec);
                
                if (CurrentSectionData.Left != null && CurrentSectionData.Left.Name != "@PowerUp") {
                    if (!CurrentSectionData.Left.IsBroken)
                    {
                        if (CurrentSectionData.DistanceLeft < 100)
                        {
                            CurrentSectionData.DistanceLeft += CurrentSectionData.Left.Performance * CurrentSectionData.Left.Speed;
                        }
                        if (CurrentSectionData.DistanceLeft >= 100)
                        {
                            // Move to next space
                            NextSection(i, CurrentSectionData.Left, CurrentSectionData, true, sec);
                        }
                    }
                }
                if (CurrentSectionData.Right != null) {
                    if (!CurrentSectionData.Right.IsBroken)
                    {
                        if (CurrentSectionData.DistanceRight < 100)
                        {
                            CurrentSectionData.DistanceRight += CurrentSectionData.Right.Performance * CurrentSectionData.Right.Speed;
                        }
                        if (CurrentSectionData.DistanceRight >= 100)
                        {
                            // Move to next space
                            NextSection(i, CurrentSectionData.Right, CurrentSectionData, false, sec);
                        }
                    }
                }
                i++;
                _positions[sec] = CurrentSectionData;
            }
            return _track;
        }
        public void NextSection(int i, IParticipant par, SectionData CurrentData, bool Left, Section sec)
        {
            bool Place = false;
            if (!par.Moved)
            {
                int x = i;
                if (x + 1 == Track.Sections.Count())
                {
                    x = 0;
                }
                Section NextSection = Track.Sections.ElementAt(x + 1);
                SectionData NextSectionData = GetSectionData(NextSection);

                if (NextSectionData.Left == null || NextSectionData.Left.Name == "@PowerUp")
                {
                    if (NextSectionData.Left != null) {
                        if (NextSectionData.Left.Name == "@PowerUp")
                        {
                            par.PowerUp = 5;
                        }
                    } 
                    NextSectionData.Left = par;
                    par.Moved = true;
                    Place = true;
                    if (sec.SectionType == SectionTypes.Finish)
                    {
                        par.Lapped += 1;
                        Debug.WriteLine("Lapped" + par.Lapped);
                        if (par.Lapped == Rounds + 1)
                        {
                            NextSectionData.Left = null;
                            foreach (IParticipant _participant in Participants)
                            {
                                if (_participant.Lapped != Rounds + 1)
                                {
                                    AllPlayersFinished = false;
                                    _timer.Stop();
                                    break;
                                }
                                else
                                {
                                    AllPlayersFinished = true;
                                }
                            }

                            int PointsRewarded = 1;
                            foreach (IParticipant _participant in Participants)
                            {
                                if (_participant.Lapped != Rounds + 1)
                                {
                                    PointsRewarded++;
                                }
                            }

                            par.Points += PointsRewarded;
                        }
                    }
                }
                else if (NextSectionData.Right == null)
                {
                    NextSectionData.Right = par;
                    par.Moved = true;
                    Place = true;
                    if (sec.SectionType == SectionTypes.Finish)
                    {
                        par.Lapped += 1;
                        if (par.Lapped == Rounds + 1) {
                            NextSectionData.Right = null;
                            foreach (IParticipant _participant in Participants)
                            {
                                if (_participant.Lapped != Rounds + 1)
                                {
                                    AllPlayersFinished = true;
                                    _timer.Stop();
                                    break;
                                }
                            }
                            int PointsRewarded = 1;
                            foreach (IParticipant _participant in Participants)
                            {
                                if (_participant.Lapped != Rounds + 1) { PointsRewarded++; }
                            }
                            par.Points += PointsRewarded;
                        }
                    }
                }
                if (Place)
                {
                    if (Left)
                    {
                        CurrentData.DistanceLeft = 0;
                        CurrentData.Left = null;
                    }
                    else
                    {
                        CurrentData.DistanceRight = 0;
                        CurrentData.Right = null;
                    }
                }
            }
        }

        // Wordt wel getest in Race_Test.cs bij PlaceParticipants_ParticipantsOnTrack()
        public void PlaceParticipants()
        {
            Stack<Section> _sections = new Stack<Section>();
            Section finish = Track.Sections.Where(x => x.SectionType == SectionTypes.Finish).First();
            foreach (Section section in Track.Sections) {
                if (section.SectionType == SectionTypes.StartGrid) { _sections.Push(section); };
                if (section == finish) { break; };
            }

            Section CurrentSection = _sections.Pop();
            foreach (IParticipant participant in Participants) {
                SectionData CurrentSectionData = GetSectionData(CurrentSection);
                if (CurrentSectionData.Left == null)
                {
                    CurrentSectionData.Left = participant;
                }
                else if (CurrentSectionData.Right == null)
                {
                    CurrentSectionData.Right = participant;
                    if (_sections.Count() != 0)
                    {
                        CurrentSection = _sections.Pop();
                    }
                }
            }
        }
        public void AddPowerUp()
        {
            bool PowerUp = true;
            foreach (Section section in Track.Sections)
            {
                if (PowerUp)
                {
                    if (section.SectionType == SectionTypes.Straight)
                    {
                        if (_random.Next(1, Track.Sections.Count / 2) <= 2)
                        {
                            SectionData sc = GetSectionData(section);
                            sc.Left = new Driver("@PowerUp", 10, new Car(10, 10, 10, false), TeamColors.Red);
                            PowerUp = false;
                        }
                    }
                }
            }
        }

        public SectionData GetSectionData(Section Section)
        {
            if (_positions.ContainsKey(Section))
            {
                return _positions[Section];
            }
            else
            {
                SectionData _sectionData = new SectionData();
                _positions.Add(Section, _sectionData);
                return _sectionData;
            }
        }
    }
}
