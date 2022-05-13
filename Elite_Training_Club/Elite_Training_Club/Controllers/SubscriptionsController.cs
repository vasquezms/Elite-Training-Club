using Elite_Training_Club.Data;
using Elite_Training_Club.Data.Entities;
using Elite_Training_Club.Helpers;
using Elite_Training_Club.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elite_Training_Club.Controllers
{

    [Authorize(Roles = "Admin")]

    public class SubscriptionsController : Controller

    {

        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;

        public SubscriptionsController(DataContext context, ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Subscriptions
                .Include(p => p.SubscriptionsPlans)
                .ThenInclude(pc => pc.Plan)
                .ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            CreateSubscriptionsViewModel model = new()
            {
                Plans = await _combosHelper.GetComboPlansAsync(),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSubscriptionsViewModel model)
        {
            if (ModelState.IsValid)
            {

                Subscriptions subscriptions = new()
                {
                    Description = model.Description,
                    Name = model.Name,
                    Price = model.Price,
                };

                subscriptions.SubscriptionsPlans = new List<SubscriptionsPlan>()
                    {
                        new SubscriptionsPlan
                        {
                            Plan = await _context.Plans.FindAsync(model.PlanId)
                        }
                    };
                try
                {
                    _context.Add(subscriptions);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una suscripción con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            model.Plans = await _combosHelper.GetComboCategoriesAsync();
            return View(model);
        }
    }
}
