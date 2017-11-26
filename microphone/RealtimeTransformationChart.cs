using System.Linq;
using NAudio.Wave;
using ScottPlot;
using System;

namespace microphone
{
    public class RealtimeTransformation
    {
        public ScottPlotUC PulseModulationChart { get; private set; }
        public ScottPlotUC FastFourierTransformChart { get; private set; }
        public int Frequency { get; private set; }
       
        private byte[] rawData;
        private int sampleRate;
        private int arraySize;
        private int[] plottableData;               

        public RealtimeTransformation(byte[] rawData, int sampleRate)
        {
            this.rawData = rawData;
            this.sampleRate = sampleRate;
            arraySize = ComputeArraySizeFor(rawData.Length);

            ComputeCharts();
            LoadFrequencyData();
        }        

        private int ComputeArraySizeFor(int rawDataLength)
        {
            int sampleResolution = 16;
            int bytesPerPoint = sampleResolution / 8;

            return rawDataLength / bytesPerPoint;
        }

        private void ComputeCharts()
        {
            SetPlottableDataFrom(rawData);
            LoadPulseModulationChart();
            LoadFastFourierTransformationChart();
        }

        private int[] SetPlottableDataFrom(byte[] rawData)
        {
            plottableData = new int[arraySize];

            for (int i = 0; i < plottableData.Length; i++)
            {
                // bit shift the byte buffer into the right variable format
                byte hByte = rawData[i * 2 + 1];
                byte lByte = rawData[i * 2 + 0];

                plottableData[i] = (short)((hByte << 8) | lByte);
            }

            return plottableData;
        }
      
        private void LoadPulseModulationChart()
        {           
            double[] xAxisValues = new double[arraySize];
            double[] yAxisValues = new double[arraySize];

            for (int i = 0; i < plottableData.Length; i++)
            {
                xAxisValues[i] = i;
                yAxisValues[i] = plottableData[i];
            }

            PulseModulationChart = new ScottPlotUC
            {
                Xs = xAxisValues,
                Ys = yAxisValues
            };
        }

        private void LoadFastFourierTransformationChart()
        {
            double[] xAxisValues = new double[arraySize];
            double[] yAxisValues = new double[arraySize];

            for (int i = 0; i < plottableData.Length; i++)
            {
                xAxisValues[i] = (double)i / plottableData.Length * sampleRate / 1000.0; // units are in kHz;
            }

            var fft = new FastFourierTransform(PulseModulationChart.Ys);
            yAxisValues = fft.Get();           

            FastFourierTransformChart = new ScottPlotUC
            {
                Xs = xAxisValues.Take(xAxisValues.Length / 2).ToArray(),
                Ys = yAxisValues.Take(yAxisValues.Length / 2).ToArray()
            };
        }

        private void LoadFrequencyData()
        {
            var amplitudes = FastFourierTransformChart.Ys.ToList();
            int dominantAmplitudeIndex = amplitudes.IndexOf(amplitudes.Max());
            double dominantFrequencyInKhz = FastFourierTransformChart.Xs[dominantAmplitudeIndex];

            Frequency = Convert.ToInt32(dominantFrequencyInKhz * 1000);
        }
    }
}
