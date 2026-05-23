using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;

namespace ShriGo.Pages.Passengers
{
    public class PassengerProfileModel : PageModel
    {
        private readonly RideDBContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        // Passenger
        public List<PassengerModel> activePassenger { get; set; } = new();

        // Bookings
        public List<BookingsModel> only_PassengerBookings { get; set; } = new();

        [BindProperty]
        public IFormFile? ProfileImage { get; set; }

        [BindProperty]
        public SortedRideModel updateRecord { get; set; }

        // Constructor
        public PassengerProfileModel(
            RideDBContext context,
            IWebHostEnvironment environment)
        {
            _dbContext = context;
            _environment = environment;
        }

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

            // Passenger Profile
            activePassenger = await _dbContext.PassengerTb
                .Where(p => p.PassengerFirstName == session_UserName)
                .ToListAsync();

            // Passenger Bookings
            only_PassengerBookings = await _dbContext.Bookings_DBTable
                .Where(b => b.PassengerUniqueId == session_UserUniqueId)
                .OrderByDescending(b => b.BookingId)
                .ToListAsync();
        }

        // Upload Passenger Profile Photo
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

            // Find Passenger
            var passenger = await _dbContext.PassengerTb
                .FirstOrDefaultAsync(p =>
                    p.PassengerUniqueId == session_UserUniqueId);

            if (passenger == null)
            {
                TempData["Error"] = "Passenger not found.";
                return RedirectToPage();
            }

            // Upload Folder
            var uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "img",
                "passengers");

            // Create Folder If Not Exists
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

            // Save New Image
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ProfileImage.CopyToAsync(stream);
            }

            // Delete Old Image
            if (!string.IsNullOrEmpty(passenger.PassengerImagePath))
            {
                var oldImagePath = Path.Combine(
                    _environment.WebRootPath,
                    passenger.PassengerImagePath.TrimStart('/'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Save Path To Database
            passenger.PassengerImagePath =
                $"/img/passengers/{uniqueFileName}";

            await _dbContext.SaveChangesAsync();

            TempData["Success"] =
                "Profile image updated successfully.";

            return RedirectToPage();
        }

        // Allows Passenger To Delete Booking
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            string? session_UserUniqueId =
                HttpContext.Session.GetString("session_UserUniqueId");

            if (string.IsNullOrEmpty(session_UserUniqueId))
            {
                TempData["Error"] = "Session expired.";
                return RedirectToPage();
            }

            // Find Booking
            var rowToDelete = await _dbContext.Bookings_DBTable
                .FirstOrDefaultAsync(b =>
                    b.BookingId == id &&
                    b.PassengerUniqueId == session_UserUniqueId);

            if (rowToDelete == null)
            {
                TempData["Error"] =
                    "Booking not found.";

                return RedirectToPage();
            }

            // Delete Booking
            _dbContext.Bookings_DBTable.Remove(rowToDelete);

            await _dbContext.SaveChangesAsync();

            TempData["Success"] =
                "Booking deleted successfully.";

            return RedirectToPage();
        }

        // Redirect To Book Ride
        public IActionResult OnPostBookRide()
        {
            return RedirectToPage("/Index");
        }
    }
}