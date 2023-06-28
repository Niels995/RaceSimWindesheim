using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition Comp1 = new Competition();
        public static Race CurrentRace {get; set;}
        public static void Initialize() {
            AddParticipant();
            AddTracks();
        }

        public static void NextRace() { 
            Track _track = Comp1.NextTrack();
            if (_track is Track) {
                CurrentRace = new Race(_track, Comp1.Participants);
            }
            
        }

        public static void AddParticipant() {
            Comp1.Participants.Add(new Driver("Red", 10, new Car(10, 10, 10, false), TeamColors.Red));

            Comp1.Participants.Add(new Driver("Blue", 10, new Car(10, 10, 10, false), TeamColors.Blue));

            Comp1.Participants.Add(new Driver("Yellow", 10, new Car(10, 10, 10, false), TeamColors.Yellow));
        }
        public static void AddTracks()
        {
            Track _trackTest = new Track("Race1", new[] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.LeftCorner, SectionTypes.Straight,
                SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner});
            //Track _trackTest = new Track("Race1", new[] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.RightCorner,
            //SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner});
            SectionTypes[] _sections1 = { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.LeftCorner, SectionTypes.Straight,
                SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,SectionTypes.RightCorner, SectionTypes.Straight,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner};
            Track _trackTest1 = new Track("Race2", _sections1);
            Comp1.Tracks.Enqueue(_trackTest);
            Comp1.Tracks.Enqueue(_trackTest1);
        }
    }
}
