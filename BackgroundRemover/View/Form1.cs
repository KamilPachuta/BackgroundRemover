using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BackgroundRemover.Model;

namespace BackgroundRemover.View
{
    public partial class Form1 : Form
    {
        private Model1 model;
        private Color usersColor;
        

        /**
         * Form1 konstruktor - Wywoluje metode InitializeComponent(),
         * ustawia widocznosc przycisku execute na false
         * ustawia poczatkowy kolor wymazania
         * ustawia poczatkowa wartosc radioButton
         * ustawia poczatkowa ilosc watkow 
         */
        internal Form1()
        {
            InitializeComponent();
            button_Execute.Visible = false;
            usersColor = Color.Green;
            radioButton1.Checked = true;
            trackBar1.Value = Environment.ProcessorCount;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        /**
         * button_Load_Click metoda - otwiera okno FileDialog,
         * próba przypisania wybranego przez uzytkownika pliku do bitmapy, w razie wypadku obsluga bledu poprzez wyswietlenie komuniktu
         * 
         */
        private void button_Load_Click(object sender, EventArgs e)
        {
            
            try
            {
                openFileDialog1.ShowDialog(this);
                Bitmap newImage = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = newImage;
                pictureBox1.Refresh();
                model = new Model1(newImage);
                button_Execute.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        /**
         * button_Execute_Click metoda - Odpowiada za wywołanie poprawne wywolanie metody modelu przetwazajacej obraz.
         * Po wykonaniu obliczen, pokazany zostaje czas oraz ustawiany jest wyjaciowy obraz.  
         */
        private void button_Execute_Click(object sender, EventArgs e)
        { 
            (Bitmap newImage, double time) = model.calculate_Image(colorDialog1.Color, trackBar1.Value , radioButton1.Checked);
            label1.Text = $"{time.ToString()} ms.";
            pictureBox2.Image = newImage;
            pictureBox2.Refresh();
        }

        /**
         * button_Color_Click metoda - Pobiera kolor od uzywtkoanika, zawiera kontrole bledow
         */
        private void button_Color_Click(object sender, EventArgs e)
        {
           
            try
            {
                colorDialog1.ShowDialog(this);
                usersColor = colorDialog1.Color;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
