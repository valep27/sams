﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Team19.Model;
namespace Team19.Presentation
{
    public partial class ElementDataView : UserControl
    {
        //Istanza privata contenente il DataSource della DataGridView
        //usata per filtrarne il contenuto
        private object _dataSource;
        public ElementDataView()
        {
            InitializeComponent();
            _subItemsCombo.SelectionChangeCommitted += FiltraElenco;
        }

        private void FiltraElenco(object sender, EventArgs e)
        {
            Type tipo = (Type)_subItemsCombo.SelectedItem;
            IList<object> elencoFiltrato = new List<object>();
            foreach (object obj in (IEnumerable<object>)_dataSource)
            {
                if (obj.GetType().Equals(tipo))
                    elencoFiltrato.Add(obj);
            }
            _dataGridView.DataSource = elencoFiltrato;
        }

        public DataGridViewSelectedRowCollection SelectedRows
        {
            get { return _dataGridView.SelectedRows; }
        }

        public object DataSource
        {
            get { return _dataSource; }
            set
            {
                _subItemsCombo.Enabled = true;
                _dataSource = value;
                _dataGridView.DataSource = _dataSource;
                if (_dataSource != null)
                {
                    _subItemsCombo.Items.Clear();
                    _subItemsCombo.Text = "";
                    Type mainclass;
                    Type contenitore = _dataSource.GetType();

                    if (contenitore.BaseType.GetGenericArguments().Count() != 0)
                    {
                        mainclass = _dataSource.GetType().BaseType.GetGenericArguments()[0];

                        //Trova le sottoclassi del tipo da visualizzare
                        //e le inserisce nella combobox
                        if (mainclass.IsAbstract)
                        {
                            foreach (Type type in Assembly.GetAssembly(mainclass).GetTypes())
                            {
                                if (type.IsSubclassOf(mainclass) && !type.IsAbstract)
                                {
                                    _subItemsCombo.Items.Add(type);
                                }
                            }
                            _subItemsCombo.DisplayMember = "Name";
                        }
                    }
                    if (_subItemsCombo.Items.Count == 0)
                        _subItemsCombo.Enabled = false;             
                }
            }
        }
        public Type DataType
        {
            get
            {
                if (_dataSource.GetType().BaseType.GetGenericArguments().Count() != 0)
                    return _dataSource.GetType().BaseType.GetGenericArguments()[0];
                else
                    return _dataSource.GetType();
            }
        }
    }
}
