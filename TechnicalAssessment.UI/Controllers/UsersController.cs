using Microsoft.AspNetCore.Mvc;
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
                totalUsers = users?.Count ?? 0;
            }

            ViewBag.TotalUsers = totalUsers;

            var groups = await _http.GetFromJsonAsync<List<GroupDto>>($"{_apiBaseUrl}/groups") 
                 ?? new List<GroupDto>();

            ViewBag.Groups = groups;

            return View(users);
        }

        // GET: Users/Create
        public async Task<IActionResult> Create()
        {
            var groups = await _http.GetFromJsonAsync<List<GroupDto>>($"{_apiBaseUrl}/groups");
            ViewBag.Groups = groups ?? new List<GroupDto>();
            var model = new UserCreateModel
            {
                GroupIds = new List<int>() 
            };

            return View(model);
        }


        // POST: Users/Create
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            var groupsList = await _http.GetFromJsonAsync<List<GroupDto>>($"{_apiBaseUrl}/groups");
            ViewBag.Groups = groupsList ?? new List<GroupDto>();

            if (!ModelState.IsValid)
                return View(model);

            var response = await _http.PostAsJsonAsync($"{_apiBaseUrl}/users", model);

            if (response.IsSuccessStatusCode)
            {
                ModelState.Clear();
                var emptyModel = new UserCreateModel
                {
                    GroupIds = new List<int>() 
                };

                ViewData["SuccessMessage"] = "User created successfully!";
                return View(emptyModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                ModelState.AddModelError("Email", "This email already exists. Please choose another.");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Failed to create user. Please try again.");
                return View(model);
            }
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

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditModel model)
        {
            var groupsList = await _http.GetFromJsonAsync<List<GroupDto>>($"{_apiBaseUrl}/groups");
            ViewBag.Groups = groupsList ?? new List<GroupDto>();

            if (!ModelState.IsValid)
                return View(model);

            var response = await _http.PutAsJsonAsync($"{_apiBaseUrl}/users/{model.Id}", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                ModelState.AddModelError("Email", "This email already exists. Please choose another.");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Failed to update user. Please try again.");
                return View(model);
            }
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
