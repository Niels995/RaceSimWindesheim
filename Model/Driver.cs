using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Driver : IParticipant
    {
        public Driver(String name, int points, IEquipment equipment, TeamColors teamColors)
        {
            Name = name;
            Points = points;
            Equipment = equipment;
            TeamColor = teamColors;
        }
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }
        public bool Moved { get; set; }
        public int Lapped { get; set; }
    }
}
