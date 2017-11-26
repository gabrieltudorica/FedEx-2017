using System;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using System.Numerics;


namespace microphone
{
    public partial class Form1 : Form
    {
        public WaveIn wi;
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
            WaveIn recorder = new WaveIn();
            recorder.DeviceNumber = 0;
            recorder.WaveFormat = new NAudio.Wave.WaveFormat(soundCardSampleRate, 1);

            // create a wave buffer and start the recording
            recorder.DataAvailable += new EventHandler<WaveInEventArgs>(recorder_DataAvailable);
            bufferProvider = new BufferedWaveProvider(recorder.WaveFormat);
            bufferProvider.BufferLength = fftBuffer * 2;

            bufferProvider.DiscardOnBufferOverflow = true;
            recorder.StartRecording();

        }

        // adds data to the audio recording buffer
        void recorder_DataAvailable(object sender, WaveInEventArgs e)
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
            Ys2 = FFT(Ys);
            scottPlotUC2.Xs = Xs2.Take(Xs2.Length / 2).ToArray();
            scottPlotUC2.Ys = Ys2.Take(Ys2.Length / 2).ToArray();
            

            // update the displays
            scottPlotUC1.UpdateGraph();
            scottPlotUC2.UpdateGraph();

            scottPlotUC1.Update();
            scottPlotUC2.Update();
            
            timer1.Enabled = true;

        }

        public double[] FFT(double[] data)
        {
            double[] fft = new double[data.Length]; // this is where we will store the output (fft)
            Complex[] fftComplex = new Complex[data.Length]; // the FFT function requires complex format
            for (int i = 0; i < data.Length; i++)
            {
                fftComplex[i] = new Complex(data[i], 0.0); // make it complex format (imaginary = 0)
            }
            Accord.Math.FourierTransform.FFT(fftComplex, Accord.Math.FourierTransform.Direction.Forward);
            for (int i = 0; i < data.Length; i++)
            {
                fft[i] = fftComplex[i].Magnitude; // back to double
                //fft[i] = Math.Log10(fft[i]); // convert to dB
            }
            return fft;
            //todo: this could be much faster by reusing variables
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
