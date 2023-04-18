using BigStore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BigStore.Areas.Admin.Pages.Role
{
    public class CreateModel : RolePageModel
    {
        public CreateModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext context) : base(roleManager) { }

        public class InputModel
        {
            [Required(ErrorMessage = "Phải nhập tên role")]
            [Display(Name = "Tên của Role")]
            [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
            public string Name { set; get; } = string.Empty;
        }

        [BindProperty]
        public InputModel Input { set; get; }

        public void OnGet()
        {
            StatusMessage = "Hãy nhập thông tin để tạo role mới";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = null;
                return Page();
            }

            // TẠO MỚI
            var newRole = new IdentityRole(Input.Name);
            // Thực hiện tạo Role mới
            var rsNewRole = await _roleManager.CreateAsync(newRole);
            if (rsNewRole.Succeeded)
            {
                StatusMessage = $"Đã tạo role mới thành công: {newRole.Name}";
                return RedirectToPage("./Index");
            }
            else
            {
                StatusMessage = "Error: ";
                foreach (var er in rsNewRole.Errors)
                {
                    StatusMessage += er.Description;
                }
            }

            return Page();
        }
    }
}
