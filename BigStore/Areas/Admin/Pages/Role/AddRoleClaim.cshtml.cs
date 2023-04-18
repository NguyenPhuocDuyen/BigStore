using BigStore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BigStore.Areas.Admin.Pages.Role
{
    public class AddRoleClaim : RolePageModel
    {
        private readonly ApplicationDbContext _context;

        public AddRoleClaim(RoleManager<IdentityRole> roleManager, ApplicationDbContext context) : base(roleManager)
        {
            _context = context;
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Phải nhập tên của đặc tính")]
            [Display(Name = "Tên của đặc tính (Claim)")]
            [StringLength(256, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
            public string ClaimType { set; get; } = string.Empty;

            [Required(ErrorMessage = "Phải nhập giá trị của đặc tính")]
            [Display(Name = "Giá trị của đặc tính (Claim)")]
            [StringLength(256, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
            public string ClaimValue { set; get; } = string.Empty;
        }

        [BindProperty]
        public InputModel Input { set; get; }

        public IdentityRole Role { get; set; }

        public async Task<IActionResult> OnGet(string roleid)
        {
            Role = await _roleManager.FindByIdAsync(roleid);
            if (Role == null)
                return NotFound("Không tìm thấy role");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            Role = await _roleManager.FindByIdAsync(roleid);
            if (Role == null) return NotFound("Không tìm thấy role");

            if (!ModelState.IsValid) return Page();

            if ((await _roleManager.GetClaimsAsync(Role)).Any(c => c.Type == Input.ClaimType && c.Value == Input.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Claim này đã có trong role");
                return Page();
            }

            var newClaim = new Claim(Input.ClaimType, Input.ClaimValue);

            var result = await _roleManager.AddClaimAsync(Role, newClaim);

            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
                return Page();
            }

            StatusMessage = "Vừa thêm đặc tính (claim) mới.";

            return RedirectToPage("./Edit", new { roleid });
        }
    }
}
