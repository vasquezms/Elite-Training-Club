using Microsoft.AspNetCore.Mvc.Rendering;

namespace Elite_Training_Club.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboPlansAsync();

        Task<IEnumerable<SelectListItem>> GetComboCountriesAsync();

        Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int countryId);

        Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId);
        Task<IEnumerable<SelectListItem>> GetComboHeadquarterAsync(int cityId);

    }
}
