using Groupement_Citoyen.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace Groupement_Citoyen.Data
{
    public static class DataInitializer
    {
        public static void SeedData(UserManager<Utilisateur> userManager, RoleManager<IdentityRole> roleManager, GroupementCitoyenDbContext groupementCitoyenDbContext)
        {
            SeedRoleAsync(roleManager);
            SeedUser(userManager, groupementCitoyenDbContext);
        }
        public static void SeedRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole admin = new IdentityRole() { Name = "Responsable" };
                roleManager.CreateAsync(admin).Wait();
            }

            if (!roleManager.RoleExistsAsync("Producteur").Result)
            {
                IdentityRole admin = new IdentityRole() { Name = "Producteur" };
                roleManager.CreateAsync(admin).Wait();
            }

            if (!roleManager.RoleExistsAsync("Utilisateur").Result)
            {
                IdentityRole admin = new IdentityRole() { Name = "Utilisateur" };
                roleManager.CreateAsync(admin).Wait();
            }
        }
        public static void SeedUser(UserManager<Utilisateur> userManager, GroupementCitoyenDbContext groupementCitoyenDbContext)
        {
            //Responsable
            var user = userManager.FindByEmailAsync("m.ney@gacci.com").Result;
            if (user == null)
            {
                Utilisateur user1 = new Utilisateur()
                {
                    UserName = "m.ney@gacci.com",
                    Nom = "Ney",
                    Prenom = "Mo",
                    Email = "m.ney@gacci.com"

                };
                userManager.CreateAsync(user1, "Renaud3011!").Wait();
                userManager.AddToRoleAsync(user1, "Responsable").Wait();
                groupementCitoyenDbContext.SaveChangesAsync().Wait();
            }
            //Membres
            user = userManager.FindByEmailAsync("p.achere@hotmail.com").Result;
            if (user == null)
            {
                Utilisateur user1 = new Utilisateur()
                {
                    UserName = "p.achere@hotmail.com",
                    Nom = "Achère",
                    Prenom = "Pierre",
                    Email = "p.achere@hotmail.com",

                };
                userManager.CreateAsync(user1, "Renaud3011!").Wait();
                userManager.AddToRoleAsync(user1, "Utilisateur").Wait();
                groupementCitoyenDbContext.SaveChangesAsync().Wait();
            }
            user = userManager.FindByEmailAsync("r.adin@gmail.com").Result;
            if (user == null)
            {
                Utilisateur user1 = new Utilisateur()
                {
                    UserName = "r.adin@gmail.com",
                    Nom = "Adin",
                    Prenom = "Raphaelle",
                    Email = "r.adin@gmail.com"

                };
                userManager.CreateAsync(user1, "Renaud3011!").Wait();
                userManager.AddToRoleAsync(user1, "Utilisateur").Wait();
                groupementCitoyenDbContext.SaveChangesAsync().Wait();

            }
            //Producteurs
            user = userManager.FindByEmailAsync("b.braham@gmail.com").Result;
            if (user == null)
            {
                Utilisateur user1 = new Utilisateur()
                {
                    UserName = "b.braham@gmail.com",
                    Nom = "Boucherie",
                    Prenom = "Braham",
                    Email = "b.braham@gmail.com",
                    Adresse = "Bèfve 57, 5550 Clérister"
                };
                user1.Produits.Add(new Produit()
                {
                    Nom = "Barquettes de saucisses porc et boeuf",
                    Prix = 6.60M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Barquettes de saucisses de poulet",
                    Prix = 7.20M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Côte d'agneau",
                    Prix = 1.50M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Charcuteries",
                    Description = "Plat de charcuteries 2 personnes",
                    Prix = 16M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Barquettes de haché porc et boeuf",
                    Prix = 6.50M,
                    Quantite = new Random().Next(5, 15)
                });
                userManager.CreateAsync(user1, "Renaud3011!").Wait();
                userManager.AddToRoleAsync(user1, "Producteur").Wait();
                foreach (Produit produit in user1.Produits)
                {
                    produit.Producteur = user1;
                    groupementCitoyenDbContext.Add(produit);
                    groupementCitoyenDbContext.SaveChangesAsync().Wait();
                }


            }
            user = userManager.FindByEmailAsync("b.collin@gmail.com").Result;
            if (user == null)
            {
                Utilisateur user1 = new Utilisateur()
                {
                    UserName = "b.collin@gmail.com",
                    Nom = "Boulangerie",
                    Prenom = "Collin",
                    Email = "b.collin@gmail.com",
                    Adresse = "Centre 38, 5550 Clérister"
                };
                user1.Produits.Add(new Produit()
                {
                    Nom = "Pain Blanc",
                    Description = "500g",
                    Prix = 2.35M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Pain Gris",
                    Description = "500g",
                    Prix = 2.40M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Farine bio",
                    Description = "1kg",
                    Prix = 1.10M,
                    Quantite = new Random().Next(5, 15)
                });
                userManager.CreateAsync(user1, "Renaud3011!").Wait();
                userManager.AddToRoleAsync(user1, "Producteur").Wait();
                foreach (Produit produit in user1.Produits)
                {
                    produit.Producteur = user1;
                    groupementCitoyenDbContext.Add(produit);
                    groupementCitoyenDbContext.SaveChangesAsync().Wait();
                }

            }
            user = userManager.FindByEmailAsync("m.ahalo@gmail.com").Result;
            if (user == null)
            {
                Utilisateur user1 = new Utilisateur()
                {
                    UserName = "m.ahalo@gmail.com",
                    Nom = "Mahalo",
                    Email = "m.ahalo@gmail.com",
                    Adresse = "Rue de l'Université 35, 5000 Thierel"
                };
                user1.Produits.Add(new Produit()
                {
                    Nom = "Fromage de chèvre",
                    Description = "100g",
                    Prix = 1.00M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Yaourt blanc",
                    Description = "1pot",
                    Prix = 0.85M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Lait demi-écrémé",
                    Description = "1l",
                    Prix = 1.30M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Oeuf Bio",
                    Description = "par 6",
                    Prix = 2.20M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Beurre salé",
                    Description = "250g",
                    Prix = 2.20M,
                    Quantite = new Random().Next(5, 15)
                });
                userManager.CreateAsync(user1, "Renaud3011!").Wait();
                userManager.AddToRoleAsync(user1, "Producteur").Wait();
                foreach (Produit produit in user1.Produits)
                {
                    produit.Producteur = user1;
                    groupementCitoyenDbContext.Add(produit);
                    groupementCitoyenDbContext.SaveChangesAsync().Wait();
                }

            }
            user = userManager.FindByEmailAsync("j.ibkenne@gmail.com").Result;
            if (user == null)
            {
                Utilisateur user1 = new Utilisateur()
                {
                    UserName = "j.ibkenne@gmail.com",
                    Nom = "Jobkenne & Fils Sprl",
                    Email = "j.ibkenne@gmail.com",
                    Adresse = "La Forge 8, 4890 Clérister"
                };
                user1.Produits.Add(new Produit()
                {
                    Nom = "Poudre à lessiver",
                    Description = "2kg",
                    Prix = 16.00M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Adoucissant liquide",
                    Description = "1.5l",
                    Prix = 8.60M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Tablette vaisselle",
                    Description = "par 20",
                    Prix = 15.20M,
                    Quantite = new Random().Next(5, 15)
                });
                userManager.CreateAsync(user1, "Renaud3011!").Wait();
                userManager.AddToRoleAsync(user1, "Producteur").Wait();
                foreach (Produit produit in user1.Produits)
                {
                    produit.Producteur = user1;
                    groupementCitoyenDbContext.Add(produit);
                    groupementCitoyenDbContext.SaveChangesAsync().Wait();
                }

            }
            user = userManager.FindByEmailAsync("m.dutronc@gmail.com").Result;
            if (user == null)
            {
                Utilisateur user1 = new Utilisateur()
                {
                    UserName = "m.dutronc@gmail.com",
                    Nom = "Magasin Dutronc",
                    Email = "m.dutronc@gmail.com",
                    Adresse = "Avenue de Navagne 34, 4600 Chaudemine"
                };
                user1.Produits.Add(new Produit()
                {
                    Nom = "Café moulu",
                    Description = "250g",
                    Prix = 6.30M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Bière locale",
                    Description = "33cl",
                    Prix = 1.80M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Miel d'acacias bio",
                    Description = "300g",
                    Prix = 6.00M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Pâtes complètes",
                    Description = "500g",
                    Prix = 2.10M,
                    Quantite = new Random().Next(5, 15)
                });
                user1.Produits.Add(new Produit()
                {
                    Nom = "Riz long grain",
                    Description = "400G",
                    Prix = 2.50M,
                    Quantite = new Random().Next(5, 15)
                });
                userManager.CreateAsync(user1, "Renaud3011!").Wait();
                userManager.AddToRoleAsync(user1, "Producteur").Wait();
                foreach (Produit produit in user1.Produits)
                {
                    produit.Producteur = user1;
                    groupementCitoyenDbContext.Add(produit);
                    groupementCitoyenDbContext.SaveChangesAsync().Wait();
                }
            }
        }


    }
}
