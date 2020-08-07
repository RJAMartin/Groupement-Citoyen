using Groupement_Citoyen.Models;
using Groupement_Citoyen.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Groupement_Citoyen.Controllers
{
    [Authorize(Roles ="Responsable, Utilisateur")]
    public class UtilisateursController : Controller
    {
        private readonly GroupementCitoyenDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UtilisateursController(GroupementCitoyenDbContext context, UserManager<Utilisateur> userManager, RoleManager<IdentityRole> roleManager, GroupementCitoyenDbContext groupementCitoyenDbContext)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Producteurs
        public async Task<IActionResult> Index()
        {
            var users = await _context.Utilisateurs.ToListAsync();
            List<UserRoleVM> result = new List<UserRoleVM>();
            foreach (Utilisateur u in users)
            {
                UserRoleVM userRole = new UserRoleVM();
                userRole.role = _userManager.GetRolesAsync(u).Result.First();
                if (userRole.role.Contains("Producteur"))
                {
                    userRole.utilisateur = u;
                    result.Add(userRole);
                }

            }
            return View(result);
        }


        // GET: Producteurs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producteurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom,Prenom, Adresse, Email")] Utilisateur utilisateur, string motDePasse)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByEmailAsync(utilisateur.Email).Result;
                if (user == null)
                {
                    _userManager.CreateAsync(utilisateur, motDePasse).Wait();
                    _userManager.AddToRoleAsync(utilisateur, "Producteur").Wait();
                    _context.Add(utilisateur);
                    _context.SaveChangesAsync().Wait();

                }
                return RedirectToAction(nameof(Index));
            }
            return View(utilisateur);
        }

        // GET: Producteur/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producteur = await _context.Utilisateurs.FindAsync(id);
            if (producteur == null)
            {
                return NotFound();
            }

            return View(producteur);
        }

        // POST: DetailsCommandes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Utilisateur producteur = await _context.Utilisateurs.Where(u => u.Id.Equals(id)).FirstAsync();
            foreach (Produit produit in producteur.Produits)
            {
                _context.Remove(produit);
            }
            _context.Remove(producteur);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: AjouterMontant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjouterMontant(decimal montant)
        {
            string idUtilisateur = _userManager.GetUserId(HttpContext.User);
            var utilisateur = await _userManager.FindByIdAsync(idUtilisateur);
            utilisateur.AjouterMontant(montant);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Commandes");
        }
    }
}
