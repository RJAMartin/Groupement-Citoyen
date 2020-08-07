using Groupement_Citoyen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Groupement_Citoyen.Controllers
{
    [Authorize(Roles ="Utilisateur")]
    public class DetailsCommandesController : Controller
    {
        private readonly GroupementCitoyenDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;
        public DetailsCommandesController(GroupementCitoyenDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DetailsCommandes
        public async Task<IActionResult> Index()
        {
            return View(await _context.DetailsCommandes.Include(dc => dc.Produit).ToListAsync());
        }

        // GET: DetailsCommandes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailsCommande = await _context.DetailsCommandes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detailsCommande == null)
            {
                return NotFound();
            }

            return View(detailsCommande);
        }

        // GET: DetailsCommandes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DetailsCommandes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PrixUnitaire,Quantite")] DetailsCommande detailsCommande)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detailsCommande);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(detailsCommande);
        }

        // GET: DetailsCommandes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailsCommande = await _context.DetailsCommandes.Include(dc => dc.Commande).Where(dc => dc.Id == id).FirstAsync();
            if (detailsCommande == null)
            {
                return NotFound();
            }
            return View(detailsCommande);
        }

        // POST: DetailsCommandes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PrixUnitaire,Quantite")] DetailsCommande detailsCommande, int idCommande)
        {
            if (id != detailsCommande.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detailsCommande);
                    _context.SaveChangesAsync().Wait();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetailsCommandeExists(detailsCommande.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Commandes", new { id = idCommande });
            }
            return View(detailsCommande);
        }

        // GET: DetailsCommandes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailsCommande = await _context.DetailsCommandes.Include(dc => dc.Commande).Where(dc => dc.Id == id).FirstAsync();

            if (detailsCommande == null)
            {
                return NotFound();
            }

            return View(detailsCommande);
        }

        // POST: DetailsCommandes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int idCommande)
        {
            var detailsCommande = await _context.DetailsCommandes.FindAsync(id);
            _context.DetailsCommandes.Remove(detailsCommande);
            await _context.SaveChangesAsync();
            string idUtilisateur = _userManager.GetUserId(HttpContext.User);
            var utilisateur = await _context.Utilisateurs.Include(u => u.Commandes).ThenInclude(c => c.DetailsCommandes).ThenInclude(dc => dc.Produit).Where(u => u.Id.Equals(idUtilisateur)).FirstAsync();
            utilisateur.Commandes.Last().CalculerTotal();
            _context.SaveChangesAsync().Wait();
            return RedirectToAction("Details", "Commandes", new { id = idCommande });
        }

        private bool DetailsCommandeExists(int id)
        {
            return _context.DetailsCommandes.Any(e => e.Id == id);
        }
    }
}
