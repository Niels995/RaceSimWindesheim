using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RaceSim
{
    public static class Visiualize
    {
        public static void Initialize(Track track) {
            DrawTrack(track);
        }

        public static void DrawTrack(Track track) {
            int[] coords = FindStartXY(track);
            int x = coords[0];
            int y = coords[1];
            if (x < 0) { x = -coords[0]; };
            if (y < 0) { y = -coords[1]; };
            Console.WriteLine(x + "  " + y);
            string[] hallo = Horizontaal;

            Console.SetCursorPosition(x * 8, y * 8);
        }
        public static int[] FindStartXY(Track track) {
            int[] coords = new int[2];

            int Compass = 1; // Used for flipping

            foreach (Section sec in track.Sections) {
                //if (sec.SectionType == SectionTypes.StartGrid || sec.SectionType == SectionTypes.Straight || sec.SectionType == SectionTypes.Finish) {
                coords = SwitchCompass(sec, coords, Compass);
                //}
                if (sec.SectionType == SectionTypes.RightCorner || sec.SectionType == SectionTypes.LeftCorner)
                {
                    //SwitchCompass(sec, coords, Compass);
                    if (sec.SectionType == SectionTypes.RightCorner)
                    {
                        Compass += 1;
                        if (Compass == 5) { Compass = 1; };
                    }
                    else {
                        Compass -= 1;
                        if (Compass == 0) { Compass = 4; };
                    }
                }
            }

            return coords;
        }
        public static int[] SwitchCompass(Section sec, int[] coords, int Compass) {
            switch (Compass)
            {
                case 1:
                    coords[0] += 1;
                    SetXYCompass(sec, coords, Compass);
                    break;
                case 2:
                    coords[1] += 1;
                    SetXYCompass(sec, coords, Compass);
                    break;
                case 3:
                    coords[0] += -1;
                    SetXYCompass(sec, coords, Compass);
                    break;
                case 4:
                    coords[1] += -1;
                    SetXYCompass(sec, coords, Compass);
                    break;
            }
            return coords;
        }
        public static void SetXYCompass(Section sec, int[] coords, int compass) {
            sec.X = coords[0];
            sec.Y = coords[1];
            sec.Compass = compass;
        }
        public static int horizontal() { 
            return 1; 
        }
        public static int Corner()
        {
            return 0;
        }
        #region graphics

        public static string[] Links =
        {
                 " |   | ",
                 "-/   | ",
                 "*    | ",
                 "     | ",
                 "     / ",
                 "   +/  ",
                 "---/   ",
                 "       "

        };
        public static string[] Rechts =
        {
                "       ",
                "---\\   ",
                "   *\\  ",
                "     \\ ",
                "     | ",
                "     | ",
                "-\\+  | ",
                " |   | "

        };
        public static string[] Horizontaal =
        {
                "       ",
                "-------",
                " *     ",
                "       ",
                "       ",
                "  +    ",
                "-------",
                "       ",

        };
        public static string[] Start =
        {
                "       ",
                "-------",
                "  *    ",
                "       ",
                "       ",
                "  +    ",
                "-------",
                "       "

        };
        public static string[] Finish =
        {
                "       ",
                "----#--",
                " *  #  ",
                "    #  ",
                "    #  ",
                " +  #  ",
                "----#--",
                "       "

        };
        #endregion
    }
}
