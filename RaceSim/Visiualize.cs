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
            Console.Clear();
            int[] coords = FindStartXY(track);
            int x = coords[0];
            int y = coords[1];
            if (x < 0) { x = -coords[0]; };
            if (y < 0) { y = -coords[1]; };

            Console.Clear();
            foreach (Section sec in track.Sections)
            {
                Console.SetCursorPosition((sec.X + x) * 7, (sec.Y + y) * 7);
                if (sec.SectionType == SectionTypes.Straight)
                {
                    switch (sec.Compass)
                    {
                        case 1:
                        case 3:
                            PrintSection(Horizontaal, (sec.X + x), (sec.Y + y));
                            break;
                        case 2:
                        case 4:
                            PrintSection(Verticaal, (sec.X + x), (sec.Y + y));
                            break;
                    }
                }
                if (sec.SectionType == SectionTypes.Finish) {PrintSection(Finish, (sec.X + x), (sec.Y + y)); };
                if (sec.SectionType == SectionTypes.StartGrid) { PrintSection(Start, (sec.X + x), (sec.Y + y)); };
                if (sec.SectionType == SectionTypes.RightCorner)
                {
                    switch (sec.Compass)
                    {
                        case 1:
                            PrintSection(Rechts1, (sec.X + x), (sec.Y + y));
                            break;
                        case 2:
                            PrintSection(Rechts2, (sec.X + x), (sec.Y + y));
                            break;
                        case 3:
                            PrintSection(Rechts3, (sec.X + x), (sec.Y + y));
                            break;
                        case 4:
                            PrintSection(Rechts4, (sec.X + x), (sec.Y + y));
                            break;
                    }
                }
                if (sec.SectionType == SectionTypes.LeftCorner)
                {
                    switch (sec.Compass)
                    {
                        case 1:
                            PrintSection(Rechts2, (sec.X + x), (sec.Y + y));
                            break;
                        case 2:
                            PrintSection(Rechts3, (sec.X + x), (sec.Y + y));
                            break;
                        case 3:
                            PrintSection(Rechts4, (sec.X + x), (sec.Y + y));
                            break;
                        case 4:
                            PrintSection(Rechts1, (sec.X + x), (sec.Y + y));
                            break;
                    }
                }
            }
        }
        public static void PrintSection(string[] printLine, int X, int Y) {
            for (int i = 0; i <= 7; i++)
            {
                Console.SetCursorPosition(X * 7, Y * 7 + i);
                Console.Write(printLine[i]);
            }
        }

        public static int[] FindStartXY(Track track) {
            int[] coords = new int[2];
            coords[0] = 1;
            coords[1] = 1;
            int lowestX = 0;
            int lowestY = 0;

            int Compass = 1; // Used for flipping
            foreach (Section sec in track.Sections) {
                Console.WriteLine(coords[0] + "  " + coords[1]);
                if (sec.SectionType == SectionTypes.StartGrid || sec.SectionType == SectionTypes.Straight || sec.SectionType == SectionTypes.Finish) {
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
                            coords[0] -= 1;
                            SetXYCompass(sec, coords, Compass);
                            break;
                        case 4:
                            coords[1] -= 1;
                            SetXYCompass(sec, coords, Compass);
                            break;
                    }
                }
                if (sec.SectionType == SectionTypes.RightCorner || sec.SectionType == SectionTypes.LeftCorner)
                {
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
                            coords[0] -= 1;
                            SetXYCompass(sec, coords, Compass);
                            break;
                        case 4:
                            coords[1] -= 1;
                            SetXYCompass(sec, coords, Compass);
                            break;
                    }
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
                if (coords[0] < lowestX) { lowestX = coords[0]; };
                if (coords[1] < lowestY) { lowestY = coords[1]; };
            }
            int[] lowest = { lowestX, lowestY };
            return lowest;
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

        public static string[] Rechts1 =
        {
                "       ",
                "---\\   ",
                "   +\\  ",
                "     \\ ",
                "     | ",
                "     | ",
                "-\\*  | ",
                " |   | "

        };
        public static string[] Rechts2 =
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
        public static string[] Rechts3 =
{
                 " |   | ",
                 " |   \\-",
                 " |   * ",
                 " |     ",
                 " |     ",
                 " \\+    ",
                 "  \\----",
                 "       "

        };
        public static string[] Rechts4 =
{
                "       ",
                "   /---",
                "  /+   ",
                " /     ",
                " |     ",
                " |  *  ",
                " |   /-",
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
        public static string[] Verticaal =
        {
                " |   | ",
                " |   | ",
                " |   | ",
                " |* +| ",
                " |   | ",
                " |   | ",
                " |   | ",
                " |   | "

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

    }
    #endregion
}
