using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum SectionTypes { 
        Straight,
        LeftCorner,
        RightCorner,
        StartGrid,
        Finish
    }
    public class Section
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Compass { get; set; } = 1;
        public SectionTypes SectionType { get; set; }
        public Section(SectionTypes sectionType)
        {
            SectionType = sectionType;
        }
    }
}
