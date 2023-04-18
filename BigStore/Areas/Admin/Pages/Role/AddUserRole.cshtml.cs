using BigStore.Data;
using BigStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BigStore.Areas.Admin.Pages.Role
{
    [Authorize(Policy = "AllowEditRole")]
    public class AddUserRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public AddUserRoleModel(RoleManager<IdentityRole> roleManager,
                            UserManager<User> userManager,
                            ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public class InputModel
        {
            [Required]
            public string ID { set; get; }
            public string? Name { set; get; }

            public string[]? RoleNames { set; get; }
        }

        [BindProperty]
        public InputModel Input { set; get; }

        [BindProperty]
        public bool isConfirmed { set; get; }

        [TempData] // Sử dụng Session
        public string StatusMessage { get; set; }

        public List<string> AllRoles { set; get; } = new List<string>();

        public List<IdentityRoleClaim<string>> ClaimInRoles { get; set; }
        public List<IdentityUserClaim<string>> ClaimInUsers { get; set; }

        public async Task<IActionResult> OnGetAsync(string userid)
        {
            if (string.IsNullOrEmpty(userid)) return NotFound("Không thấy user");

            var user = await _userManager.FindByIdAsync(userid);
            if (user == null) return NotFound("Không thấy user");

            var roles = await _userManager.GetRolesAsync(user);
            var allroles = await _roleManager.Roles.ToListAsync();

            allroles.ForEach((r) =>
            {
                AllRoles.Add(r.Name);
            });

            //get claim in role user have
            var listRoles = from r in _context.Roles
                            join ur in _context.UserRoles on r.Id equals ur.RoleId
                            where ur.UserId == user.Id
                            select r;

            var _claimInRole = from c in _context.RoleClaims
                               join r in listRoles on c.RoleId equals r.Id
                               select c;

            ClaimInRoles = await _claimInRole.ToListAsync();
            ClaimInUsers = await _context.UserClaims.Where(c => c.UserId == user.Id).ToListAsync();

            Input = new InputModel
            {
                ID = user.Id,
                Name = user.UserName,
                RoleNames = roles.ToArray()
            };

            Input.Name = user.UserName;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var user = await _userManager.FindByIdAsync(Input.ID);
            if (user == null)
            {
                return NotFound("Không thấy user");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var allroles = await _roleManager.Roles.ToListAsync();

            allroles.ForEach((r) =>
            {
                AllRoles.Add(r.Name);
            });

            //get claim in role user have
            var listRoles = from r in _context.Roles
                            join ur in _context.UserRoles on r.Id equals ur.RoleId
                            where ur.UserId == user.Id
                            select r;

            var _claimInRole = from c in _context.RoleClaims
                               join r in listRoles on c.RoleId equals r.Id
                               select c;

            ClaimInRoles = await _claimInRole.ToListAsync();
            ClaimInUsers = await _context.UserClaims.Where(c => c.UserId == user.Id).ToListAsync();

            if (!isConfirmed)
            {
                Input.RoleNames = roles.ToArray();
                isConfirmed = true;
                StatusMessage = "";
                ModelState.Clear();
            }
            else
            {
                // Update add and remove
                StatusMessage = "Vừa cập nhật";
                if (Input.RoleNames == null) Input.RoleNames = new string[] { };
                foreach (var rolename in Input.RoleNames)
                {
                    if (roles.Contains(rolename)) continue;
                    await _userManager.AddToRoleAsync(user, rolename);
                }
                foreach (var rolename in roles)
                {
                    if (Input.RoleNames.Contains(rolename)) continue;
                    await _userManager.RemoveFromRoleAsync(user, rolename);
                }

            }

            Input.Name = user.UserName;
            return Page();
        }
    }
}
