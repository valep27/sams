﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Team19.Model;
using Team19.Persistence;
using System.Reflection;

namespace Team19.Presentation
{
    public partial class MainForm : Form
    {

        private readonly Controller _controller;
        public MainForm()
        {
            InitializeComponent();
            _controller = new Controller(_documentListView, _dataGridView,_utenteConnessoMenuItem);
            aggiungiToolStripMenuItem.Click += _controller.CreaElemento;
            rimuoviToolStripMenuItem.Click += _controller.EliminaElemento;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _controller.Autentica();
        }

        private void riepilogoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_dataGridView.DataSource.GetType().BaseType.GetGenericArguments().First().Equals(typeof(Soggetto)))
            {
                _controller.MostraRiepilogo();
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controller.Autentica();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            using (InfoForm info = new InfoForm())
            {
                info.ShowDialog();
            }
        }
        

    }
}
