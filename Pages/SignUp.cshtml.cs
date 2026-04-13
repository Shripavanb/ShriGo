using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Twilio;
using Twilio.Rest.Verify.V2.Service;

namespace ShriGo.Pages
{
    public class SignUpModel : PageModel
    {
        private const string smsSent = "Success";
        private readonly RideDBContext _dBContext;


        public List<UserModel> listUserModel = new List<UserModel>();

        [BindProperty]
        public UserModel NewUserModel { get; set; }

        private static readonly Random _random = new Random();
        public int UniqueNumber {  get; set; }

        //public PhoneVerify Twilo(string phone)
        //{
        //    if (string.IsNullOrEmpty(phone))
        //    {
        //        return "Invalid"; // Path 1
        //    }
        //    var accountSid = "AC20a782fc1473c3682b6481adc266e7c9";
        //    var authToken = "[AuthToken]";
        //    TwilioClient.Init(accountSid, authToken);

        //    var verification = VerificationResource.Create(
        //        to: phone,//"+918374499001",
        //        channel: "sms",
        //        pathServiceSid: "VA632db498adb24bbc2a49be09a2fbbf73"
        //        );

        //    Console.WriteLine(verification.Sid);
        //    // Missing return here!
        //    return smsSent; // Add this

        //}

        //Constructor
        public SignUpModel(RideDBContext context)
        {
            _dBContext = context;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
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

            if(_dBContext.SaveChanges() ==1 )
            {
               
                return RedirectToPage("/SignIn");
            }
            else
            {
                return RedirectToPage("/Index");
            }

            //Twilo(NewDriverModel.DriverContact.ToString());
         
        }
    }
}
