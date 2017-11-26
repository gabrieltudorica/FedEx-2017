using NAudio.Wave;
using System;

namespace Music
{
    public class Recorder
    {
        private BufferedWaveProvider bufferProvider;
        private WaveIn recorderDevice;
        private int bufferLength;

        public Recorder(int deviceIndex, int sampleRate, int channelCount, int bufferLength)
        {
            this.bufferLength = bufferLength;

            recorderDevice = new WaveIn
            {
                DeviceNumber = deviceIndex,
                WaveFormat = new WaveFormat(sampleRate, channelCount)
            };

            recorderDevice.DataAvailable += new EventHandler<WaveInEventArgs>(Recorder_DataAvailable);

            bufferProvider = new BufferedWaveProvider(recorderDevice.WaveFormat)
            {
                BufferLength = bufferLength * 2,
                DiscardOnBufferOverflow = true
            };
        }      

        public void StartCapturing()
        {
            recorderDevice.StartRecording();
        }

        public void StopCapturing()
        {
            recorderDevice.StopRecording();
        }

        public byte[] GetCapturedData()
        {
            var capturedData = new byte[bufferLength];
            bufferProvider.Read(capturedData, 0, bufferLength);

            return capturedData;
        }

        private void Recorder_DataAvailable(object sender, WaveInEventArgs e)
        {
            bufferProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }
    }
}
