﻿#nullable disable
using Elite_Training_Club.Data;
using Elite_Training_Club.Data.Entities;
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
        public async Task<IActionResult> DetailsHeadquarter(int? id)
        {
            if (id == null)  
            {
                return NotFound();
            }
            Headquarter headquarter = await _context.Headquarters
                .Include(h => h.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (headquarter == null)
            {
                return NotFound();
            }
            return View(headquarter);
        }

        public IActionResult Create()
        {
            Country country = new() { States = new List<State>() };
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
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
            return View(country);
        }

        public async Task<IActionResult> AddState(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

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
                    return RedirectToAction(nameof(Details), new { Id = model.CountryId });
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
            return View(model);
        }
        public async Task<IActionResult> AddCity(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

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
                    return RedirectToAction(nameof(DetailsState), new { Id = model.StateId });
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
            return View(model);
        }
        public async Task<IActionResult> AddHeadquarter(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

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
                    return RedirectToAction(nameof(DetailsCity), new { Id = model.CityId });
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
            return View(model);
        }
        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .Include(c => c.States)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
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
            return View(country);
        }
        public async Task<IActionResult> EditState(int? id)
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
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = model.CountryId });
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
            return View(model);
        }
        public async Task<IActionResult> EditCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
                    return RedirectToAction(nameof(DetailsState), new { id = model.StateId });
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
            return View(model);
        }
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
                    return RedirectToAction(nameof(DetailsCity), new { id = model.CityId});
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
            return View(model);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .Include(c => c.States)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteState(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            State state = await _context.States
                .Include(s => s.Country)
                .FirstOrDefaultAsync(S => S.Id == id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        [HttpPost, ActionName("DeleteState")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStateConfirmed(int id)
        {
            State state = await _context.States
            .Include(s => s.Country)
            .FirstOrDefaultAsync(S => S.Id == id);
            _context.States.Remove(state);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");

            return RedirectToAction(nameof(Details), new { Id = state.Country.Id });
        }
        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            City city = await _context.Cities
                .Include(c => c.State)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [HttpPost, ActionName("DeleteCity")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCityConfirmed(int id)
        {
            City city = await _context.Cities
                .Include(c => c.State)
                .FirstOrDefaultAsync(c => c.Id == id);
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");

            return RedirectToAction(nameof(DetailsState), new { Id = city.State.Id });
        }
        public async Task<IActionResult> DeleteHeadquarter(int? id)
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

            return View(headquarter);
        }

        [HttpPost, ActionName("DeleteHeadquarter")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHeadquarterConfirmed(int id)
        {
            Headquarter headquarter = await _context.Headquarters
                .Include(h => h.City)
                .FirstOrDefaultAsync(h => h.Id == id);
            _context.Headquarters.Remove(headquarter);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");

            return RedirectToAction(nameof(DetailsCity), new { Id = headquarter.City.Id });
        }
    }
}

