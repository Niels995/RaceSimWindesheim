using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Reflection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using RaceSim;
namespace ControllerTest
{

    [TestFixture]
    internal class Visualize_Test
    {
        private Race race;
        [SetUp]
        public void Setup()
        {
            Track track = new Track("Race1", new[] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.LeftCorner, SectionTypes.Straight,
                SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner});
            List<IParticipant> participants = new List<IParticipant>(); // Create a list of participants
            participants.Add(new Driver("Red", 10, new Car(10, 10, 10, false), TeamColors.Red));

            participants.Add(new Driver("Blue", 10, new Car(10, 10, 10, false), TeamColors.Blue));

            participants.Add(new Driver("Yellow", 10, new Car(10, 10, 10, false), TeamColors.Yellow));
            Competition competition = new Competition();
            competition.Participants = participants;
            competition.Tracks.Enqueue(track);
            Data.Initialize();
            Data.NextRace();

            race = new Race(track, participants);
        }

        [Test]
        public void InitializeVisualize()
        {
            Visiualize.Initialize(race);
            Assert.IsNotNull(Visiualize.CurrentRace);
        }
    }
}
