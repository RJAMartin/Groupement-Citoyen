using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Groupement_Citoyen.Models
{
    public class Utilisateur : IdentityUser
    {
        public Utilisateur() : base()
        {

        }
        public string Nom { get; set; }
        [DisplayName("Prénom")]
        public string Prenom { get; set; }
        [Range(0, 10000)]
        [DataType(DataType.Currency)]
        [DisplayName("Montant de votre portefeuille")]
        public decimal MontantPortefeuille { get; set; } = 0;
        public string Adresse { get; set; }
        public string NomComplet
        {
            get
            {
                return $"{this.Nom} {this.Prenom}";
            }
        }
        public List<Produit> Produits = new List<Produit>();
        public List<Commande> Commandes { get; set; } = new List<Commande>();
        public bool AjouterMontant(decimal montant)
        {
            if (montant <= 0) return false;
            MontantPortefeuille += montant;
            return true;
        }

        public void PayerCommande()
        {

            if (!Commandes.Last().Valider && (Commandes.Last().Total <= MontantPortefeuille))
            {
                MontantPortefeuille -= Commandes.Last().Total;
                foreach (DetailsCommande dc in Commandes.Last().DetailsCommandes)
                {
                    dc.Produit.Quantite -= dc.Quantite;
                }
                Commandes.Last().DateAchat = DateTime.Now;
                Commandes.Last().Valider = true;
                Commandes.Add(new Commande());
            }
        }
    }
}
