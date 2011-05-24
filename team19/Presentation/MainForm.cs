﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Team19.Model;

namespace Team19.Presentation
{
    public partial class MainForm : Form
    {
        private Document _document;

        public MainForm()
        {
            InitializeComponent();
            _document = Document.GetInstance();
           moneyGrid.DataSource = _document.ContenitoriDiDenaro;
           this.dgdipendenti.DataSource = _document.Dipendenti;
  //         UpdateDocument();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //MessageBox.Show((new Currency(10.30m)).ToString());

            
            

        }
        private void UpdateDocument()
        {
            DataTable d = new DataTable();

            d.Columns.Add("Tipo di deposito");
            d.Columns.Add("Codice");
            d.Columns.Add("Saldo iniziale");
            d.Columns.Add("Saldo corrente");
            Cassa cassa = _document.Cassa;
            List<string> rowValues = new List<string>();
            rowValues.Add(cassa.GetType().Name);
            rowValues.Add("");
            rowValues.Add(cassa.SaldoIniziale.ToString());
            rowValues.Add(cassa.Saldo.ToString());
            d.Rows.Add(rowValues.ToArray());
            foreach (ContenitoreDiDenaro c in _document.ContenitoriDiDenaro)
            {
                rowValues = new List<string>();
                rowValues.Add(c.GetType().Name);
                if (c is ContoCorrenteBancario) rowValues.Add(((ContoCorrenteBancario)c).CodConto);
                else rowValues.Add("");
                rowValues.Add(c.SaldoIniziale.ToString());
                 rowValues.Add(c.Saldo.ToString());
                d.Rows.Add(rowValues.ToArray());

            }

            moneyGrid.DataSource = d;
        }

        private void dgdipendenti_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgdipendenti_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow dv = dgdipendenti.CurrentRow;
            txtnome.Text = dv.Cells["nome"].Value.ToString();
            txtcognome.Text = dv.Cells["cognome"].Value.ToString();
            txtuser.Text = dv.Cells["username"].Value.ToString();
            txtpassword.Text = dv.Cells["password"].Value.ToString();
            cmbruolo.Text = dv.Cells["ruolo"].Value.ToString();
        }
    }
}
