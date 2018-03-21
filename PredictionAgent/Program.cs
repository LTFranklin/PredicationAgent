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
            //declare variables
            Random rand = new Random();
            double[] weights = { 0, 1, 2 };// rand.NextDouble(), rand.NextDouble(), rand.NextDouble() };

            //should be random? not yet implemented
            double threshhold = 0;
            //other vars
            double learningRate = 0.4;
            double prediction = 0;
            double errTot;
            double prev = 0 ;
            bool x = false;
            //used for output
            StreamWriter write = new StreamWriter("output.txt");
            StreamWriter write2 = new StreamWriter("error.txt");

            double[] input = LoadInput();
            for (int j = 0; j < 2000; ++j)
            {
                errTot = 0;
                for (int i = 200; i < 3502; ++i)
                {
                    prediction = DoWork(input, weights, threshhold, i);
                    double sig = CalcSig(prediction);
                    double err = input[i] - prediction;
                    errTot += input[i] - prediction;
                    weights[0] = AdjustWeights(input[i-2], input[i] - prediction, weights[0], sig, learningRate);
                    weights[1] = AdjustWeights(input[i-1], input[i] - prediction, weights[1], sig, learningRate);
                    weights[2] = AdjustWeights(input[i], input[i] - prediction, weights[2], sig, learningRate);
                    if(x)
                    {
                        Console.WriteLine(weights[0]);
                    }
                }
                errTot = errTot / 3502;

                if (prev != 0 && Math.Abs(prev) - Math.Abs(errTot) < 0)
                {
                    //x = true;
                }

                prev = errTot;
                write2.WriteLine(errTot);
                //Console.WriteLine(errTot);
                if(-0.001 < errTot && errTot < 0.001)
                {
                    //break;
                }
                //Console.Write("Prediction: {0}, Actual: {1}, Average Error: {2}\n", prediction.ToString(), input[input.Length-3].ToString(), errTot.ToString());
            }

            for (int i = 2; i < input.Length - 2; ++i)
            {
                write.WriteLine(DoWork(input, weights, threshhold, i));
            }
            write.Close();
            write2.Close();
        }

        //gets the input data
        static double[] LoadInput()
        {
            //sets the first 2 vlaues to zero and goes through the file to get the rest of the values
            double[] arr = new double[3502];
            arr[0] = 0;
            arr[1] = 0;
            StreamReader read = new StreamReader("input.txt");
            for (int i = 2; i < 3502; ++i)
            {
                arr[i] = double.Parse(read.ReadLine());
            }
            return arr;
        }

        //predicts the next value
        static double DoWork(double[] x, double[] w, double threshold, int pos)
        {
            //sets a total to zero
            double total = 0;

            //loops through a number of values and adds them to the total after multiplying them by the weights
            for (int i = 0; i < 3; ++i)
            {
                total += (x[pos - i] * w[i]);
            }

            //add the threshold and divide by the number of values used -> is this right?
            return (total - threshold) / 3;
        }

        //calculates the sigmoid
        static double CalcSig(double prediction)
        {
            //
            double ex = Math.Pow(4.95E-10, -prediction);
            double sig = Math.Pow(ex + 1, -1);

            return sig;
        }

        static double AdjustWeights(double input, double error, double w, double sig, double learningRate)
        {
            return w + (error * sig * learningRate);
        }
    }
}
