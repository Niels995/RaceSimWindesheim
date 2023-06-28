using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }
        public Track(string name, SectionTypes[] sections)
        {
            Name = name;
            Sections = ArrayToLinkedList(sections);
        }
        public LinkedList<Section> ArrayToLinkedList(SectionTypes[] SectionTypes)
        {
            LinkedList<Section> _sections = new LinkedList<Section>();

            foreach (SectionTypes SectionType in SectionTypes)
            {
                Section Section = new Section(SectionType);
                _sections.AddLast(Section);
            }
            return _sections;
        }
    }
}
