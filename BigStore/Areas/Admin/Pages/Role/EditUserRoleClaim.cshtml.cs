using BigStore.Data;
using BigStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BigStore.Areas.Admin.Pages.Role
{
    public class EditUserRoleClaimModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public EditUserRoleClaimModel(RoleManager<IdentityRole> roleManager,
                            UserManager<User> userManager,
                            ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public NotFoundObjectResult OnGet() => NotFound("Không được truy cập");

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

        [BindProperty] public InputModel Input { set; get; }
        public User User { get; set; }

        public async Task<IActionResult> OnGetAddClaimAsync(string userid)
        {
            User = await _userManager.FindByIdAsync(userid);
            if (User == null) return NotFound("Khong tim thay user");

            return Page();
        }

        public async Task<IActionResult> OnPostAddClaimAsync(string userid)
        {
            User = await _userManager.FindByIdAsync(userid);
            if (User == null) return NotFound("Khong tim thay user");

            if (!ModelState.IsValid) return Page();

            var claims = _context.UserClaims.Where(c => c.UserId == userid);
            if (claims.Any(c => c.ClaimType == Input.ClaimType && c.ClaimValue == Input.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Đặc tính này đã có");
                return Page();
            }

            await _userManager.AddClaimAsync(User, new Claim(Input.ClaimType, Input.ClaimValue));
            StatusMessage = "Đã thêm đặc tính cho user";

            return Page();
        }

        public IdentityUserClaim<string> UserClaim { get; set; }

        public async Task<IActionResult> OnGetEditClaimAsync(int? claimid)
        {
            if (claimid == null) return NotFound("Khong tim thay claim");

            UserClaim = await _context.UserClaims.FirstOrDefaultAsync(c => c.Id == claimid);
            if (UserClaim == null) return NotFound("Không tìm thấy claim");

            User = await _userManager.FindByIdAsync(UserClaim.UserId);
            if (User == null) return NotFound("Khong tim thay user");

            Input = new InputModel
            {
                ClaimType = UserClaim.ClaimType,
                ClaimValue = UserClaim.ClaimValue
            };

            return Page();
        }

        public async Task<IActionResult> OnPostEditClaimAsync(int? claimid)
        {
            if (claimid == null) return NotFound("Khong tim thay claim");

            UserClaim = await _context.UserClaims.FirstOrDefaultAsync(c => c.Id == claimid);
            if (UserClaim == null) return NotFound("Không tìm thấy claim");

            User = await _userManager.FindByIdAsync(UserClaim.UserId);
            if (User == null) return NotFound("Khong tim thay user");

            if (!ModelState.IsValid) return Page();

            if ((await _context.UserClaims.AnyAsync(
                c => c.UserId == User.Id
                && c.Id != claimid
                && c.ClaimType == Input.ClaimType
                && c.ClaimValue == Input.ClaimValue)))
            {
                ModelState.AddModelError(string.Empty, "Đã tồn tại đặc tính này");
                return Page();
            }

            UserClaim.ClaimType = Input.ClaimType;
            UserClaim.ClaimValue = Input.ClaimValue;
            await _context.SaveChangesAsync();

            StatusMessage = "Đã cập nhật đặc tính cho user";
            return RedirectToPage("./AddUserRole", new { userid = User.Id });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? claimid)
        {
            if (claimid == null) return NotFound("Khong tim thay claim");

            UserClaim = await _context.UserClaims.FirstOrDefaultAsync(c => c.Id == claimid);
            if (UserClaim == null) return NotFound("Không tìm thấy claim");

            User = await _userManager.FindByIdAsync(UserClaim.UserId);
            if (User == null) return NotFound("Khong tim thay user");

            await _userManager.RemoveClaimAsync(User, new Claim(UserClaim.ClaimType, UserClaim.ClaimValue));

            StatusMessage = "Đã xoá đặc tính cho user";
            return RedirectToPage("./AddUserRole", new { userid = User.Id });
        }
    }
}
