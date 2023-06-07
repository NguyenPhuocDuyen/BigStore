using BigStore.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BigStore.Areas.Admin.Pages.Role
{
    public class EditRoleClaim : RolePageModel
    {
        private readonly ApplicationDbContext _context;

        public EditRoleClaim(RoleManager<IdentityRole> roleManager, ApplicationDbContext context) : base(roleManager)
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

        public IdentityRoleClaim<string> Claim { get; set; }

        public async Task<IActionResult> OnGet(int? claimid)
        {
            if (claimid == null) return NotFound("không tìm thấy claim");

            Claim = await _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefaultAsync();
            if (Claim == null) return NotFound("không tìm thấy claim");

            Role = await _roleManager.FindByIdAsync(Claim.RoleId);
            if (Role == null) return NotFound("không tìm thấy role của claim");

            Input = new InputModel()
            {
                ClaimType = Claim.ClaimType,
                ClaimValue = Claim.ClaimValue
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? claimid)
        {
            if (claimid == null) return NotFound("không tìm thấy claim");

            Claim = await _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefaultAsync();
            if (Claim == null) return NotFound("không tìm thấy claim");

            Role = await _roleManager.FindByIdAsync(Claim.RoleId);
            if (Role == null) return NotFound("không tìm thấy role của claim");

            if (!ModelState.IsValid) return Page();

            if (_context.RoleClaims.Any(c =>
                c.RoleId == Role.Id
                && c.ClaimType == Input.ClaimType
                && c.ClaimValue == Input.ClaimValue
                && c.Id != claimid))
            {
                ModelState.AddModelError(string.Empty, "Claim này đã có trong role");
            }

            Claim.ClaimType = Input.ClaimType;
            Claim.ClaimValue = Input.ClaimValue;

            await _context.SaveChangesAsync();

            StatusMessage = "Vừa cập nhật đặc tính (claim).";

            return RedirectToPage("./Edit", new { roleid = Role.Id });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? claimid)
        {
            if (claimid == null) return NotFound("không tìm thấy claim");

            Claim = await _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefaultAsync();
            if (Claim == null) return NotFound("không tìm thấy claim");

            Role = await _roleManager.FindByIdAsync(Claim.RoleId);
            if (Role == null) return NotFound("không tìm thấy role của claim");

            await _roleManager.RemoveClaimAsync(Role, new Claim(Claim.ClaimType, Claim.ClaimValue));
            await _context.SaveChangesAsync();

            StatusMessage = "Vừa xoá đặc tính (claim).";

            return RedirectToPage("./Edit", new { roleid = Role.Id });
        }
    }
}
