using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriGo.Helpers;
using ShriGo.Model;
using ShriGo.Pages.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Windows;
using Twilio;
using Twilio.Rest.Verify.V2.Service;


namespace ShriGo.Pages
{
    public class SignUpModel : PageModel
    {
        private const string smsSent = "Success";
        private readonly RideDBContext _dBContext;
        private readonly IConfiguration _config;

        public PhoneVerify twilo;
        public List<UserModel> listUserModel = new List<UserModel>();
        public List<PassengerModel> listPassengerModel = new List<PassengerModel>();

        [BindProperty]
        public UserModel NewUserModel { get; set; }

        [BindProperty]
        public PassengerModel NewPassengerModel { get; set; }

        private static readonly Random _random = new Random();
        public int UniqueNumber { get; set; }

        //Constructor
        public SignUpModel(RideDBContext context, IConfiguration config)
        {
            _dBContext = context;
            _config=config;
        }

        public void OnGet()
        {
        }
        //Mobile verification
        //public PhoneVerify Twilo(string phone)
        //{
        //    if (string.IsNullOrEmpty(phone))
        //    {
        //        return "Invalid"; // Path 1
        //    }
        //    var accountSid = _config["TwiloConnection.accountSid"];
        //    var authToken = _config["TwiloConnection.AuthToken"];
        //    TwilioClient.Init(accountSid, authToken);

        //    var verification = VerificationResource.Create(
        //        to: phone,
        //        channel: "sms",
        //        _config["TwiloConnection.pathServiceSid"]
        //        );

        //    Console.WriteLine(verification.Sid);
        //    // Missing return here!
        //    return verification; // Add this

        //}

        //Driver Signup
        public IActionResult OnPostDriver()
        {
            if (NewUserModel.UserPswd.Length < 8)
            {
                ViewData["Message"] = "Password must be minimum 8 characters";
                return Page();
            }

            //DriverId/Admin Id
             NewUserModel.UserId =
             _dBContext.UserTb.Any()
             ? _dBContext.UserTb.Max(r => r.UserId) + 1
             : 1;

            var passwordHelper = new PasswordHelper();
            NewUserModel.UserPswd = passwordHelper.HashPassword(NewUserModel.UserPswd);

            //DriverUniqueId
            //string driverLastName = _dBContext.DriversTb.Where(x=>x.DriverId == NewDriverModel.DriverId).Select(u => u.DriverLastName).FirstOrDefault();
            // Generate a random number between 1,000,000 and 9,999,999
            UniqueNumber = _random.Next(10000, 100000);

            //If the number doesn't need to be purely numeric or short, use a GUID for guaranteed uniqueness: 
            //string uniqueId = Guid.NewGuid().ToString("N");


            NewUserModel.UserUniqueId = UniqueNumber.ToString();

            // DriverReg Date only 
            NewUserModel.UserRegDate = TimeHelper.GetIndiaDate();

            NewUserModel.AcceptedTerms = true;
            //NewUserModel.AcceptedAt = DateTime.UtcNow;
            //NewUserModel.TermsVersion = "v1.0";
            //NewUserModel.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            _dBContext.UserTb.Add(NewUserModel);

            if (_dBContext.SaveChanges() ==1)
            {
                ViewData["Message"]= "Your details have been saved successfully!";
                return RedirectToPage("/SignIn");
            }
            else
            {
                return RedirectToPage("/Index");
            }


        }

        //Passenger Signup 
        public IActionResult OnPostPassenger()
        {
            if (NewPassengerModel.PassengerPswd.Length < 8)
            {
                ViewData["Message"] = "Password must be minimum 8 characters";
                return Page();
            }
            //PassengerId
            NewPassengerModel.PassengerId =
      _dBContext.PassengerTb.Any()
      ? _dBContext.PassengerTb.Max(r => r.PassengerId) + 1
      : 1;

            var passwordHelper = new PasswordHelper();
            NewPassengerModel.PassengerPswd = passwordHelper.HashPassword(NewPassengerModel.PassengerPswd);
            //DriverUniqueId
            //string driverLastName = _dBContext.DriversTb.Where(x=>x.DriverId == NewDriverModel.DriverId).Select(u => u.DriverLastName).FirstOrDefault();
            // Generate a random number between 1,000,000 and 9,999,999
            UniqueNumber = _random.Next(10000, 100000);

            //If the number doesn't need to be purely numeric or short, use a GUID for guaranteed uniqueness: 
            //string uniqueId = Guid.NewGuid().ToString("N");


            NewPassengerModel.PassengerUniqueId = UniqueNumber.ToString();

            // DriverReg Date only 
            NewPassengerModel.PassengerRegDate = TimeHelper.GetIndiaDate();

            NewPassengerModel.Role = "Passenger";

            NewPassengerModel.AcceptedTerms = true;
            //NewUserModel.AcceptedAt = DateTime.UtcNow;
            //NewUserModel.TermsVersion = "v1.0";
            //NewUserModel.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            _dBContext.PassengerTb.Add(NewPassengerModel);

            if (_dBContext.SaveChanges() ==1)
            {
                ViewData["Message"]= "Your details have been saved successfully!";
                return RedirectToPage("/SignIn");
            }
            else
            {
                return RedirectToPage("/Index");
            }
        }

    }
}
