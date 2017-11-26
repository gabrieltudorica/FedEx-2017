using System;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;


namespace microphone
{
    public partial class Form1 : Form
    {
        public BufferedWaveProvider bufferProvider;

        private int soundCardSampleRate = 44100;
        private int fftBuffer = (int) Math.Pow(2,13); // must be a multiple of 2

        public Form1()
        {
            InitializeComponent();

            // see what audio devices are available
            int devcount = WaveIn.DeviceCount;
            Console.Out.WriteLine("Device Count: {0}.", devcount);           

            // get the WaveIn class started
            WaveIn recorder = new WaveIn
            {
                DeviceNumber = 0,
                WaveFormat = new WaveFormat(soundCardSampleRate, 1)
            };          
            recorder.DataAvailable += new EventHandler<WaveInEventArgs>(Recorder_DataAvailable);
            bufferProvider = new BufferedWaveProvider(recorder.WaveFormat)
            {
                BufferLength = fftBuffer * 2,
                DiscardOnBufferOverflow = true
            };

            recorder.StartRecording();
        }

        void Recorder_DataAvailable(object sender, WaveInEventArgs e)
        {
            bufferProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        public void UpdateAudioGraph()
        {
            // read the bytes from the stream
            int frameSize = fftBuffer;
            var frames = new byte[frameSize];
            bufferProvider.Read(frames, 0, frameSize);
            if (frames.Length == 0) return;
            if (frames[frameSize-2] == 0) return;
            
            timer1.Enabled = false;

            // convert it to int32 manually (and a double for scottplot)
            int SAMPLE_RESOLUTION = 16;
            int BYTES_PER_POINT = SAMPLE_RESOLUTION / 8;
            Int32[] vals = new Int32[frames.Length/BYTES_PER_POINT];
            double[] Ys = new double[frames.Length / BYTES_PER_POINT];
            double[] Xs = new double[frames.Length / BYTES_PER_POINT];
            double[] Ys2 = new double[frames.Length / BYTES_PER_POINT];
            double[] Xs2 = new double[frames.Length / BYTES_PER_POINT];
            for (int i=0; i<vals.Length; i++)
            {
                // bit shift the byte buffer into the right variable format
                byte hByte = frames[i * 2 + 1];
                byte lByte = frames[i * 2 + 0];
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
