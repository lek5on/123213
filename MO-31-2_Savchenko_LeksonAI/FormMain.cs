using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MO_31_2_Savchenko_LeksonAI
{
    public partial class FormMain : Form
    {
        private double[] inputPixels; // хранение состояния пикселей (0 - белый, 1 - чёрный)

        //Конструктор
        public FormMain()
        {
            InitializeComponent();

            inputPixels = new double[15];
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
    }
}