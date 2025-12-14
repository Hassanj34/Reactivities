using System.Net.Http.Headers;
using System.Text;
using API.DTOs;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using static API.DTOs.GitHubInfo;

namespace API.Controllers
{
    public class AccountController(SignInManager<User> signInManager, IEmailSender<User> emailSender, IConfiguration config) : BaseApiController
    {
        [AllowAnonymous]
        [HttpPost("github-login")]
        public async Task<ActionResult> LogInWithGitHub(string code)
        {
            if (string.IsNullOrEmpty(code)) return BadRequest("Missing authorization code");

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //step 1 - Exchange code for access token
            var tokenResponse = await httpClient.PostAsJsonAsync("https://github.com/login/oauth/access_token",
                new GitHubAuthRequest
                {
                    ClientId = config["Authentication:GitHub:ClientId"]!,
                    ClientSecret = config["Authentication:GitHub:ClientSecret"]!,
                    Code = code,
                    RedirectUrl = $"{config["ClientAppUrl"]!}/auth-callback"
                }
            );

            if (!tokenResponse.IsSuccessStatusCode) return BadRequest("Failed to get access token");

            var tokenContent = await tokenResponse.Content.ReadFromJsonAsync<GitHubTokenResponse>();

            if (string.IsNullOrEmpty(tokenContent?.AccessToken)) return BadRequest("Failed to retrieve access token");

            //step 2 - Fetch user info from GitHub
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenContent.AccessToken);
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Reactivities");

            var userResponse = await httpClient.GetAsync("https://api.github.com/user");

            if (!userResponse.IsSuccessStatusCode) return BadRequest("Failed to fetch user from GitHub");

            var user = await userResponse.Content.ReadFromJsonAsync<GitHubUser>();

            if (user == null) return BadRequest("Failed to get user from GitHub");

            //step 3 - Getting the email if needed
            if (string.IsNullOrEmpty(user?.Email))
            {
                var emailResponse = await httpClient.GetAsync("https://api.github.com/user/emails");

                if (emailResponse.IsSuccessStatusCode)
                {
                    var emails = await emailResponse.Content.ReadFromJsonAsync<List<GitHubEmail>>();

                    var primaryEmail = emails?.FirstOrDefault(e => e is { Primary: true, Verified: true })?.Email;

                    if (string.IsNullOrEmpty(primaryEmail)) return BadRequest("Failed to get email from GitHub");

                    user!.Email = primaryEmail;
                }
            }

            //step 4 - Find or create user and sign in
            var existingUser = await signInManager.UserManager.FindByEmailAsync(user!.Email);

            if (existingUser == null)
            {
                existingUser = new User
                {
                    Email = user.Email,
                    UserName = user.Email,
                    DisplayName = user.Name,
                    ImageUrl = user.ImageUrl
                };

                var createdResult = await signInManager.UserManager.CreateAsync(existingUser);

                if (!createdResult.Succeeded) return BadRequest("Failed to create user");
            }

            await signInManager.SignInAsync(existingUser, false);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(RegisterDTO registerDTO)
        {
            var user = new User
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName
            };

            var result = await signInManager.UserManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                await SendConfirmationEmailAsync(user, registerDTO.Email);
                return Ok();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        [AllowAnonymous]
        [HttpGet("resendConfirmEmail")]
        public async Task<ActionResult> ResendConfirmEmail(string? email, string? userId)
        {
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(userId)) return BadRequest("Email or User ID must be provided");

            var user = await signInManager.UserManager.Users.FirstOrDefaultAsync(x => x.Email == email || x.Id == userId);

            if (user == null || string.IsNullOrEmpty(user.Email)) return BadRequest("User not found");

            await SendConfirmationEmailAsync(user, user.Email);

            return Ok();
        }

        private async Task SendConfirmationEmailAsync(User user, string email)
        {
            var code = await signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var confirmEmailUrl = $"{config["ClientAppUrl"]}/confirm-email?userId={user.Id}&code={code}";

            await emailSender.SendConfirmationLinkAsync(user, email, confirmEmailUrl);
        }

        [AllowAnonymous]
        [HttpGet("user-info")]
        public async Task<ActionResult> GetUserInfo()
        {
            if (User.Identity?.IsAuthenticated == false) return NoContent();

            var user = await signInManager.UserManager.GetUserAsync(User);

            if (user == null) return Unauthorized();

            return Ok(new
            {
                user.DisplayName,
                user.Email,
                user.Id,
                user.ImageUrl
            });
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return NoContent();
        }

        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            var user = await signInManager.UserManager.GetUserAsync(User);

            if (user == null) return Unauthorized();

            var result = await signInManager.UserManager
                .ChangePasswordAsync(user, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);

            if (result.Succeeded) return Ok();

            return BadRequest(result.Errors.First().Description);
        }
    }
}