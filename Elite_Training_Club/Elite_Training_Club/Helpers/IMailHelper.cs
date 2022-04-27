using Elite_Training_Club.common;

namespace Elite_Training_Club.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string toName, string toEmail, string subject, string body);
    }
}
