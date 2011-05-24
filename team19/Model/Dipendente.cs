﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace Team19.Model
{
    public class Dipendente 
    {
        private string _username;
        private string _password;
        private string _cognome;
        private string _nome;
        //_ruolo--> indica il ruolo del dipendente
        //se non può cambiare runtime--> readonly
        private string _ruolo;
       
        
        public Dipendente(string username, string password, string nome, string cognome,string ruolo)
        {
            this._username = username;
            this._password = password;
            this._cognome = cognome;
            this._nome = nome;
            this._ruolo = ruolo;//A Amministratore o U  utente
        }
        public string Nome
        {
            get { return _nome; }
        }
        public string Cognome
        {
            get { return _cognome; }
        }
        public string Username
        {
            get { return _username; }
        }
        public string Password
        {
            get { return _password; }
        }
       
        public string Ruolo
        {
            get { return _ruolo; }
        }   
    }
}
