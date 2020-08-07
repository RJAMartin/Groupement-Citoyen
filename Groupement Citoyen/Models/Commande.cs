using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Groupement_Citoyen.Models
{
    public class Commande
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Date d'achat")]
        public DateTime DateAchat { get; set; } = DateTime.Now;
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }
        public List<DetailsCommande> DetailsCommandes { get; set; } = new List<DetailsCommande>() { };
        public bool Valider { get; set; } = false;
        public Utilisateur Utilisateur { get; set; }
        public void CalculerTotal()
        {
            Total = 0;
            foreach (DetailsCommande detailsCommande in DetailsCommandes)
            {
                Total += detailsCommande.Produit.Prix * detailsCommande.Quantite;
            }
        }

        public void AjouterProduit(Produit produit, int quantite)
        {
            if (produit.Quantite < quantite)
            {
                throw QuantiteInsuffisante();
            }
            DetailsCommandes.Add(new DetailsCommande(produit, quantite, this));
            CalculerTotal();
        }

        private Exception QuantiteInsuffisante()
        {
            throw new NotImplementedException();
        }
    }
}
