using Elite_Training_Club.Data;
using Elite_Training_Club.Data.Entities;
using Elite_Training_Club.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace Elite_Training_Club.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PlansController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;

        public PlansController(DataContext context, IFlashMessage flashMessage)
        {
           _context = context;
            _flashMessage = flashMessage;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Plans
                .Include(c => c.SubscriptionsPlans)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Plan plan = await _context.Plans.FindAsync(id);

            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Delete(int? id)
        {
            Plan plan = await _context.Plans.FirstOrDefaultAsync(c => c.Id == id);
            try
            {
                _context.Plans.Remove(plan);
                await _context.SaveChangesAsync();
                _flashMessage.Info("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar el plan porque tiene registros relacionados.");
            }

            return RedirectToAction(nameof(Index));
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Plan());
            }
            else
            {
                Plan plan = await _context.Plans.FindAsync(id);
                if (plan == null)
                {
                    return NotFound();
                }

                return View(plan);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, Plan plan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == 0) //Insert
                    {
                        _context.Add(plan);
                        await _context.SaveChangesAsync();
                        _flashMessage.Info("Registro creado.");
                    }
                    else //Update
                    {
                        _context.Update(plan);
                        await _context.SaveChangesAsync();
                        _flashMessage.Info("Registro actualizado.");
                    }
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe un plan con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                    return View(plan);
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                    return View(plan);
                }

                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAll", _context.Plans.Include(c => c.SubscriptionsPlans).ToList()) });

            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", plan) });
        }

    }

}
