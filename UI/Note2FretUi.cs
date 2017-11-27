using Music;
using System;
using System.Windows.Forms;

namespace UI
{
    public partial class Note2FretUi : Form
    {       
        private int soundCardSampleRate = 44100;
        private int fftBufferSize = (int)Math.Pow(2, 13); // must be a multiple of 2
        private Recorder recorder;
        private RealtimeTransformation realtimeTransformation;

        public Note2FretUi()
        {
            InitializeComponent();

            timer.Interval = 10;

            int recordingDeviceIndex = 0;
            int channelCount = 1;            
            recorder = new Recorder(recordingDeviceIndex, soundCardSampleRate, channelCount, fftBufferSize);
            recorder.StartCapturing();            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateUi();
            timer.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateUi();
        }

        public void UpdateUi()
        {            
            byte[] capturedData = recorder.GetCapturedData();            
            if (IsEmpty(capturedData))
            {
                return;
            }

            timer.Enabled = false;

            realtimeTransformation = new RealtimeTransformation(capturedData, soundCardSampleRate);
            LoadGraphs(capturedData);
            LoadNoteDetails();

            timer.Enabled = true;
        }

        private bool IsEmpty(byte[] rawData)
        {
            return rawData.Length == 0 || rawData[fftBufferSize - 2] == 0;
        }

        private void LoadGraphs(byte[] rawData)
        {
            realtimeTransformation = new RealtimeTransformation(rawData, soundCardSampleRate);
            pulseCodeModulationChart.Xs = realtimeTransformation.PulseModulationChart.Xs;
            pulseCodeModulationChart.Ys = realtimeTransformation.PulseModulationChart.Ys;
            pulseCodeModulationChart.UpdateGraph();

            fastFourierTransformationChart.Xs = realtimeTransformation.FastFourierTransformChart.Xs;
            fastFourierTransformationChart.Ys = realtimeTransformation.FastFourierTransformChart.Ys;
            fastFourierTransformationChart.UpdateGraph();
        }

        private void LoadNoteDetails()
        {
            frequencyLbl.Text = string.Format("Frequency: {0} Hz", realtimeTransformation.Frequency.ToString("#.##"));
            var note = new Note(Convert.ToInt16(realtimeTransformation.Frequency));

            noteLbl.Text = string.Format("Note: {0} ({1} Hz)", note.Name, note.TargetFrequency);

            string positions = "Positions: ";
            foreach (string position in note.Positions)
            {
                positions += Environment.NewLine + position;
            }

            positionsLbl.Text = positions;
        }        
    }
}
