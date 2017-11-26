using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace microphone
{
    public partial class Form1 : Form
    {
        private int soundCardSampleRate = 44100;
        private int fftBufferSize = (int)Math.Pow(2, 13); // must be a multiple of 2
        private Recorder recorder;

        public Form1()
        {
            InitializeComponent();
            int recordingDeviceIndex = 0;
            int channelCount = 1;
            var note = new Note(418);
            recorder = new Recorder(recordingDeviceIndex, soundCardSampleRate, channelCount, fftBufferSize);
            recorder.StartCapturing();            
        }   
             
        public void UpdateCharts()
        {
            byte[] capturedData = recorder.GetCapturedData();

            if (capturedData.Length == 0) return;
            if (capturedData[fftBufferSize - 2] == 0) return;
            
            timer1.Enabled = false;

            var realtimeTransformation = new RealtimeTransformation(capturedData, soundCardSampleRate);
            pulseCodeModulationChart.Xs = realtimeTransformation.PulseModulationChart.Xs;
            pulseCodeModulationChart.Ys = realtimeTransformation.PulseModulationChart.Ys;
            pulseCodeModulationChart.UpdateGraph();

            fastFourierTransformationChart.Xs = realtimeTransformation.FastFourierTransformChart.Xs;
            fastFourierTransformationChart.Ys = realtimeTransformation.FastFourierTransformChart.Ys;
            fastFourierTransformationChart.UpdateGraph();

            frequencyLbl.Text = string.Format("Frequency: {0} Hz", realtimeTransformation.Frequency.ToString("#.##"));

            var note = new Note(Convert.ToInt16(realtimeTransformation.Frequency));
            
            timer1.Enabled = true;
        }      

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateCharts();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateCharts();
        }
    }
}
