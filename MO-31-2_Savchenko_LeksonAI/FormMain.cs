using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using MO_31_2_Savchenko_LeksonAI.NeuroNet;

namespace MO_31_2_Savchenko_LeksonAI
{
    public partial class FormMain : Form
    {
        private double[] inputPixels; // хранение состояния пикселей (0 - белый, 1 - чёрный)
        private Network network;
        private NeuroNet.HiddenLayer hiddenlayer1;
        //Конструктор
        public FormMain()
        {
            InitializeComponent();

            inputPixels = new double[15];
            network = new Network();
        }

        //Обработчик кнопки
        private void change_btn_onClick(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == Color.White)      // если белый
            {
                ((Button)sender).BackColor = Color.Black;       // то меняем на чёрный
                inputPixels[((Button)sender).TabIndex] = 1d;    // флаг состояния
            }
            else // если чёрный
            {
                ((Button)sender).BackColor = Color.White;       // то меняем на белый
                inputPixels[((Button)sender).TabIndex] = 0d;    // флаг состояния
            }
        }

        private void button_SaveTrainSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "train.txt";
            string tmpStr = numericUpDown_NecessaryOutput.Value.ToString();

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n";

            File.AppendAllText(path, tmpStr);
        }

        private void button_SaveTestSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "test.txt";
            string tmpStr = numericUpDown_NecessaryOutput.Value.ToString();

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n";

            File.AppendAllText(path, tmpStr);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            hiddenlayer1 = new NeuroNet.HiddenLayer(10, 10, NeuroNet.NeuronType.Hidden, nameof(hiddenlayer1));
        }

        private void buttonRecognize_Click(object sender, EventArgs e)
        {
            network.ForwardPass(network, inputPixels);
            labelOut.Text = network.Fact.ToList().IndexOf(network.Fact.Max()).ToString();
            labelProbability.Text = (100 * network.Fact.Max()).ToString("0.00") + " %";

        }
    }
}