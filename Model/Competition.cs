using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; } = new List<IParticipant>();
        public Queue<Track> Tracks { get; set; } = new Queue<Track>();
        public Track NextTrack() {
            Track _track = null;
            try { _track = Tracks.Dequeue(); } catch { return null; };
            if (_track is null) {
                return null;
            } else {
                return _track;
            }
        }
        public Competition() {  }
        public Competition(List<IParticipant> participants, Queue<Track> tracks)
        {
            Participants = participants;
            Tracks = tracks;
        }
    }
}
