using System;
using System.IO;
using System.Windows.Forms;


namespace MO_31_2_Savchenko_LeksonAI.NeuroNet
{
     abstract class Layer
    {
        //поля
        protected string name_Layer; //наименование слоя
        string pathDirWeights; // путь к каталогу где находится файл
        string pathFileWeights; //путь к файлу синаптических весов 
        protected int numofneurons; //число нейронов тек слоя
        protected int numofprevneurons; //число нейронов предыдущего слоя
        protected const double learningrate = 0.060; //скорость обучения
        protected const double momentum = 0.050d; //момент инерции
        protected double[,] lastdeltaweights; //веса пред итерации
        protected Neuron[] neurons; //массив нейронов 

        //свойства 
        public Neuron[] Neurons { get => neurons; set => neurons = value; }
        public double[] Data //передача входных данных на нейроны слоя
        {
            set
            {
                for( int i=0;i<numofneurons;i++)
                {
                    Neurons[i].Activator(value);
                }
            }
        }

        protected Layer(int non, int nopn, NeuronType nt, string nm_Layer)
        {
            int i, j;
            numofneurons = non;
            numofprevneurons = nopn;
            Neurons = new Neuron[non];
            name_Layer = nm_Layer;
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";
            lastdeltaweights = new double[non, nopn + 1];
            double[,] Weights;

            if (File.Exists(pathFileWeights))
                Weights = WeightInitialize(MemoryMode.INIT, pathFileWeights);
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                Weights = WeightInitialize(MemoryMode.INIT, pathFileWeights);
            }
            for(i =0;i<non;i++)
            {
                double[] tmp_weights = new double[nopn + 1];
                for(j=0;j<nopn+1;j++)
                {
                    tmp_weights[j] = Weights[i, j];
                }
                Neurons[i] = new Neuron(tmp_weights, nt);
            }
        }
        
        public double[,] WeightInitialize(MemoryMode mm, string path)
        {
            int i, j;
            char[] delim = new char[] { ';', ' ' };
            string tmpStr;
            string[] tmpStrWeights;
            double[,] weights = new double[numofneurons, numofprevneurons + 1];
        }
    }
}
