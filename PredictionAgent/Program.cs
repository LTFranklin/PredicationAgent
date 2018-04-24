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
            double[] input = LoadInput();
            double[] weights = { rand.NextDouble(), rand.NextDouble(), rand.NextDouble(), 0 };
            double learningRate = 0.1;
            double[] predictions = new double[input.Count() - 3];

            StreamWriter write = new StreamWriter("output.txt");
            double cost = 1;

            while (cost > 0.0000000000001)
            {

                for (int i = 0; i < input.Count() - 3; ++i)
                {
                    //get the sum of XW
                    double sum = (weights[0] * input[i]) + (weights[1] * input[i + 1]) + (weights[2] * input[i + 2]) + weights[3];
                    //get the sig (output)
                    double sig = Activate(sum);
                    //calculate the cost
                    cost = 0.5 * Math.Pow((input[i + 3] - sig), 2);
                    //adjust weights (w + LR * Error * x * s'(x))
                    weights[0] = weights[0] + learningRate * (input[i + 3] - sig) * input[i] * sig * (1 - sig);
                    weights[1] = weights[1] + learningRate * (input[i + 3] - sig) * input[i + 1] * sig * (1 - sig);
                    weights[2] = weights[2] + learningRate * (input[i + 3] - sig) * input[i + 2] * sig * (1 - sig);
                    weights[3] = weights[3] + learningRate * (input[i + 3] - sig) * sig * (1 - sig);
                }
                Console.WriteLine(cost);
            }

            //write out the predictions with no weight adjustment
            for (int i = 0; i < input.Count() - 4; ++i)
            {
                predictions[i] = Activate((weights[0] * input[i + 2]) + (weights[1] * input[i + 3]) + (weights[2] * input[i + 4]) + weights[3]);
                write.WriteLine(predictions[i]);
            }

            write.Close();
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

        static double Activate(double sum)
        {
            //perceptron output
            /*if(sum > 0.5)
            {
                return 1;
            }
            else
            {
                return 0;
            }*/

            double ex = Math.Pow(Math.E, -sum);
            double sig = Math.Pow(ex + 1, -1);

            return sig;

        }
    }
}
