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
    public class CommandesController : Controller
    {
        private readonly GroupementCitoyenDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;

        public CommandesController(GroupementCitoyenDbContext context, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Commandes
        public async Task<IActionResult> Index()
        {
            string idUtilisateur = _userManager.GetUserId(HttpContext.User);
            ViewBag.userId = idUtilisateur;
            Utilisateur utilisateur = await _context.Utilisateurs.Where(u => u.Id.Equals(idUtilisateur)).Include(u => u.Commandes).ThenInclude(c => c.DetailsCommandes).ThenInclude(dc => dc.Produit).FirstAsync();
            if (utilisateur.Commandes.Count == 0)
            {
                utilisateur.Commandes.Add(new Commande());
                _context.Add(utilisateur.Commandes.Last());
                await _context.SaveChangesAsync();

            }
            utilisateur.Commandes.Last().CalculerTotal();
            _context.Update(utilisateur.Commandes.Last());
            await _context.SaveChangesAsync();
            return View(utilisateur);
        }

        // GET: Commandes/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var commande = await _context.Commandes
                 .Include(c => c.DetailsCommandes)
                 .ThenInclude(dc => dc.Produit)
                 .ThenInclude(p => p.Producteur)
                 .FirstOrDefaultAsync(m => m.Id == id);

            commande.CalculerTotal();
            _context.Update(commande);
            await _context.SaveChangesAsync();

            if (commande == null)
            {
                return NotFound();
            }

            return View(commande);
        }

        // GET: Commandes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Commandes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DateAchat,Total")] Commande commande)
        {
            if (ModelState.IsValid)
            {
                _context.Add(commande);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(commande);
        }

        // POST: Commandes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValiderCommande()
        {
            string idUtilisateur = _userManager.GetUserId(HttpContext.User);
            Utilisateur u = await _context.Utilisateurs.Where(u => u.Id.Equals(idUtilisateur))
                .Include(u => u.Commandes)
                .ThenInclude(c => c.DetailsCommandes)
                .ThenInclude(dc => dc.Produit)
                .FirstAsync();
            u.PayerCommande();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Commandes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjouterProduit(int quantite, int idProduit)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string idUtilisateur = _userManager.GetUserId(HttpContext.User);
                    var produit = await _context.Produits
                .FirstOrDefaultAsync(m => m.Id == idProduit);
                    var commande = await _context.Commandes.Where(c => c.Utilisateur.Id.Equals(idUtilisateur) && !c.Valider).Include(dc => dc.DetailsCommandes).ThenInclude(dc => dc.Produit).FirstAsync();
                    commande.AjouterProduit(produit, quantite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Commandes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = await _context.Commandes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commande == null)
            {
                return NotFound();
            }

            return View(commande);
        }

        // POST: Commandes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commande = await _context.Commandes.FindAsync(id);
            _context.Commandes.Remove(commande);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommandeExists(int id)
        {
            return _context.Commandes.Any(e => e.Id == id);
        }
    }
}
