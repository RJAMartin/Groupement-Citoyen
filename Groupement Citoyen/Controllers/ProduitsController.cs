using Groupement_Citoyen.Models;
using Groupement_Citoyen.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Groupement_Citoyen.Controllers
{
    [Authorize(Roles ="Utilisateur, Producteur")]
    public class ProduitsController : Controller
    {
        private readonly GroupementCitoyenDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;
        public ProduitsController(GroupementCitoyenDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Produits
        public async Task<IActionResult> Index(string idProducteur)
        {
            string idUtilisateur = _userManager.GetUserId(HttpContext.User);
            Utilisateur utilisateur = await _userManager.FindByIdAsync(idUtilisateur);
            var roles = await _userManager.GetRolesAsync(utilisateur);
            if (roles.First().Contains("Producteur"))
            {
                return View(await _context.Produits.Include(p => p.Producteur).Where(p => p.Producteur.Id.Equals(idUtilisateur)).ToListAsync());
            }
            ViewBag.userId = idUtilisateur;
            if (idProducteur != null)
                return View(await _context.Produits.Include(p => p.Producteur).Where(p => p.Producteur.Id.Equals(idProducteur) && p.Visible).ToListAsync());
            return View(await _context.Produits.Include(p => p.Producteur).Where(p => p.Visible).ToListAsync());
        }

        // GET: Produits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // GET: Produits/Create
        public async Task<IActionResult> Create()
        {
            ProduitCreationVM vm = new ProduitCreationVM()
            {
                Produit = new Produit(),
                Producteur = await _context.Utilisateurs.FirstOrDefaultAsync()
            };
            return View(vm);
        }

        // POST: Produits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom,Quantite,Prix,Description,Visible")] Produit produit)
        {
            string idProduteur = _userManager.GetUserId(HttpContext.User);
            produit.Producteur = await _context.Utilisateurs.Where(u => u.Id.Equals(idProduteur)).FirstAsync();
            if (ModelState.IsValid)
            {
                _context.Add(produit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produit);
        }

        // GET: Produits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }
            return View(produit);
        }

        // POST: Produits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Quantite,Prix,Description,Visible")] Produit produit)
        {
            if (id != produit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduitExists(produit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produit);
        }

        // GET: Produits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // POST: Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            _context.Produits.Remove(produit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduitExists(int id)
        {
            return _context.Produits.Any(e => e.Id == id);
        }
    }
}
