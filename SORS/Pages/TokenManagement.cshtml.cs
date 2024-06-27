using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SORS.Pages
{
    public class TokenManagementModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public TokenManagementModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public List<UserTokenViewModel> UserTokens { get; set; }

        public async Task OnGetAsync()
        {
            UserTokens = new List<UserTokenViewModel>();

            var users = _userManager.Users;
            foreach (var user in users)
            {
                var token = await _userManager.GetAuthenticationTokenAsync(user, "Default", "AccessToken");
                UserTokens.Add(new UserTokenViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    Value = token
                });
            }
        }

        public async Task<IActionResult> OnPostGenerateTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var token = GenerateRandomToken();
                await _userManager.SetAuthenticationTokenAsync(user, "Default", "AccessToken", token);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "AccessToken");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditTokenAsync([FromBody] EditTokenModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user != null)
            {
                await _userManager.SetAuthenticationTokenAsync(user, "Default", "AccessToken", model.Token);
            }

            return RedirectToPage();
        }

        private string GenerateRandomToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }

        public class EditTokenModel
        {
            public string UserId { get; set; }
            public string Token { get; set; }
        }
    }

    public class UserTokenViewModel
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string Value { get; set; }
    }
}
