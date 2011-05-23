﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team19.Model
{
    public abstract class Soggetto
    {
        private string _denominazione;
        private string _telefono;
        private string _eMail;
        private string _partitaIva;
        private Indirizzo _indirizzo;

        protected Soggetto(string denominazione, string telefono, string email, string partitaIva, Indirizzo indirizzo)
        {
            _denominazione = denominazione;
            _telefono = telefono;
            _eMail = email;
            _partitaIva = partitaIva;
            _indirizzo = indirizzo;
        }

        public Indirizzo Indirizzo
        {
            get { return _indirizzo; }
            //set { _indirizzo = value; }
        }

        public string Telefono
        {
            get { return _telefono; }
            //set { _telefono = value; }
        }

        public string EMail
        {
            get { return _eMail; }
            //set { _eMail = value; }
        }

        public string Denominazione
        {
            get { return _denominazione; }
            //set { _denominazione = value; }
        }

        public string PartitaIva
        {
            get { return _partitaIva; }
            //set { _partitaIva = value; }
        }
    }
}
