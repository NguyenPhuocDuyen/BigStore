using Microsoft.AspNetCore.Identity;

namespace BigStore.Modes
{
    public class User : IdentityUser
    {
        public DateTime DOB { get; set; }
    }
}
