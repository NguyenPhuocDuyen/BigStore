using System.ComponentModel.DataAnnotations;

namespace BigStore.Validation
{
    public class BirthDay : ValidationAttribute
    {
        public BirthDay() => ErrorMessage = "{0} khong hop le";

        public override bool IsValid(object? value)
        {
            if (value == null) return false;

            DateTime? date = (DateTime?)value;

            if (date >= DateTime.Now.AddYears(-16)) return false;

            return base.IsValid(value);
        }
    }
}
