using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Twilio;
using Twilio.Rest.Verify.V2.Service;
using System.Windows;

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
        public IActionResult OnPostDriver()
        {

            //DriverId
            NewUserModel.UserId = (_dBContext.UserTb.Max(r => r.UserId))+1;

            //DriverUniqueId
            //string driverLastName = _dBContext.DriversTb.Where(x=>x.DriverId == NewDriverModel.DriverId).Select(u => u.DriverLastName).FirstOrDefault();
            // Generate a random number between 1,000,000 and 9,999,999
            UniqueNumber = _random.Next(10000, 100000);

            //If the number doesn't need to be purely numeric or short, use a GUID for guaranteed uniqueness: 
            //string uniqueId = Guid.NewGuid().ToString("N");


            NewUserModel.UserUniqueId = UniqueNumber.ToString();

            // DriverReg Date only 
            NewUserModel.UserRegDate = DateOnly.FromDateTime(DateTime.Today);

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


        public IActionResult OnPostPassenger()
        {
            //DriverId
            NewPassengerModel.PassengerId = (_dBContext.UserTb.Max(r => r.UserId))+1;

            //DriverUniqueId
            //string driverLastName = _dBContext.DriversTb.Where(x=>x.DriverId == NewDriverModel.DriverId).Select(u => u.DriverLastName).FirstOrDefault();
            // Generate a random number between 1,000,000 and 9,999,999
            UniqueNumber = _random.Next(10000, 100000);

            //If the number doesn't need to be purely numeric or short, use a GUID for guaranteed uniqueness: 
            //string uniqueId = Guid.NewGuid().ToString("N");


            NewPassengerModel.PassengerUniqueId = UniqueNumber.ToString();

            // DriverReg Date only 
            NewPassengerModel.PassengerRegDate = DateOnly.FromDateTime(DateTime.Today);

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
