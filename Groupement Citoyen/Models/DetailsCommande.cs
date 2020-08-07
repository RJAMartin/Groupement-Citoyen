using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Groupement_Citoyen.Models
{
    public class DetailsCommande
    {
        public DetailsCommande()
        {

        }
        public DetailsCommande(Produit produit, int quantite, Commande commande)
        {
            PrixUnitaire = produit.Prix;
            Quantite = quantite;
            Produit = produit;
            Commande = commande;
        }
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Currency)]
        [DisplayName("Prix Unitaire")]
        public decimal PrixUnitaire { get; set; }
        [Range(0, 100)]
        [DisplayName("Quantité")]
        public int Quantite { get; set; }
        public Produit Produit { get; set; }
        public Commande Commande { get; set; }


    }
}
