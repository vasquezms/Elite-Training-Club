using Elite_Training_Club.Data.Entities;
using Elite_Training_Club.Enums;
using Elite_Training_Club.Helpers;

namespace Elite_Training_Club.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {            
            _context = context;
            _userHelper = userHelper;
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckPlanAsync();
            await CheckCategoriesAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Hernan", "Berrio", "hernan@yopmail.com", "317 891 1968", "San Gabril Itagui", UserType.Admin);
            await CheckUserAsync("2020", "Santiago", "Muñoz", "santiago@yopmail.com", "300 815 8260", "La Gabriela Bello", UserType.User);

        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Adelgazantes" });
                _context.Categories.Add(new Category { Name = "Accesorios deportivos" });
                _context.Categories.Add(new Category { Name = "Productos nutricionales" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }

            return user;
        }


        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }


        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                { 
                   Name = "Colombia",
                   States = new List<State>()
                   {
                       new State {
                           Name = "Antioquia",
                           Cities = new List<City>()
                           {
                               new City
                               {
                                  Name = "Bello",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Bello Elite Training Club 1"},
                                   new Headquarter {Name ="Bello Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Copacabana",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Copacabana Elite Training Club 1"},
                                   new Headquarter {Name ="Copacabana Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Envigado",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Envigado Elite Training Club 1"},
                                   new Headquarter {Name ="Envigado Elite Training Club 2"}

                                  }
                               },
                               new City 
                               { 
                                  Name = "Medellín",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Medellín Elite Training Club 1"},
                                   new Headquarter {Name ="Medellín Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Marinilla",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Marinilla Elite Training Club 1"},
                                   new Headquarter {Name ="Marinilla Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Rionegro",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Rionegro Elite Training Club 1"},
                                   new Headquarter {Name ="Rionegro Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Sabaneta",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Sabaneta Elite Training Club 1"},
                                   new Headquarter {Name ="Sabaneta Elite Training Club 2"}

                                  }
                               },
                           }
                       },                      
                       new State {
                           Name = "Bogotá",
                           Cities = new List<City>()
                           {
                               new City
                               {
                                  Name = "Bosa",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Bosa Elite Training Club 1"},
                                   new Headquarter {Name ="Bosa Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Santa Fe",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Santa Fe Elite Training Club 1"},
                                   new Headquarter {Name ="Santa Fe Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Kennedy",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Kennedy Elite Training Club 1"},
                                   new Headquarter {Name ="Kennedy Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Usme",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Usme Elite Training Club 1"},
                                   new Headquarter {Name ="Usme Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Chapinero",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Chapinero Elite Training Club 1"},
                                   new Headquarter {Name ="Chapinero Elite Training Club 2"}

                                  }
                               },
                           }
                       },
                       new State {
                           Name = "Valle del Cauca",
                           Cities = new List<City>()
                           {
                               new City
                               {
                                  Name = "Cali",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Cali Elite Training Club 1"},
                                   new Headquarter {Name ="Cali Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Tuluá",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Tuluá Elite Training Club 1"},
                                   new Headquarter {Name ="Tuluá Elite Training Club 2"}

                                  }
                               }
                           }
                       },
                       new State {
                           Name = "Atlantico",
                           Cities = new List<City>()
                           {
                               new City
                               {
                                  Name = "Barranquilla",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Barranquilla Elite Training Club 1"},
                                   new Headquarter {Name ="Barranquilla Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Malambo",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Malambo Elite Training Club 1"},
                                   new Headquarter {Name ="Malambo Elite Training Club 2"}

                                  }
                               }
                           }
                       }
                   }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>()
                   {
                       new State {
                           Name = "Florida",
                           Cities = new List<City>()
                           {
                               new City
                               {
                                  Name = "Orlando",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Orlando Elite Training Club 1"},
                                   new Headquarter {Name ="Orlando Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Miami",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Miami Elite Training Club 1"},
                                   new Headquarter {Name ="Miami Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Tampa",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Tampa Elite Training Club 1"},
                                   new Headquarter {Name ="Tampa Elite Training Club 2"}

                                  }
                               }
                           }
                       },
                       new State {
                           Name = "Texas",
                           Cities = new List<City>()
                           {
                               new City
                               {
                                  Name = "Houston",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Houston Elite Training Club 1"},
                                   new Headquarter {Name ="Houston Elite Training Club 2"}
                                  }
                               },
                               new City
                               {
                                  Name = "San Antonio",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="San Antonio Elite Training Club 1"},
                                   new Headquarter {Name ="San Antonio Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Dallas",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Dallas Elite Training Club 1"},
                                   new Headquarter {Name ="Dallas Elite Training Club 2"}

                                  }
                               }
                           }
                       },
                       new State {
                           Name = "California",
                           Cities = new List<City>()
                           {
                               new City
                               {
                                  Name = "Los Ángeles",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Los Ángeles Elite Training Club 1"},
                                   new Headquarter {Name ="Los Ángeles Elite Training Club 2"}
                                  }
                               },
                               new City
                               {
                                  Name = "San Francisco",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="San Francisco Elite Training Club 1"},
                                   new Headquarter {Name ="San Francisco Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "San Diego",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="San Diego Elite Training Club 1"},
                                   new Headquarter {Name ="San Diego Elite Training Club 2"}

                                  }
                               }
                           }
                       },
                        new State {
                           Name = "New Jersey",
                           Cities = new List<City>()
                           {
                               new City
                               {
                                  Name = "Newark",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Newark Elite Training Club 1"},
                                   new Headquarter {Name ="Newark Elite Training Club 2"}
                                  }
                               },
                               new City
                               {
                                  Name = "Atlantic City",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Atlantic City Elite Training Club 1"},
                                   new Headquarter {Name ="Atlantic City Elite Training Club 2"}

                                  }
                               }
                           }
                        },
                        new State {
                           Name = "New York",
                           Cities = new List<City>()
                           {
                               new City
                               {
                                  Name = "Búfalo",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Búfalo Elite Training Club 1"},
                                   new Headquarter {Name ="Búfalo Elite Training Club 2"}
                                  }
                               },
                               new City
                               {
                                  Name = "Siracusa",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Siracusa City Elite Training Club 1"},
                                   new Headquarter {Name ="Siracusa City Elite Training Club 2"}

                                  }
                               }
                           }
                        }
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name = "España",
                    States = new List<State>()
                   {
                       new State {
                           Name = "Madrid",
                           Cities = new List<City>()
                           {
                               new City
                               {
                                  Name = "Aranjuez",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Aranjuez Elite Training Club 1"},
                                   new Headquarter {Name ="Aranjuez Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Alcorcón",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Alcorcón Elite Training Club 1"},
                                   new Headquarter {Name ="Alcorcón Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Mostoles",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Mostoles Elite Training Club 1"},
                                   new Headquarter {Name ="Mostoles Elite Training Club 2"}

                                  }
                               }
                           }
                       },
                       new State {
                           Name = "Barcelona",
                           Cities = new List<City>()
                           {
                               new City
                               {
                                  Name = "Badalona",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Badalona Elite Training Club 1"},
                                   new Headquarter {Name ="Badalona Elite Training Club 2"}
                                  }
                               },
                               new City
                               {
                                  Name = "Viladecans",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Viladecans Elite Training Club 1"},
                                   new Headquarter {Name ="Viladecans Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Terrassa",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Terrassa Elite Training Club 1"},
                                   new Headquarter {Name ="Terrassa Elite Training Club 2"}

                                  }
                               }
                           }
                       },
                       new State {
                           Name = "Valencia",
                           Cities = new List<City>()
                           {
                               new City
                               {
                                  Name = "Sagunto",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Sagunto Elite Training Club 1"},
                                   new Headquarter {Name ="Sagunto Elite Training Club 2"}
                                  }
                               },
                               new City
                               {
                                  Name = "Torrente",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Torrente Elite Training Club 1"},
                                   new Headquarter {Name ="Torrente Elite Training Club 2"}

                                  }
                               },
                               new City
                               {
                                  Name = "Catarroja",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Catarroja Elite Training Club 1"},
                                   new Headquarter {Name ="Catarroja Elite Training Club 2"}

                                  }
                               }
                           }
                       },
                        new State {
                           Name = "Sevilla",
                           Cities = new List<City>()
                           {
                               new City
                               {
                                  Name = "Carmona",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Carmona Elite Training Club 1"},
                                   new Headquarter {Name ="Carmona Elite Training Club 2"}
                                  }
                               },
                               new City
                               {
                                  Name = "Marchena",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Marchena City Elite Training Club 1"},
                                   new Headquarter {Name ="Marchena City Elite Training Club 2"}

                                  }
                               }
                           }
                        },
                        new State {
                           Name = "Málaga",
                           Cities = new List<City>()
                           {
                               new City
                               {
                                  Name = "Marbella",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Marbella Elite Training Club 1"},
                                   new Headquarter {Name ="Marbella Elite Training Club 2"}
                                  }
                               },
                               new City
                               {
                                  Name = "Ronda",
                                  Headquarters = new List<Headquarter>()
                                  {
                                   new Headquarter {Name ="Ronda City Elite Training Club 1"},
                                   new Headquarter {Name ="Ronda City Elite Training Club 2"}

                                  }
                               }
                           }
                        }
                    }
                });
            }
            await _context.SaveChangesAsync();
        }
        private async Task CheckPlanAsync()
        {
            if (!_context.Plans.Any())
            {
                _context.Plans.Add(new Plan { Name = "Plan Basico" });
                _context.Plans.Add(new Plan { Name = "Plan Medio" });
                _context.Plans.Add(new Plan { Name = "Plan Premium" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
