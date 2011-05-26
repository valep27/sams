﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Team19.Persistence;

namespace Team19.Model
{
    class Document
    {
        private List<MovimentoDiDenaro> _movimenti;
        private List<ContenitoreDiDenaro> _contenitoriDiDenaro;
        private List<Fattura> _fatture;
        private List<Prodotto> _prodotti; //meglio un dictionary?
        private List<ISoggetto> _soggetti;
        private List<Dipendente> _dipendenti;
        private Cassa _cassa;

        private static Document _instance;
        private IDocumentPersister _persister;
        private Dipendente _utenteConnesso;
        public static event EventHandler Changed;

        private Document(IDocumentPersister persister)
        {
            this._persister = persister;
        }


        #region Document Adders

        public void Add(MovimentoDiDenaro movimento)
        {
            _movimenti.Add(movimento);
            OnChanged();
        }

        public void Add(Fattura fattura)
        {
            _fatture.Add(fattura);
            OnChanged();
        }

        public void Add(ISoggetto soggetto)
        {
            _soggetti.Add(soggetto);
            OnChanged();
        }

        public void Add(ContenitoreDiDenaro contenitore)
        {
            _contenitoriDiDenaro.Add(contenitore);
            OnChanged();
        }

        public void Add(Prodotto prodotto)
        {
            _prodotti.Add(prodotto);
            OnChanged();
        }

        public void Add(Dipendente dipendente)
        {
            IsAmministratore();
            _dipendenti.Add(dipendente);
            OnChanged();
        }

        #endregion

        #region Document Removers

        public void Remove(MovimentoDiDenaro movimento)
        {
            _movimenti.Remove(movimento);
            OnChanged();
        }

        public void Remove(Fattura fattura)
        {
            _fatture.Remove(fattura);
            OnChanged();
        }

        public void Remove(ISoggetto soggetto)
        {
            _soggetti.Remove(soggetto);
            OnChanged();
        }

        public void Remove(ContenitoreDiDenaro contenitore)
        {
            _contenitoriDiDenaro.Remove(contenitore);
            OnChanged();
        }

        public void Remove(Prodotto prodotto)
        {
            _prodotti.Remove(prodotto);
            OnChanged();
        }

        public void Remove(Dipendente dipendente)
        {
            IsAmministratore();
            _dipendenti.Remove(dipendente);
            OnChanged();
        }

        #endregion

        #region Document properties

        public Dipendente UtenteConnesso
        {
            get { return _utenteConnesso; }
            set { _utenteConnesso = value; }
        }

        public IEnumerable<Dipendente> Dipendenti
        {
            get { return _dipendenti; }

        }

        public IEnumerable<MovimentoDiDenaro> Movimenti
        {
            get { return _movimenti; }
        }

        public IEnumerable<ContenitoreDiDenaro> ContenitoriDiDenaro
        {
            get { return _contenitoriDiDenaro; }
        }

        public Cassa Cassa
        {
            get { return _cassa; }
        }

        public IEnumerable<Fattura> Fatture
        {
            get { return _fatture; }
        }

        public IEnumerable<Prodotto> Prodotti
        {
            get { return _prodotti; }
        }

        public IEnumerable<ISoggetto> Soggetti
        {
            get { return _soggetti; }
        }

        #endregion

        #region Document members

        public static void CreateInstance(IDocumentPersister persister)
        {
            _instance = new Document(persister);
            _instance.Load();
        }

        public static Document GetInstance()
        {
            if (_instance == null)
                CreateInstance(new DefaultPersister());
            if (_instance.UtenteConnesso == null) throw new ApplicationException("Nessun utente connesso");
            return _instance;
        }

        private void IsAmministratore()
        {
            if (!UtenteConnesso.Ruolo.Equals(TipoDipendente.Amministratore))
                throw new InvalidOperationException("Non hai privilegi di amministratore");
        }

        public static void Autentica(string username, string password)
        {
            IEnumerable<Dipendente> dipendenti = from dipendente in _instance._dipendenti
                where dipendente.Username.Equals(username) && dipendente.Password.Equals(password)
                select dipendente;

             Dipendente d = null;

            if(dipendenti.Count() != 0)
               d = dipendenti.First();

            if (d == null) throw new KeyNotFoundException("Username o password non corrispondenti");
            _instance._utenteConnesso = d;

        }

        private void Load()
        {
            IDocumentLoader loader = _persister.GetLoader();
            _prodotti = loader.LoadProdotti();
            _dipendenti = loader.LoadDipendenti();
            _cassa = loader.LoadCassa();
            _contenitoriDiDenaro = loader.LoadContenitori();
            _soggetti = loader.LoadSoggetti();
            _fatture = loader.LoadFatture();
            _movimenti = loader.LoadMovimenti();
            OnChanged();
        }

        public void Save()
        {
            //questo metodo non fa nulla
        }

        public IEnumerable<FatturaVendita> GetFattureVendita()
        {
            return this.Fatture.OfType<FatturaVendita>();
        }

        public IEnumerable<FatturaAcquisto> GetFattureAcquisto()
        {
            return this.Fatture.OfType<FatturaAcquisto>();
        }

        public IEnumerable<MovimentoDiDenaro> GetPagamentiAcquisti()
        {
            return this.Movimenti.Where(movimento => movimento.Destinazione is FatturaAcquisto);
        }

        public IEnumerable<MovimentoDiDenaro> GetIncassiVendite()
        {
            return this.Movimenti.Where(movimento => movimento.Sorgente is FatturaVendita);
        }

        #endregion

        protected virtual void OnChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }
    }
}
