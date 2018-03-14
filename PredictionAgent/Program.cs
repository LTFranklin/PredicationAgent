using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PredictionAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            double[] weights = new double[3];
            weights[0] = 0.3000001;
            weights[1] = -0.2000001;
            weights[2] = 0.1000001;
            double threshhold = 0.2;
            double learningRate = 0.1;
            double prediction = 0;
            double errTot;

            double[] input = LoadInput();
            for (int j = 0; j < 1000; ++j)
            {
                errTot = 0;
                for (int i = 0; i < input.Length - 3; ++i)
                {
                    prediction = DoWork(input, weights, threshhold, i);
                    double sig = CalcSig(prediction);

                    errTot += input[i] - prediction;
                    weights[0] = AdjustWeights(input[i], input[i] - prediction, weights[0], sig, learningRate);
                    weights[1] = AdjustWeights(input[i], input[i] - prediction, weights[1], sig, learningRate);
                    weights[2] = AdjustWeights(input[i], input[i] - prediction, weights[2], sig, learningRate);
                }
                errTot = errTot / input.Length;
                Console.Write("Prediction: {0}, Actual: {1}, Average Error: {2}\n", prediction.ToString(), input[input.Length-3].ToString(), errTot.ToString());
            }

        }

        static double[] LoadInput()
        {
            double[] arr = new double[2000];
            StreamReader read = new StreamReader("input.txt");
            for(int i = 0; i < 2000; ++i)
            {
                arr[i] = double.Parse(read.ReadLine());
            }
            return arr;
        }


        static double DoWork(double[] x, double[] w, double threshold, int pos)
        {
            double total = 0;

            for(int i = 0; i < 1; ++i)
            {
                total += (x[i + pos] * w[i]);
            }

            return total;
        }

        static double CalcSig (double prediction)
        {
            double ex = Math.Pow(4.95E-10, -prediction);
            double sig = Math.Pow(ex + 1, -1);

            return sig;
        }

        static double AdjustWeights(double input, double error, double w, double sig, double learningRate)
        {
           return w + (error * sig);
        }
    }
}
