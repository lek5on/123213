using MO_31_2_Savchenko_LeksonAI.NeuroNet;
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;


namespace MO_31_2_Savchenko_LeksonAI.NeuroNet
{
    abstract class Layer
    {
        //Поля
        protected string name_Layer; //название слоя
        string pathDirWeights; //путь к каталогу, где находится файл синаптических весов
        string pathFileWeights; //путь к файлу саниптическов весов
        protected int numofneurons; //число нейронов текущего слоя
        protected int numofprevneurons; //число нейронов предыдущего слоя
        protected const double learningrate = 0.060; //скорость обучения
        protected const double momentum = 0.050d; //момент инерции
        protected double[,] lastdeltaweights; //веса предыдущей итерации
        protected Neuron[] neurons; //массив нейронов текущего слоя

        //Свойства
        public Neuron[] Neurons { get => neurons; set => neurons = value; }
        public double[] Data //Передача входных сигналов на нейроны слоя и авктиватор
        {
            set
            {
                for (int i = 0; i < numofneurons; i++)
                {
                    Neurons[i].Activator(value);
                }
            }
        }

        //Конструктор
        protected Layer(int non, int nopn, NeuronType nt, string nm_Layer)
        {
            numofneurons = non; //количество нейронов текущего слоя
            numofprevneurons = nopn; //количество нейронов предыдущего слоя
            Neurons = new Neuron[non]; //определение массива нейронов
            name_Layer = nm_Layer; //наиминование слоя
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";

            double[,] Weights; //временный массив синаптических весов
            lastdeltaweights = new double[non, nopn + 1];

            if (File.Exists(pathFileWeights)) //определяет существует ли pathFileWeights
                Weights = WeightInitialize(MemoryMode.GET, pathFileWeights); //считывает данные из файла
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                Weights = WeightInitialize(MemoryMode.INIT, pathFileWeights);
            }

            for (int i = 0; i < non; i++) //цикл формирования нейронов слоя и заполнения
            {
                double[] tmp_weights = new double[nopn + 1];
                for (int j = 0; j < nopn; j++)
                {
                    tmp_weights[j] = Weights[i, j];
                }
                Neurons[i] = new Neuron(tmp_weights, nt); //заполнение массива нейронами
            }
        }

        //Метод работы с массивом синаптических весов слоя
        public double[,] WeightInitialize(MemoryMode mm, string path)
        {
            char[] delim = new char[] { ';', ' ' };
            string tmpStr;
            string[] tmpStrWeights;
            double[,] weights = new double[numofneurons, numofprevneurons + 1];

            switch (mm)
            {
                case MemoryMode.GET:
                    tmpStrWeights = File.ReadAllLines(path); //читаем все строки файла
                    string[] memory_element; //временный массив, хранящий веса одного нейрона в виде строк
                    for (int i = 0; i < numofneurons; i++)
                    {
                        memory_element = tmpStrWeights[i].Split(delim);
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            weights[i, j] = double.Parse(memory_element[j].Replace(',', '.'),
                                System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    break;
                    

                case MemoryMode.SET:
                    string[] tmpLines = new string[numofneurons];
                    for (int i = 0; i < numofneurons; i++)
                    {
                        string[] tmpRow = new string[numofprevneurons + 1];
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            tmpRow[j] = weights[i, j].ToString(System.Globalization.CultureInfo.InvariantCulture); //Преобразуем число в строку и записываем в row
                        }
                        tmpLines[i] = string.Join(";", tmpRow); //соединяем все елементы tmpRow и присваиваем в tmpLines для текущего нейрона
                    }
                    File.WriteAllLines(path, tmpLines); //запись в файл
                    break;


                case MemoryMode.INIT:
                    Random random = new Random();
                    for (int i = 0; i < numofneurons; i++)
                    {
                        double sum = 0.0;
                        double squaredsum = 0.0;
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            weights[i, j] = random.NextDouble()-1;
                            sum += weights[i, j];
                            squaredsum += weights[i, j] * weights[i, j];
                        }
                        double mean = sum / (numofneurons + 1);

                        double variance = (squaredsum / (numofprevneurons + 1)) - (mean * mean);
                        double root = Math.Sqrt(variance);
                    }
                    WeightInitialize(MemoryMode.SET, path);
                    break;
            }
            return weights;
        }

    }
}
