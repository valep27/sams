﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
namespace Team19.Model
{
    public abstract class MovimentoDiDenaro
    {
        private DateTime _data;
        private ISorgente _sorgente;
        private IDestinazione _destinazione;
        private Dipendente _dipendente;
        private DateTime _dataRegistrazione;
        private string _causale;

        protected MovimentoDiDenaro(ISorgente sorgente, IDestinazione destinazione, DateTime data, Dipendente dipendente, string causale)
        {
            if(Object.ReferenceEquals(sorgente, destinazione))
                throw new ArgumentException("sorgente e destinazione uguali!");

            _data = data.Date;
            _sorgente = sorgente;
            _destinazione = destinazione;
            _dataRegistrazione = DateTime.Now;
            _dipendente = dipendente;
            _causale = causale;
        }

        public DateTime Data
        {
            get { return _data; }
        }

        public abstract Currency Importo
        {
            get;
        }

        [Browsable(false)]
        public virtual ISorgente Sorgente
        {
            get { return _sorgente; }
        }

        [Browsable(false)]
        public virtual IDestinazione Destinazione
        {
            get { return _destinazione; }
        }

        [DisplayName("Sorgente")]
        public string Da
        {
            get { return Sorgente.ToString(); }
        }

        [DisplayName("Destinazione")]
        public string A
        {
            get { return Destinazione.ToString(); }
        }

        public Dipendente Dipendente
        {
            get { return _dipendente; }
        }

        public DateTime DataRegistrazione
        {
            get { return _dataRegistrazione; }
        }

        public string Causale
        {
            get { return _causale; }
        }

    }
}