using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Groupement_Citoyen.Models
{
    public class Produit
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nom { get; set; }
        [Range(0, 100)]
        [Required]
        [DisplayName("Quantité")]
        public int Quantite { get; set; } = 0;
        [Range(0.01, 1000.00)]
        [Required]
        [DataType(DataType.Currency)]
        public decimal Prix { get; set; } = 0;
        public string Description { get; set; }
        public bool Visible { get; set; } = true;
        public Utilisateur Producteur { get; set; }
    }
}
