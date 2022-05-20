using Elite_Training_Club.Data;
using Elite_Training_Club.Data.Entities;
using Elite_Training_Club.Helpers;
using Elite_Training_Club.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace Elite_Training_Club.Controllers
{

    [Authorize(Roles = "Admin")]

    public class SubscriptionsController : Controller

    {

        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IFlashMessage _flashMessage;

        public SubscriptionsController(DataContext context, ICombosHelper combosHelper, IFlashMessage flashMessage)
        {
            _context = context;
            _combosHelper = combosHelper;
            _flashMessage = flashMessage;
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
                        _flashMessage.Danger("Ya existe una suscripción con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                }
            }
            model.Plans = await _combosHelper.GetComboCategoriesAsync();
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subscriptions subscriptions = await _context.Subscriptions.FindAsync(id);
            if (subscriptions == null)
            {
                return NotFound();
            }

            EditSubscriptionsViewModel model = new()
            {
                Description = subscriptions.Description,
                Id = subscriptions.Id,
                Name = subscriptions.Name,
                Price = subscriptions.Price,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateSubscriptionsViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {
                Subscriptions subscriptions = await _context.Subscriptions.FindAsync(model.Id);
                subscriptions.Description = model.Description;
                subscriptions.Name = model.Name;
                subscriptions.Price = model.Price;
                _context.Update(subscriptions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    _flashMessage.Danger("Ya existe una suscripción con el mismo nombre.");
                }
                else
                {
                    _flashMessage.Danger(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                _flashMessage.Danger( exception.Message);
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subscriptions subscriptions = await _context.Subscriptions
                .Include(s => s.SubscriptionsPlans)
                .ThenInclude(sp => sp.Plan)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (subscriptions == null)
            {
                return NotFound();
            }

            return View(subscriptions);
        }


        public async Task<IActionResult> AddPlan(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subscriptions subscriptions = await _context.Subscriptions
                .Include(s => s.SubscriptionsPlans)
                .ThenInclude(sp => sp.Plan)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (subscriptions == null)
            {
                return NotFound();
            }

            List<Plan> plans = subscriptions.SubscriptionsPlans.Select(sp => new Plan
            {
                Id = sp.Plan.Id,
                Name = sp.Plan.Name,
            }).ToList();

            AddSubscriptionPlansViewModel model = new()
            {
                SubscriptionId = subscriptions.Id,
                Plans = await _combosHelper.GetComboPlansAsync(plans),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPlan(AddSubscriptionPlansViewModel model)
        {
           
               
            if (ModelState.IsValid)
            {
                Subscriptions subscriptions = await _context.Subscriptions
                   .Include(s => s.SubscriptionsPlans)
                   .ThenInclude(sp => sp.Plan)
                   .FirstOrDefaultAsync(s => s.Id == model.PlanId);
                SubscriptionsPlan subscriptionPlans = new()
                {
                    Plan = await _context.Plans.FindAsync(model.PlanId),
                    Subscriptions = subscriptions,
                };

                try
                {
                    _context.Add(subscriptionPlans);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new {Id = subscriptions.Id });
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                }
            }

            model.Plans = await _combosHelper.GetComboPlansAsync();
            return View(model);
        }

        public async Task<IActionResult> DeletePlan(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SubscriptionsPlan subscriptionsPlan = await _context.SubscriptionsPlans
                .Include(sp => sp.Subscriptions)
                .FirstOrDefaultAsync(sp => sp.Id == id);
            if (subscriptionsPlan == null)
            {
                return NotFound();
            }

            _context.SubscriptionsPlans.Remove(subscriptionsPlan);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Details), new { Id = subscriptionsPlan.Subscriptions.Id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subscriptions subscriptions = await _context.Subscriptions
                .Include(s => s.SubscriptionsPlans)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (subscriptions == null)
            {
                return NotFound();
            }

            return View(subscriptions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Subscriptions model)
        {
            Subscriptions subscriptions = await _context.Subscriptions
               .Include(s => s.SubscriptionsPlans)
               .FirstOrDefaultAsync(s => s.Id ==model.Id);


            _context.Subscriptions.Remove(subscriptions);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Index));
        }

    }
}
