using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;

namespace ShriGo.Pages
{
    public class RiderProfileModel : PageModel
    {
        private readonly RideDBContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        // Active User
        public List<UserModel> activeUser { get; set; } = new();

        // Driver Rides
        public List<SortedRideModel> only_DriverRides { get; set; } = new();

        [BindProperty]
        public SortedRideModel updateRecord { get; set; }

        [BindProperty]
        public IFormFile? ProfileImage { get; set; }

        // Constructor
        public RiderProfileModel(
            RideDBContext context,
            IWebHostEnvironment environment)
        {
            _dbContext = context;
            _environment = environment;
        }

        // Load Profile
        public async Task OnGetAsync()
        {
            string? session_UserName =
                HttpContext.Session.GetString("session_UserName");

            string? session_UserUniqueId =
                HttpContext.Session.GetString("session_UserUniqueId");

            if (string.IsNullOrEmpty(session_UserName) ||
                string.IsNullOrEmpty(session_UserUniqueId))
            {
                return;
            }

            // Active User
            activeUser = await _dbContext.UserTb
                .Where(u => u.UserFirstName == session_UserName)
                .ToListAsync();

            // Driver Rides
            only_DriverRides = await _dbContext.Ride_DBTable
                .Where(r => r.DriverUniqueId == session_UserUniqueId)
                .OrderByDescending(r => r.RideId)
                .ToListAsync();
        }

        // Upload Profile Photo
        public async Task<IActionResult> OnPostUploadPhotoAsync()
        {
            string? session_UserUniqueId =
                HttpContext.Session.GetString("session_UserUniqueId");

            if (string.IsNullOrEmpty(session_UserUniqueId))
            {
                TempData["Error"] = "Session expired.";
                return RedirectToPage();
            }

            if (ProfileImage == null || ProfileImage.Length == 0)
            {
                TempData["Error"] = "Please select an image.";
                return RedirectToPage();
            }

            // Allowed Extensions
            var allowedExtensions = new[]
            {
                ".jpg",
                ".jpeg",
                ".png"
            };

            var extension =
                Path.GetExtension(ProfileImage.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                TempData["Error"] =
                    "Only JPG and PNG images are allowed.";

                return RedirectToPage();
            }

            // File Size Limit (2MB)
            if (ProfileImage.Length > 2 * 1024 * 1024)
            {
                TempData["Error"] =
                    "Image size must be less than 2 MB.";

                return RedirectToPage();
            }

            // Find User
            var user = await _dbContext.UserTb
                .FirstOrDefaultAsync(u =>
                    u.UserUniqueId == session_UserUniqueId);

            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToPage();
            }

            // Upload Folder
            var uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "img",
                "drivers");

            // Create Folder
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Unique File Name
            var uniqueFileName =
                $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(
                uploadsFolder,
                uniqueFileName);

            // Save Image
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ProfileImage.CopyToAsync(stream);
            }

            // Delete Old Image
            if (!string.IsNullOrEmpty(user.UserImagePath))
            {
                var oldImagePath = Path.Combine(
                    _environment.WebRootPath,
                    user.UserImagePath.TrimStart('/'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Save Path To Database
            user.UserImagePath =
                $"/img/drivers/{uniqueFileName}";

            await _dbContext.SaveChangesAsync();

            TempData["Success"] =
                "Profile image updated successfully.";

            return RedirectToPage();
        }

        // Delete Driver Ride
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            string? session_UserUniqueId =
                HttpContext.Session.GetString("session_UserUniqueId");

            if (string.IsNullOrEmpty(session_UserUniqueId))
            {
                TempData["Error"] = "Session expired.";
                return RedirectToPage();
            }

            // Find Ride
            var rowToDelete = await _dbContext.Ride_DBTable
                .FirstOrDefaultAsync(r =>
                    r.RideId == id &&
                    r.DriverUniqueId == session_UserUniqueId);

            if (rowToDelete == null)
            {
                TempData["Error"] =
                    "Ride not found.";

                return RedirectToPage();
            }

            // Delete Ride
            _dbContext.Ride_DBTable.Remove(rowToDelete);

            await _dbContext.SaveChangesAsync();

            TempData["Success"] =
                "Ride deleted successfully.";

            return RedirectToPage();
        }

        // Redirect To Upload Ride
        public IActionResult OnPostUploadRide()
        {
            return RedirectToPage("/RideInput");
        }
    }
}