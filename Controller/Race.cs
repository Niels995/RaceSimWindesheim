using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }
        private Random _random { get; set; }
        private Dictionary<Section, SectionData> _positions { get; set; }

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            RandomizeEquipment();
            
        }
        public void RandomizeEquipment() { 
            foreach (var participant in Participants)
            {
                participant.Performance = _random.Next(1, 50); ;

                participant.Quality = _random.Next(1, 50); ;
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
