using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Compi
{
    public partial class Compilador : Form
    {
        public Compilador()
        {
            InitializeComponent();
        }

        private void ejecutarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lexico = new Lexico(txtCodigoFuente.Text);
            lexico.EjecutarLexico();

            List<Error> listaErroresLexico = lexico.listaError;

            var lista = new BindingList<Token>(lexico.listaToken);
            dgvLexico.DataSource = null;
            dgvLexico.DataSource = lista;

            dgvErrores.DataSource = null;
            dgvErrores.DataSource = lexico.listaError;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void sintacticoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lexico = new Lexico(txtCodigoFuente.Text);
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            var sintactico = new Sintatico(lexico.listaError, lexico.EjecutarLexico());
            sintactico.ejecSintactico();
            /*
            for (int i = 0; i < sintactico.listaPolish.Count; i++)
            {
                comboBox1.Items.Add(sintactico.listaPolish.ElementAt<Token>(i).Lexema+ " " + sintactico.listaPolish.ElementAt<Token>(i).Lexema);
            }
            */
            for (int i = 0; i < sintactico.listaAuxPostfix.Count; i++)
            {
                comboBox2.Items.Add(sintactico.listaAuxPostfix.ElementAt(i)+" a");
            }
            for (int i = 0; i < sintactico.listaPostfix.Count; i++)
            {
                comboBox2.Items.Add(sintactico.listaPostfix.ElementAt(i) + " a");
            }
            dgvErrores.DataSource = null;
            dgvErrores.DataSource = sintactico.listaError;
            dvgPolish.DataSource = sintactico.listaPolish;
            txtEnsamblador.Text = sintactico.textoASMFinal;
            //EDITAR AQUI DIRECTORIO
            File.WriteAllText("C:\\Users\\Julian\\compi.asm", sintactico.textoASMFinal);
        }
    }
}
