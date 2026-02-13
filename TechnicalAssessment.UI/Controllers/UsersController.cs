using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using TechnicalAssessment.UI.Models;

namespace TechnicalAssessment.UI.Controllers
{
    public class UsersController : Controller
    {
        private readonly HttpClient _http;
        private readonly string _apiBaseUrl = "http://localhost:5244/api";

        public UsersController(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient();
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _http.GetFromJsonAsync<List<UserDto>>($"{_apiBaseUrl}/users");

            int totalUsers = 0;

            try
            {
                totalUsers = await _http.GetFromJsonAsync<int>($"{_apiBaseUrl}/users/count");
            }
            catch
            {
            }

            ViewBag.TotalUsers = totalUsers;
            return View(users);
        }

        // GET: Users/Create
        public async Task<IActionResult> Create()
        {
            var groups = await _http.GetFromJsonAsync<List<GroupDto>>($"{_apiBaseUrl}/groups");

            // Ensure it's never null
            ViewBag.Groups = groups ?? new List<GroupDto>();

            // Optional: default selection
            var model = new UserCreateModel
            {
                GroupIds = new List<int>() // can pre-select default group IDs here
            };

            return View(model);
        }


        // POST: Users/Create
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _http.PostAsJsonAsync($"{_apiBaseUrl}/users", model);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Failed to create user");
            return View(model);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _http.GetFromJsonAsync<UserDto>($"{_apiBaseUrl}/users/{id}");
            if (user == null) return NotFound();

            var model = new UserEditModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                GroupIds = user.Groups.Select(g => g.Id).ToList()
            };

            var groups = await _http.GetFromJsonAsync<List<GroupDto>>($"{_apiBaseUrl}/groups");
            ViewBag.Groups = groups;

            return View(model);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _http.PutAsJsonAsync($"{_apiBaseUrl}/users/{model.Id}", model);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Failed to update user");
            return View(model);
        }

       // POST: Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"{_apiBaseUrl}/users/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Failed to delete user.";
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the user.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
