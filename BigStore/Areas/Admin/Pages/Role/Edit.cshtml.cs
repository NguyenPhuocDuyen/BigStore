using BigStore.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BigStore.Areas.Admin.Pages.Role
{
    [Authorize(Policy = "AllowEditRole")]
    public class EditModel : RolePageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext context) : base(roleManager)
        {
            _context = context;
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Phải nhập tên role")]
            [Display(Name = "Tên của Role")]
            [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
            public string Name { set; get; } = string.Empty;
        }

        [BindProperty]
        public InputModel Input { set; get; }

        public IdentityRole Role { get; set; }

        public List<IdentityRoleClaim<string>> Claims { get; set; }

        public async Task<IActionResult> OnGet(string roleid)
        {
            if (string.IsNullOrWhiteSpace(roleid))
            {
                StatusMessage = "Không tìm thấy role";
                return RedirectToPage("./Index");
            }

            Role = await _roleManager.FindByIdAsync(roleid);
            if (Role is null)
            {
                StatusMessage = "Không tìm thấy role";
                return RedirectToPage("./Index");
            }

            Input = new InputModel
            {
                Name = Role.Name
            };

            Claims = await _context.RoleClaims.Where(rc => rc.RoleId == Role.Id).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPost(string roleid)
        {
            if (string.IsNullOrWhiteSpace(roleid))
            {
                StatusMessage = "Không tìm thấy role";
                return RedirectToPage("./Index");
            }

            if (!ModelState.IsValid)
                return Page();

            Role = await _roleManager.FindByIdAsync(roleid);

            if (Role is null)
            {
                StatusMessage = "Error: Không tìm thấy Role cập nhật";
            }

            Role.Name = Input.Name;
            // Cập nhật tên Role
            var roleUpdateRs = await _roleManager.UpdateAsync(Role);
            if (roleUpdateRs.Succeeded)
            {
                StatusMessage = "Đã cập nhật role thành công";
            }
            else
            {
                StatusMessage = "Error: ";
                foreach (var er in roleUpdateRs.Errors)
                {
                    StatusMessage += er.Description;
                }
            }

            return Page();
        }
    }
}
