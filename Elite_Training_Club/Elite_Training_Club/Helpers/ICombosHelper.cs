using Elite_Training_Club.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Elite_Training_Club.Helpers
{
    public interface ICombosHelper

    {

        Task<IEnumerable<SelectListItem>> GetComboCategoriesAsync();

        Task<IEnumerable<SelectListItem>> GetComboCategoriesAsync(IEnumerable<Category> filter);

        Task<IEnumerable<SelectListItem>> GetComboPlansAsync();
        Task<IEnumerable<SelectListItem>> GetComboPlansAsync(IEnumerable<Plan> filter);

        Task<IEnumerable<SelectListItem>> GetComboCountriesAsync();

        Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int countryId);

        Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId);
        Task<IEnumerable<SelectListItem>> GetComboHeadquarterAsync(int cityId);

    }
}
