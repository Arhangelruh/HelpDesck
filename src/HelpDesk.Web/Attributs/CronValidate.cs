using NCrontab;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Web.Attributs
{
    public class CronValidate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {            
            try
            {
                CrontabSchedule.Parse(value.ToString());
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}

