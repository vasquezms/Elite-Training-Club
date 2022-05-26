#nullable disable
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
    public class CountriesController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;

        public CountriesController(DataContext context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Countries
           .Include(c => c.States)
           .ThenInclude(s => s.Cities)
           .ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Country country = await _context.Countries
                .Include(c => c.States)
                .ThenInclude(c => c.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }
        public async Task<IActionResult> DetailsState(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            State state = await _context.States
                .Include(s => s.Country)
                .Include(s => s.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (state == null)
            {
                return NotFound();
            }
            return View(state);
        }
        public async Task<IActionResult> DetailsCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            City city = await _context.Cities
                .Include(c => c.State)
                .Include(c => c.Headquarters)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }


        [NoDirectAccess]
        public async Task<IActionResult> AddState(int id)
        {

            Country country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            StateViewModel model = new()
            {
                CountryId = country.Id,
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddState(StateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    State state = new()
                    {
                        Cities = new List<City>(),
                        Country = await _context.Countries.FindAsync(model.CountryId),
                        Name = model.Name,

                    };
                    _context.Add(state);
                    await _context.SaveChangesAsync();
                    Country country = await _context.Countries
                            .Include(c => c.States)
                            .ThenInclude(s => s.Cities)
                            .FirstOrDefaultAsync(c => c.Id == model.CountryId);
                    _flashMessage.Info("Registro creado.");
                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllStates", country) });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe un departamento/estado con el mismo nombre " +
                            "en este país.");
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
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddState", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddCity(int id)
        {

            State state = await _context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }

            CityViewModel model = new()
            {
                StateId = state.Id,
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCity(CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    City city = new()
                    {
                        Headquarters = new List<Headquarter>(),
                        State = await _context.States.FindAsync(model.StateId),
                        Name = model.Name,
                    };
                    _context.Add(city);
                    await _context.SaveChangesAsync();
                    State state = await _context.States
                    .Include(c => c.Cities)
                    .ThenInclude(s => s.Headquarters)
                    .FirstOrDefaultAsync(c => c.Id == model.StateId);
                    _flashMessage.Info("Registro creado.");
                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllStates", state) });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una ciudad con el mismo nombre " +
                            "en este departamento/estado.");
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
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddCity", model) });
        }
        [NoDirectAccess]
        public async Task<IActionResult> AddHeadquarter(int id)
        {

            City city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            HeadquarterViewModel model = new()
            {
                CityId = city.Id,
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHeadquarter(HeadquarterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Headquarter headquarter = new()
                    {
                        City = await _context.Cities.FindAsync(model.CityId),
                        Name = model.Name,
                    };
                    _context.Add(headquarter);
                    await _context.SaveChangesAsync();
                   City city = await _context.Cities
                                   .Include(s => s.Headquarters)
                                   .FirstOrDefaultAsync(c => c.Id == model.CityId);
                    _flashMessage.Confirmation("Registro adicionado");

                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllHeadquarter", city) });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una sede con el mismo nombre " +
                            "en esta ciudad.");
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
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddHeadquarter", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> EditState(int id)
        {

            State state = await _context.States
                .Include(s => s.Country)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (state == null)
            {
                return NotFound();
            }
            StateViewModel model = new()
            {
                CountryId = state.Country.Id,
                Id = state.Id,
                Name = state.Name,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditState(int id, StateViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    State state = new()
                    {
                        Id = model.Id,
                        Name = model.Name,
                    };
                    _context.Update(state);
                    Country country = await _context.Countries
                   .Include(c => c.States)
                   .ThenInclude(s => s.Cities)
                   .FirstOrDefaultAsync(c => c.Id == model.CountryId);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Registro actualizado");
                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllStates", country) });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe un departamento/estado con el mismo" +
                            " nombre en este país.");
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
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "EditState", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> EditCity(int id)
        {
          

            City city = await _context.Cities
                .Include(c => c.State)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (city == null)
            {
                return NotFound();
            }
            CityViewModel model = new()
            {
                StateId = city.State.Id,
                Id = city.Id,
                Name = city.Name,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCity(int id, CityViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    City city = new()
                    {
                        Id = model.Id,
                        Name = model.Name,
                    };
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                    State state = await _context.States
                    .Include(s => s.Cities)
                    .ThenInclude(s => s.Headquarters)
                    .FirstOrDefaultAsync(c => c.Id == model.StateId);
                    _flashMessage.Confirmation("Registro actualizado.");
                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllCities", state) });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una ciudad con el mismo" +
                            " nombre en este depatamento/estado.");
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
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "EditCity", model) });
        }
        [NoDirectAccess]
        public async Task<IActionResult> EditHeadquarter(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Headquarter headquarter = await _context.Headquarters
                .Include(h => h.City)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (headquarter == null)
            {
                return NotFound();
            }
            HeadquarterViewModel model = new()
            {
                CityId = headquarter.City.Id,
                Id = headquarter.Id,
                Name = headquarter.Name,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHeadquarter(int id, HeadquarterViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Headquarter headquarter = new()
                    {
                        Id = model.Id,
                        Name = model.Name,
                    };
                    _context.Update(headquarter);
                    await _context.SaveChangesAsync();
                    City city = await _context.Cities
                          .Include(s => s.Headquarters)
                          .FirstOrDefaultAsync(c => c.Id == model.CityId);
                    _flashMessage.Confirmation("Registro actualizado.");
                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllHeadquarter", city) });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una sede con el mismo" +
                            " nombre en esta ciudad.");
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
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "EditCity", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Delete(int id)
        {


            Country country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            try
            {
                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();
                _flashMessage.Info("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar el país porque tiene registros relacionados.");
            }

            return RedirectToAction(nameof(Index));
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Country());
            }
            else
            {
                Country country = await _context.Countries.FindAsync(id);
                if (country == null)
                {
                    return NotFound();
                }

                return View(country);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, Country country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == 0) //Insert
                    {
                        _context.Add(country);
                        await _context.SaveChangesAsync();
                        _flashMessage.Info("Registro creado.");
                    }
                    else //Update
                    {
                        _context.Update(country);
                        await _context.SaveChangesAsync();
                        _flashMessage.Info("Registro actualizado.");
                    }
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(
                            this,
                            "_ViewAll",
                            _context.Countries
                                .Include(c => c.States)
                                .ThenInclude(s => s.Cities)
                                .ToList())
                    });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe un país con el mismo nombre.");
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

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", country) });
        }


        public async Task<IActionResult> DeleteState(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            State state = await _context.States
                .Include(s => s.Country)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (state == null)
            {
                return NotFound();
            }

            try
            {
                _context.States.Remove(state);
                await _context.SaveChangesAsync();
                _flashMessage.Info("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar el estado / departamento porque tiene registros relacionados.");
            }

            return RedirectToAction(nameof(Details), new { Id = state.Country.Id });
        }


        [NoDirectAccess]
        public async Task<IActionResult> DeleteCity(int id)
        {

            City city = await _context.Cities
              .Include(c => c.State)
              .FirstOrDefaultAsync(c => c.Id == id);
            if (city == null)

            {
                return NotFound();
            }

            try
            {
                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();
                _flashMessage.Info("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar el estado / departamento porque tiene registros relacionados.");
            }

            return RedirectToAction(nameof(DetailsState), new { Id = city.State.Id });

        }

        [NoDirectAccess]
        public async Task<IActionResult> DeleteHeadquarter(int id)

        {        

            Headquarter headquarter = await _context.Headquarters
                .Include(c => c.City)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (headquarter == null)
            {
                return NotFound();
            }

            try
            {
                _context.Headquarters.Remove(headquarter);
                await _context.SaveChangesAsync();
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar la sede porque tiene registros relacionados.");
            }

            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(DetailsCity), new { Id = headquarter.City.Id });
        }

    }
}

