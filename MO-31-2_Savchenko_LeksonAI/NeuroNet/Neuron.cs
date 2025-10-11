using static System.Math;

namespace MO_31_2_Savchenko_LeksonAI.NeuroNet
{
    class Neuron
    {
        // поля
        private NeuronType type;    // тип нейрона
        private double[] weights;   // веса
        private double[] inputs;    // входные данные
        private double output;      // выходные данные
        private double derivative;  // производная

        // константа для функции активации
        private double a = 1.0d;

        // свойства
        public double[] Weights { get => weights; set => weights = value; }
        public double[] Inputs { get => inputs; set => inputs = value; }
        public double Output { get => output; }
        public double Derivative { get => derivative; }

        // конструктор
        public Neuron(double[] memoryWeights, NeuronType typeNeuron)
        {
            type = typeNeuron;
            weights = memoryWeights;
        }

        public void Activator(double[] i)
        {
            inputs = i; // передача вектора входного сигнала в массив входных данных нейрона
            double sum = weights[0];  // аффиное преобразование через смещение
            for (int j = 0; j < inputs.Length; j++) // цикл вычисления индуцированного поля нейрона
                sum += inputs[j] * weights[j + 1];  // линейные преобразования входных сигналов

            switch (type)
            {
                case NeuronType.Hidden:  // для нейронов скрытого слоя
                    output = Logistick(sum);
                    derivative = LogistickDerivative(output);
                    break;
                case NeuronType.Output:  // для нейронов выходного слоя
                    output = Logistick(sum);
                    derivative = LogistickDerivative(output);
                    break;
            }
        }

        // функция активации нейрона (логистическая с коэффициентом a)
        private double Logistick(double x)
        {
            return 1.0d / (1.0d + Exp(-a * x));
        }

        // производная функции активации
        private double LogistickDerivative(double sigmoidOutput)
        {
            return a * sigmoidOutput * (1.0d - sigmoidOutput);
        }
    }
}
