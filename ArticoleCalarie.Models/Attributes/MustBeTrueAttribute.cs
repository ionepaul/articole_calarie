using System.ComponentModel.DataAnnotations;

namespace ArticoleCalarie.Models.Utils
{
    public class MustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && (bool)value == true;
        }
    }
}