using BigStore.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BigStore.Areas.Admin.Pages.Role
{
    public class DeleteModel : RolePageModel
    {
        public DeleteModel(RoleManager<IdentityRole> roleManager) : base(roleManager)
        {
        }

        public class InputModel
        {
            [Required]
            public string ID { set; get; }
            public string? Name { set; get; }
        }

        [BindProperty]
        public InputModel Input { set; get; }

        public async Task<IActionResult> OnGet(string roleid)
        {
            if (string.IsNullOrEmpty(roleid))
            {
                StatusMessage = "Không tìm thấy role";
                return RedirectToPage("./Index");
            }

            var role = await _roleManager.FindByIdAsync(roleid);
            if (role is null)
            {
                return NotFound("Không thấy role cần xóa");
            }

            Input = new InputModel { 
                ID = roleid ,
                Name = role.Name
            };

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var role = await _roleManager.FindByIdAsync(Input.ID);
            if (role is null)
            {
                return NotFound("Không thấy role cần xóa");
            }

            await _roleManager.DeleteAsync(role);
            StatusMessage = "Đã xóa " + role.Name;

            return RedirectToPage("./Index");
        }
    }
}
