using Model;
using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;
using System.Xml.Linq;
using WPFWindow;
using Track = Model.Track;

namespace RaceSim
{
    public static class Visiualize
    {
        public static Race CurrentRace;
        public static void Initialize(Race race) {
            CurrentRace = race;
            DrawTrack(race.Track);
        }
        public static void EventHandlerDriversChanged(DriversChangedEventArgs driversChanged)
        {
            DrawTrack(driversChanged.track);
        }
        internal static void EventHandlerDriversFinished()
        {
            Data.CurrentRace = null;
            Data.NextRace();
            Console.Clear();
            if (Data.CurrentRace is not null)
            {
                Console.WriteLine(Data.CurrentRace.Track.Name);
                Visiualize.Initialize(Data.CurrentRace);

                if (Data.CurrentRace != null)
                {
                    Data.CurrentRace.DriversChanged += (sender, e) => { DrawTrack(e.track); };


                    Data.CurrentRace.DriversFinished += (sender, e) => { EventHandlerDriversFinished(); };
                }
            }
        }

        public static void DrawTrack(Track track) {
            int[] coords = FindStartXY(track);
            int x = coords[0];
            int y = coords[1];
            if (x < 0) { x = -coords[0]; };
            if (y < 0) { y = -coords[1]; };

            foreach (Section sec in track.Sections)
            {
                Console.SetCursorPosition((sec.X + x) * 7, (sec.Y + y) * 7);
                if (sec.SectionType == SectionTypes.Straight)
                {
                    switch (sec.Compass)
                    {
                        case 1:
                        case 3:
                            PrintSection(sec ,Horizontaal, (sec.X + x), (sec.Y + y));
                            break;
                        case 2:
                        case 4:
                            PrintSection(sec, Verticaal, (sec.X + x), (sec.Y + y));
                            break;
                    }
                }
                if (sec.SectionType == SectionTypes.Finish) {PrintSection(sec, Finish, (sec.X + x), (sec.Y + y)); };
                if (sec.SectionType == SectionTypes.StartGrid) { PrintSection(sec, Start, (sec.X + x), (sec.Y + y)); };
                if (sec.SectionType == SectionTypes.RightCorner)
                {
                    switch (sec.Compass)
                    {
                        case 1:
                            PrintSection(sec, Rechts1, (sec.X + x), (sec.Y + y));
                            break;
                        case 2:
                            PrintSection(sec, Rechts2, (sec.X + x), (sec.Y + y));
                            break;
                        case 3:
                            PrintSection(sec, Rechts3, (sec.X + x), (sec.Y + y));
                            break;
                        case 4:
                            PrintSection(sec, Rechts4, (sec.X + x), (sec.Y + y));
                            break;
                    }
                }
                if (sec.SectionType == SectionTypes.LeftCorner)
                {
                    switch (sec.Compass)
                    {
                        case 1:
                            PrintSection(sec, Rechts2, (sec.X + x), (sec.Y + y));
                            break;
                        case 2:
                            PrintSection(sec, Rechts3, (sec.X + x), (sec.Y + y));
                            break;
                        case 3:
                            PrintSection(sec, Rechts4, (sec.X + x), (sec.Y + y));
                            break;
                        case 4:
                            PrintSection(sec, Rechts1, (sec.X + x), (sec.Y + y));
                            break;
                    }
                }
            }
        }
        public static void PrintSection(Section sec, string[] printLine, int X, int Y) {
            for (int i = 0; i <= 7; i++)
            {
                string Line = printLine[i];
                SectionData SectionData = CurrentRace.GetSectionData(sec);
                if (SectionData.Left is not null)
                {
                    string Name = SectionData.Left.Name;
                    Line = Line.Replace("*", Name.Remove(1));
                    if (SectionData.Left.IsBroken)
                    {
                        Line = Line.Replace(Name.Remove(1), "X");
                    }
                }
                if (SectionData.Right is not null)
                {
                    string Name = SectionData.Right.Name;
                    Line = Line.Replace("+", Name.Remove(1));
                    if (SectionData.Right.IsBroken)
                    {
                        Line = Line.Replace(Name.Remove(1), "X");
                    }
                }
                Line = Line.Replace("+", " ");
                Line = Line.Replace("*", " ");
                Console.SetCursorPosition(X * 7, Y * 7 + i);
                Console.Write(Line);
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
