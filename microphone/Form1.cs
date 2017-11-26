using System;
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

            recorder = new Recorder(recordingDeviceIndex, soundCardSampleRate, channelCount, fftBufferSize);
            recorder.StartCapturing();            
        }
             
        public void UpdateCharts()
        {
            byte[] capturedData = recorder.GetCapturedData();

            if (capturedData.Length == 0) return;
            if (capturedData[fftBufferSize - 2] == 0) return;
            
            timer1.Enabled = false;

            var realtimeTransformationChart = new RealtimeTransformationChart(capturedData, soundCardSampleRate);
            pulseCodeModulationChart.Xs = realtimeTransformationChart.PulseModulationChart.Xs;
            pulseCodeModulationChart.Ys = realtimeTransformationChart.PulseModulationChart.Ys;
            pulseCodeModulationChart.UpdateGraph();

            fastFourierTransformationChart.Xs = realtimeTransformationChart.FastFourierTransformChart.Xs;
            fastFourierTransformationChart.Ys = realtimeTransformationChart.FastFourierTransformChart.Ys;
            fastFourierTransformationChart.UpdateGraph();
            
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
