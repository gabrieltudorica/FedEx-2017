using System.Numerics;

namespace microphone
{
    class FastFourierTransform
    {
        private double[] data;

        public FastFourierTransform(double[] data)
        {
            this.data = data;
        }

        public double[] Get()
        {            
            Complex[] fftComplex = ConvertToComplex();
            Accord.Math.FourierTransform.FFT(fftComplex, Accord.Math.FourierTransform.Direction.Forward);
            return ConvertToDouble(fftComplex);          
        }

        private Complex[] ConvertToComplex()
        {
            Complex[] fftComplex = new Complex[data.Length]; // the FFT function requires complex format
            for (int i = 0; i < data.Length; i++)
            {
                fftComplex[i] = new Complex(data[i], 0.0); // make it complex format (imaginary = 0)
            }

            return fftComplex;
        }

        private double[] ConvertToDouble(Complex[] complexData)
        {
            double[] fft = new double[data.Length]; // this is where we will store the output (fft)

            for (int i = 0; i < data.Length; i++)
            {
                fft[i] = complexData[i].Magnitude; // back to double
                //fft[i] = Math.Log10(fft[i]); // convert to dB
            }

            return fft;
        }
    }
}
