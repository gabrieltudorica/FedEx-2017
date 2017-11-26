using System;
using System.Collections.Generic;
using System.Linq;

namespace microphone
{
    public class Note
    {
        public string Name { get; private set; }
        public int TargetFrequency { get; private set; }

        private int frequency;
        private List<Dictionary<int, string>> frequencyNotesByString = new List<Dictionary<int, string>>();
        private KeyValuePair<int, string> previousLowerNote;
        private KeyValuePair<int, string> nextHigherNote;        
        
        public Note(int frequency)
        {
            this.frequency = frequency;
            Name = "N/A";
            TargetFrequency = 0;

            frequencyNotesByString = GetFrequencyNotesByString();
            FindName();
        }

        private void FindName()
        {
            foreach (Dictionary<int, string> stringNotes in frequencyNotesByString)
            {
                if (!IsFoundOnString(stringNotes))
                {
                    continue;                    
                }

                Name = GetClosestNoteIn(stringNotes);
            }           
        }

        private bool IsFoundOnString(Dictionary<int, string> stringNotes)
        {
            return frequency <= stringNotes.Keys.Last();
        }

        private string GetClosestNoteIn(Dictionary<int, string> stringNotes)
        {
            if (stringNotes.ContainsKey(frequency))
            {
                return stringNotes[frequency];
            }

            FindNeighboringNotesFrom(stringNotes);

            if (IsCloserToLowerNote())
            {
                return stringNotes[previousLowerNote.Key];
            }

            return stringNotes[nextHigherNote.Key];
        }

        private bool IsCloserToLowerNote()
        {
            return Math.Abs(frequency - previousLowerNote.Key) < Math.Abs(frequency - nextHigherNote.Key);
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

            int[] aStringFrequencies = new int[] { 110, 117, 123, 131, 139, 147, 156, 165, 175, 185, 196, 208, 220, 233, 247, 262, 277, 294, 311, 330, 349 };
            int[] dStringFrequencies = new int[] { 147, 156, 165, 175, 185, 196, 208, 220, 233, 247, 262, 277, 294, 311, 330, 349, 370, 392, 415, 440, 466 };
            int[] gStringFrequencies = new int[] { 196, 208, 220, 233, 247, 262, 277, 294, 311, 330, 349, 370, 392, 415, 440, 466, 494, 523, 554, 587, 622 };
            int[] bStringFrequencies = new int[] { 247, 262, 277, 294, 311, 330, 349, 370, 392, 415, 440, 466, 494, 523, 554, 587, 622, 659, 698, 740, 784 };
            int[] highEStringFrequencies = new int[] { 330, 349, 370, 392, 415, 440, 466, 494, 523, 554, 587, 622, 659, 698, 740, 784, 831, 880, 932, 988, 1047 };


            var frequencyNotesByString = new List<Dictionary<int, string>>();
            frequencyNotesByString.Add(lowEStringFrequencies);

            return frequencyNotesByString;
        }
    }
}
