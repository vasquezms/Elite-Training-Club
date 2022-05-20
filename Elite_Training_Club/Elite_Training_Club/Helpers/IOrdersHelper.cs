using Elite_Training_Club.common;
using Elite_Training_Club.Models;

namespace Elite_Training_Club.Helpers
{
    public interface IOrdersHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel model);
        Task<Response> CancelOrderAsync(int id);

    }

}
