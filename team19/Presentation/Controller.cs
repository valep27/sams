﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Team19.Model;
using Team19.Persistence;
using System.Reflection;
namespace Team19.Presentation
{
    class Controller
    {
        private Document _document;
        private readonly DocumentListView _documentListView;
        private readonly ElementDataView _dataGridView;
        private readonly ToolStripMenuItem _userMenuItem;
        public Controller(DocumentListView documentListView, ElementDataView dataGridView, ToolStripMenuItem userMenuItem)
        {
            _documentListView = documentListView;
            _dataGridView = dataGridView;
            _userMenuItem = userMenuItem;
            _documentListView.SelectionChanged += AggiornaTabella;
            Document.CreateInstance(new DefaultPersister());
            _document = Document.GetInstance();
        }

        public void Autentica()
        {
            try
            {
                using (AuthenticationForm auth = new AuthenticationForm())
                {
                    if (auth.ShowDialog() == DialogResult.OK)
                    {
                        _document.Autentica(auth.Username, auth.Password);
                        _dataGridView.DataSource = _document.Movimenti;
                        _userMenuItem.Text = _document.UtenteConnesso.ToString();
                    }
                    else Application.Exit();
                }
            }
            catch (KeyNotFoundException kexc)
            {
                //Riesegue l'autenticazione finchè l'utente non effettua il login correttamente o annulla l'operazione (chiudendo l'applicazione)
                MessageBox.Show(kexc.Message);
                Autentica();
            }
        }

        private void AggiornaTabella(object sender, EventArgs e)
        {
            if (_documentListView.SelectedItem != null)
            {
                _dataGridView.DataSource = _documentListView.SelectedItem.Tag;
                _dataGridView.Refresh();
            }
        }

        public void MostraRiepilogo()
        {
            Soggetto soggetto = null;
            if (_dataGridView.SelectedRows.Count == 1)
            {
                soggetto = _document.Soggetti.ElementAt(_dataGridView.SelectedRows[0].Index);
            }
            using (FormRiepilogo formRiepilogo = new FormRiepilogo(soggetto))
            {
                formRiepilogo.ShowDialog();
            }
        }

        public void CreaElemento(object sender, EventArgs e)
        {
            try
            {
                using (InsertForm form = new InsertForm(_dataGridView.DataType))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {   //Aggiunta di un oggetto ai contentitori
                        //recupero la property del document relativa al tipo creato
                        PropertyInfo property = _document.GetType().GetProperties().Where(prop => prop.PropertyType.GetGenericArguments().Contains(_dataGridView.DataType)).First();
                        //recupero il metodo Add(object) della property
                        MethodInfo add = property.GetValue(_document, null).GetType().GetMethod("Add", new Type[] { typeof(object) });
                        //invoco il metodo Add
                        add.Invoke(property.GetValue(_document, null), new object[1] { form.ElementoCreato });
                        _dataGridView.DataSource = property.GetValue(_document, null);
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EliminaElemento(object sender, EventArgs e)
        {

            if ((MessageBox.Show("Sei sicuro?", "Eliminazione", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)).Equals(DialogResult.OK) && _dataGridView.SelectedRows.Count == 1)
            {
                int index = _dataGridView.SelectedRows[0].Index;
                Type dataType = _dataGridView.DataType;
                _dataGridView.DataSource = null;
                
                try
                {
                    //Rimozione di un oggetto ai contentitori
                    //recupero la property del document relativa al tipo creato
                    PropertyInfo property = _document.GetType().GetProperties().Where(prop => prop.PropertyType.GetGenericArguments().Contains(dataType)).First();
                    //recupero il metodo Delete(int) della property
                    MethodInfo delete = property.GetValue(_document, null).GetType().GetMethod("Delete", new Type[] { typeof(int) });
                    //invoco il metodo Delete
                    delete.Invoke(property.GetValue(_document, null), new object[1] { index });
                    _dataGridView.DataSource = property.GetValue(_document, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Impossibile rimuovere l'elemento selezionato", "Errore");
                }
            }
        }
    }
}
