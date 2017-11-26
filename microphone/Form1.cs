using System;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;


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

        public void UpdateAudioGraph()
        {
            byte[] capturedData = recorder.GetCapturedData();
            if (capturedData.Length == 0) return;
            if (capturedData[fftBufferSize - 2] == 0) return;
            
            timer1.Enabled = false;

            // convert it to int32 manually (and a double for scottplot)
            int SAMPLE_RESOLUTION = 16;
            int BYTES_PER_POINT = SAMPLE_RESOLUTION / 8;
            Int32[] vals = new Int32[capturedData.Length/BYTES_PER_POINT];
            double[] Ys = new double[capturedData.Length / BYTES_PER_POINT];
            double[] Xs = new double[capturedData.Length / BYTES_PER_POINT];
            double[] Ys2 = new double[capturedData.Length / BYTES_PER_POINT];
            double[] Xs2 = new double[capturedData.Length / BYTES_PER_POINT];
            for (int i=0; i<vals.Length; i++)
            {
                // bit shift the byte buffer into the right variable format
                byte hByte = capturedData[i * 2 + 1];
                byte lByte = capturedData[i * 2 + 0];
                vals[i] = (int)(short)((hByte << 8) | lByte);
                Xs[i] = i;
                Ys[i] = vals[i];
                Xs2[i] = (double)i/Ys.Length*soundCardSampleRate/1000.0; // units are in kHz
            }

            // update scottplot (PCM, time domain)
            scottPlotUC1.Xs = Xs;
            scottPlotUC1.Ys = Ys;

            //update scottplot (FFT, frequency domain)
            var fft = new FastFourierTransform(Ys);
            Ys2 = fft.Get();
            scottPlotUC2.Xs = Xs2.Take(Xs2.Length / 2).ToArray();
            scottPlotUC2.Ys = Ys2.Take(Ys2.Length / 2).ToArray();
            

            // update the displays
            scottPlotUC1.UpdateGraph();
            scottPlotUC2.UpdateGraph();

            scottPlotUC1.Update();
            scottPlotUC2.Update();
            
            timer1.Enabled = true;

        }       

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateAudioGraph();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateAudioGraph();
        }
    }
}
