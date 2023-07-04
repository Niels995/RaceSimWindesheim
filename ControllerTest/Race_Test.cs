using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using static System.Collections.Specialized.BitVector32;

namespace ControllerTest
    {
        [TestFixture]
        public class Race_Test
        {
            private Race race;
            private Track track;

            [SetUp]
            public void Setup()
            {
                track = new Track("Race1", new[] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.LeftCorner, SectionTypes.Straight,
                SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner});
                List<IParticipant> participants = new List<IParticipant>(); // Create a list of participants
                participants.Add(new Driver("Red", 10, new Car(10, 10, 10, false), TeamColors.Red));

                participants.Add(new Driver("Blue", 10, new Car(10, 10, 10, false), TeamColors.Blue));

                participants.Add(new Driver("Yellow", 10, new Car(10, 10, 10, false), TeamColors.Yellow));

                race = new Race(track, participants);
            }

            [Test]
            public void ResetParticipants_ShouldResetParticipantProperties()
            {
                // Arrange
                foreach (IParticipant participant in race.Participants)
                {
                    participant.Lapped = 1;
                    participant.Moved = true;
                    participant.IsBroken = true;
                }

                // Act
                race.ResetParticipants();

                // Assert
                foreach (IParticipant participant in race.Participants)
                {
                    Assert.AreEqual(0, participant.Lapped);
                    Assert.IsFalse(participant.Moved);
                    Assert.IsFalse(participant.IsBroken);
                }
            }
            [Test]
            public void MakeTimer_ShouldCreateTimerWithCorrectSettings()
            {
                // Arrange
                int expectedInterval = 100;
                bool expectedAutoReset = false;

                // Act
                race.MakeTimer();
                System.Timers.Timer timer = race._timer; 

                // Assert
                Assert.IsNotNull(timer);
                Assert.IsTrue(timer.Enabled);
            }
        [Test]
        public void OnTimedEvent_Should_UpdateParticipantStates()
        {

            // Act
            race.OnTimedEvent(null, null);

            // Assert
            foreach (IParticipant participant in race.Participants)
            {
                Assert.IsFalse(participant.Moved);
                
                Assert.GreaterOrEqual(participant.Quality, 1);
                Assert.GreaterOrEqual(participant.Speed, 1);
                Assert.GreaterOrEqual(participant.Performance, 1);
            }
        }
        [Test]
        public void OnTimedEvent2_Should_UpdateParticipantStates()
        {

            // Act
            race.AllPlayersFinished = true;
            race.OnTimedEvent(null, null);

            // Assert
            foreach (IParticipant participant in race.Participants)
            {
                Assert.IsFalse(participant.Moved);
            }
        }
        [Test]
        public void RandomizeEquipment_ShouldAssignRandomValuesToParticipant()
        {
            // Arrange

            // Act
            foreach (IParticipant participant in race.Participants)
            {

                race.RandomizeEquipment(participant);

                // Assert
                Assert.GreaterOrEqual(participant.Performance, 2);
                Assert.LessOrEqual(participant.Performance, 10);
                Assert.GreaterOrEqual(participant.Speed, 2);
                Assert.LessOrEqual(participant.Speed, 6);
                Assert.GreaterOrEqual(participant.Quality, 10);
                Assert.LessOrEqual(participant.Quality, 50);
            }
        }

        [Test]
        public void MovePlayers_ShouldUpdatePlayerPositions()
        {
            // Act
            Track updatedTrack = race.MovePlayers();
            bool FirstOne = false;

            // Assert
            foreach (Model.Section sec in track.Sections)
            {
                if (!FirstOne)
                {
                    SectionData sectionData1 = race.GetSectionData(sec);
                    Assert.IsNotNull(sectionData1.Left);
                    Assert.IsNull(sectionData1.Right);
                    Assert.AreEqual(race.Participants[2], sectionData1.Left);
                    FirstOne = true;
                }
            }
        }
        [Test]
        public void NextSection_LeftSectionAvailable_PlacesParticipantOnLeft()
        {
            bool FirstOne = false;
            bool SecondOne = false;
            race.Participants.Clear();
            race.Participants.Add(new Driver("Red", 10, new Car(10, 10, 10, false), TeamColors.Red));
            foreach (Model.Section sec in track.Sections)
            {
                if (!FirstOne)
                {

                    FirstOne = true;
                }

                if (!SecondOne)
                {
                    race.ResetParticipants();
                    SectionData CurrentSectionData = race.GetSectionData(sec);
                    CurrentSectionData.DistanceLeft = 101;
                    CurrentSectionData.Left = race.Participants[0];
                    Assert.That(CurrentSectionData.Left.Name, Is.EqualTo(race.Participants[0].Name));

                    // Act
                    race.NextSection(1, race.Participants[0], CurrentSectionData, true, sec);

                    // Assert
                    Assert.That(CurrentSectionData.DistanceLeft, Is.EqualTo(0));
                    Assert.IsNull(CurrentSectionData.Left);
                    SecondOne = true;
                }
            }
        }

        [Test]
        public void PlaceParticipants_ParticipantsOnTrack()
        {
            List<IParticipant> participants = new List<IParticipant>(); // Create a list of participants
            participants.Add(new Driver("Red", 10, new Car(10, 10, 10, false), TeamColors.Red));

            participants.Add(new Driver("Blue", 10, new Car(10, 10, 10, false), TeamColors.Blue));

            participants.Add(new Driver("Yellow", 10, new Car(10, 10, 10, false), TeamColors.Yellow));

            Race race2 = new Race(track, participants);
            
            bool FirstOne = false;
            bool SecondOne = false;
            foreach (Model.Section sec in track.Sections)
            {
                if (!FirstOne)
                {
                    FirstOne = true;
                }
                else if (SecondOne)
                {
                    SectionData CurrentSectionData = race.GetSectionData(sec);
                    Assert.That(CurrentSectionData.Left, Is.EqualTo(race.Participants[0]));
                    Assert.IsNull(CurrentSectionData.Right);
                    SecondOne = true;
                }
            }
        }
        [Test]
        public void MovePlayers_TestFinish()
        {
            //race.Participants.Clear();

            int x = 0;
            //race.Participants.Add(new Driver("Red", 10, new Car(20, 20, 20, false), TeamColors.Red));
            foreach (IParticipant par in race.Participants)
            {
                par.Quality = 500;
                par.Performance = 500;
                par.Speed = 500;
            }

            race.ResetParticipants();
            for (int i = 0; i < 10; i++)
            {
                x = 0;
                foreach (Model.Section sec in track.Sections)
                {
                    SectionData CurrentSectionData = race.GetSectionData(sec);
                    if (CurrentSectionData?.Left != null)
                    {
                        CurrentSectionData.DistanceLeft = 101;

                        // Act
                        race.NextSection(x, CurrentSectionData.Left, CurrentSectionData, true, sec);
                    }
                    if (CurrentSectionData?.Right != null)
                    {
                        CurrentSectionData.DistanceRight = 101;

                        // Act
                        race.NextSection(x, CurrentSectionData.Right, CurrentSectionData, false, sec);
                    }
                    x++;
                }
            }
            foreach (IParticipant par in race.Participants)
            {
                //race.Participants = race.Participants;
                //Assert.GreaterOrEqual(par.Lapped, 1);
                Assert.IsTrue(par.Moved);
            }
        }

        [Test]
        public void Data_CompetionStart()
        {
            Data.Initialize();
            Data.NextRace();
            Assert.IsNotNull(Data.CurrentRace);
        }
        }
    }
