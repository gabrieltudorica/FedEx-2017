using System;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using ScottPlot;

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

        private int ComputeArraySizeFor(int rawDataLength)
        {
            // convert it to int32 manually (and a double for scottplot)
            int sampleResolution = 16;
            int bytesPerPoint = sampleResolution / 8;

            return rawDataLength / bytesPerPoint;
        }

        private int[] GetPlottableDataFrom(byte[] rawData)
        {           
            int[] plottableData = new int[ComputeArraySizeFor(rawData.Length)];

            for (int i = 0; i < plottableData.Length; i++)
            {
                // bit shift the byte buffer into the right variable format
                byte hByte = rawData[i * 2 + 1];
                byte lByte = rawData[i * 2 + 0];

                plottableData[i] = (short)((hByte << 8) | lByte);               
            }

            return plottableData;
        }

        public void UpdateAudioGraph()
        {
            byte[] capturedData = recorder.GetCapturedData();
            if (capturedData.Length == 0) return;
            if (capturedData[fftBufferSize - 2] == 0) return;
            
            timer1.Enabled = false;

            int arraySize = ComputeArraySizeFor(capturedData.Length);

            double[] pulseCodeModulationXAxis = new double[arraySize];
            double[] pulseCodeModulationYAxis = new double[arraySize];
                        
            double[] fastFourierTransformXAxis = new double[arraySize];
            double[] fastFourierTransformYAxis = new double[arraySize];

            int[] plottableData = GetPlottableDataFrom(capturedData);
            for (int i=0; i < plottableData.Length; i++)
            {
                pulseCodeModulationXAxis[i] = i;
                pulseCodeModulationYAxis[i] = plottableData[i];
                fastFourierTransformXAxis[i] = (double)i/pulseCodeModulationYAxis.Length*soundCardSampleRate/1000.0; // units are in kHz
            }

            var fft = new FastFourierTransform(pulseCodeModulationYAxis);
            fastFourierTransformYAxis = fft.Get();

            UpdatePlot(pulseCodeModulationPlot, pulseCodeModulationXAxis, pulseCodeModulationYAxis);
            UpdatePlot(fastFourierTransformationPlot, fastFourierTransformXAxis.Take(fastFourierTransformXAxis.Length / 2).ToArray(), fastFourierTransformYAxis.Take(fastFourierTransformYAxis.Length / 2).ToArray());

            timer1.Enabled = true;
        }

        private void UpdatePlot(ScottPlotUC plot, double[] xValues, double[] yValues)
        {
            plot.Xs = xValues;
            plot.Ys = yValues;

            plot.UpdateGraph();
            plot.Update();
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
