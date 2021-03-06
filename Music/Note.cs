﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Music
{
    public class Note
    {
        public string Name { get; private set; }
        public int TargetFrequency { get; private set; }
        public List<string> Positions { get; private set; }

        private int frequency;
        private List<Dictionary<int, string>> frequencyNotesByString = new List<Dictionary<int, string>>();
        private KeyValuePair<int, string> previousLowerNote;
        private KeyValuePair<int, string> nextHigherNote;        
        
        public Note(int frequency)
        {
            Name = "N/A";
            TargetFrequency = 0;
            Positions = new List<string>();

            this.frequency = frequency;           
            frequencyNotesByString = GetFrequencyNotesByString();
            FindName();
            FindPositions();
        }

        private void FindName()
        {
            foreach (Dictionary<int, string> stringNotes in frequencyNotesByString)
            {
                if (!IsFoundOnString(stringNotes))
                {
                    continue;
                }

                GetClosestNoteIn(stringNotes);
                return;
            }       
        }

        private bool IsFoundOnString(Dictionary<int, string> stringNotes)
        {
            return frequency <= stringNotes.Keys.Last();
        }

        private void GetClosestNoteIn(Dictionary<int, string> stringNotes)
        {
            if (stringNotes.ContainsKey(frequency))
            {
                Name = stringNotes[frequency];
                return;
            }

            FindNeighboringNotesFrom(stringNotes);

            if (IsCloserToLowerNote())
            {
                TargetFrequency = previousLowerNote.Key;
                Name = stringNotes[previousLowerNote.Key];
                return;
            }

            TargetFrequency = nextHigherNote.Key;
            Name = stringNotes[nextHigherNote.Key];
        }        

        private void FindNeighboringNotesFrom(Dictionary<int, string> stringNotes)
        {
            var noteFrequencies = new List<int>(stringNotes.Keys).ToArray();

            int index = Array.BinarySearch(noteFrequencies, frequency);

            index = ~index;

            if (index > 0)
            {
                int noteFrequency = noteFrequencies[index - 1];
                previousLowerNote = new KeyValuePair<int, string>(noteFrequency, stringNotes[noteFrequency]);
            }

            if (index < noteFrequencies.Length)
            {
                int noteFrequency = noteFrequencies[index];
                nextHigherNote = new KeyValuePair<int, string>(noteFrequency, stringNotes[noteFrequency]);
            }
        }

        private bool IsCloserToLowerNote()
        {
            return System.Math.Abs(frequency - previousLowerNote.Key) < System.Math.Abs(frequency - nextHigherNote.Key);
        }

        private void FindPositions()
        {
            foreach (Dictionary<int, string> stringNotes in frequencyNotesByString)
            {
                if (!stringNotes.ContainsKey(TargetFrequency))
                {
                    continue;
                }

                Positions.Add(GetFretPosition(stringNotes));
            }
        }

        private string GetFretPosition(Dictionary<int, string> stringNotes)
        {            
            var frequencyFrets = stringNotes.Keys.ToArray();
            int fretNumber = Array.IndexOf(frequencyFrets, TargetFrequency);

            string stringName = stringNotes[stringNotes.Keys.First()];
         
            if (fretNumber > 0)
            {
                return string.Format("{0} String, Fret {1}",
                    stringName,
                    fretNumber.ToString());                
            }

            return string.Format("Open {0} string", stringName);
        }        

        private List<Dictionary<int, string>> GetFrequencyNotesByString()
        {
            Dictionary<int, string> lowEStringFrequencies = new Dictionary<int, string>
            {
                { 82, "E" },
                { 87, "F" },
                { 92, "F#" },
                { 98, "G" },
                { 104, "G#" },
                { 110, "A" },
                { 117, "A#" },
                { 123, "B" },
                { 131, "C" },
                { 139, "C#" },
                { 147, "D" },
                { 156, "D#" },
                { 165, "E" },
                { 175, "F" },
                { 185, "F#" },
                { 196, "G" },
                { 208, "G#" },
                { 220, "A" },
                { 233, "A#" },
                { 247, "B" },
                { 262, "C" },
            };

            Dictionary<int, string> aStringFrequencies = new Dictionary<int, string>
            {
                { 110, "A" },
                { 117, "A#" },
                { 123, "B" },
                { 131, "C" },
                { 139, "C#" },
                { 147, "D" },
                { 156, "D#" },
                { 165, "E" },
                { 175, "F" },
                { 185, "F#" },
                { 196, "G" },
                { 208, "G#" },
                { 220, "A" },
                { 233, "A#" },
                { 247, "B" },
                { 262, "C" },
                { 277, "C#" },
                { 294, "D" },
                { 311, "D#" },
                { 330, "E" },
                { 349, "F" },
            };

            Dictionary<int, string> dStringFrequencies = new Dictionary<int, string>
            {
                { 147, "D" },
                { 156, "D#" },
                { 165, "E" },
                { 175, "F" },
                { 185, "F#" },
                { 196, "G" },
                { 208, "G#" },
                { 220, "A" },
                { 233, "A#" },
                { 247, "B" },
                { 262, "C" },
                { 277, "C#" },
                { 294, "D" },
                { 311, "D#" },
                { 330, "E" },
                { 349, "F" },
                { 370, "F#" },
                { 392, "G" },
                { 415, "G#" },
                { 440, "A" },
                { 466, "A#" },
            };

            Dictionary<int, string> gStringFrequencies = new Dictionary<int, string>
            {
                { 196, "G" },
                { 208, "G#" },
                { 220, "A" },
                { 233, "A#" },
                { 247, "B" },
                { 262, "C" },
                { 277, "C#" },
                { 294, "D" },
                { 311, "D#" },
                { 330, "E" },
                { 349, "F" },
                { 370, "F#" },
                { 392, "G" },
                { 415, "G#" },
                { 440, "A" },
                { 466, "A#" },
                { 494, "B" },
                { 523, "C" },
                { 554, "C#" },
                { 587, "D" },
                { 622, "D#" },
            };

            Dictionary<int, string> bStringFrequencies = new Dictionary<int, string>
            {
                { 247, "B" },
                { 262, "C" },
                { 277, "C#" },
                { 294, "D" },
                { 311, "D#" },
                { 330, "E" },
                { 349, "F" },
                { 370, "F#" },
                { 392, "G" },
                { 415, "G#" },
                { 440, "A" },
                { 466, "A#" },
                { 494, "B" },
                { 523, "C" },
                { 554, "C#" },
                { 587, "D" },
                { 622, "D#" },
                { 659, "E" },
                { 698, "F" },
                { 740, "F#" },
                { 784, "G" },
            };

            Dictionary<int, string> highEStringFrequencies = new Dictionary<int, string>
            {
                { 330, "E" },
                { 349, "F" },
                { 370, "F#" },
                { 392, "G" },
                { 415, "G#" },
                { 440, "A" },
                { 466, "A#" },
                { 494, "B" },
                { 523, "C" },
                { 554, "C#" },
                { 587, "D" },
                { 622, "D#" },
                { 659, "E" },
                { 698, "F" },
                { 740, "F#" },
                { 784, "G" },
                { 831, "G#" },
                { 880, "A" },
                { 932, "A#" },
                { 988, "B" },
                { 1047, "C" },
            };

            return new List<Dictionary<int, string>>
            {
                lowEStringFrequencies,
                aStringFrequencies,
                dStringFrequencies,
                gStringFrequencies,
                bStringFrequencies,
                highEStringFrequencies
            };
        }
    }
}
