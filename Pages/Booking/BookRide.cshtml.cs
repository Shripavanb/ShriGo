using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;
using System.Reflection;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ShriGo.Pages.Booking
{
    public class BookRideModel : PageModel
    {
        private readonly RideDBContext _dbContext;

        public List<SortedRideModel> list_selectedRideModel = new List<SortedRideModel>();

        [BindProperty]
        public string lridetime { get; set; }

        [BindProperty]
        public SortedRideModel selectedRideModel{  get; set; }

        //[BindProperty]
        //public BookedRideModel bookedRideModel { get; set; }


        [BindProperty]
        public BookingsModel bookedRideModel { get; set; }

        public List<BookingsModel> list_BookingsModel = new List<BookingsModel>();

        [BindProperty]
        public List<int> SelectedIds { get; set; } // This will hold the selected values

        [BindProperty]
        public int ItemQuantity { get; set; } = 1;

        //Email 
        private readonly EmailService _emailService;

        [BindProperty]
        public string UserEmail { get; set; }
        [BindProperty]
        public string Message { get; set; }


        //constructor
        public BookRideModel(RideDBContext context, EmailService emailservice)
        {
            _dbContext=context;
            _emailService = emailservice;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var rideSelected = await _dbContext.Ride_DBTable.FirstOrDefaultAsync(e => e.RideId==id);

            if (rideSelected.RideTime!=null)
            {
                string time24 = rideSelected.RideTime.ToString();
                DateTime parsedTime = DateTime.Parse(time24);
                string amPmTime = parsedTime.ToString("hh:mm tt");
                lridetime = amPmTime;
            }
            selectedRideModel=rideSelected;
            list_selectedRideModel.Add(selectedRideModel);
            return Page();
        }


        public async Task<IActionResult> OnPost(int id, Exception ex1)
        {
            var rideSelected = await _dbContext.Ride_DBTable.FirstOrDefaultAsync(e => e.RideId==id);

            string session_userName = HttpContext.Session.GetString("session_UserName");
            string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");
            string session_UserContact = HttpContext.Session.GetString("session_UserContact");
            string session_UserEmail = HttpContext.Session.GetString("session_UserEmail");

            if (session_userName == "Guest"||session_userName ==null)
            {
                ViewData["Message"] = "Please SignUp to Book a Ride..";
                Response.Redirect("/SignUp");
            }
            else
            {
                try
                {
                    // Access the selected value via ItemQuantity
                    var result = ItemQuantity;

                    var newBookingId = _dbContext.Bookings_DBTable.Max(r => r.BookingId);

                    if (newBookingId!=null)
                    { 
                        bookedRideModel.BookingId  = newBookingId+1;
                    }

                    bookedRideModel.RideId =(rideSelected.RideId).ToString();
                    bookedRideModel.RideDate =rideSelected.RideDate;
                    bookedRideModel.RideSource =rideSelected.RideSource;
                    bookedRideModel.RideDesti =rideSelected.RideDesti;
                    bookedRideModel.RideVia =rideSelected.RideVia;
                    bookedRideModel.RideTime=rideSelected.RideTime;
                    //Booked Seats
                    bookedRideModel.BookedSeats = ItemQuantity.ToString();

                    int totalbookingamount = int.Parse(rideSelected.RidePrice);
                    bookedRideModel.RidePrice =(totalbookingamount*ItemQuantity).ToString();
                    bookedRideModel.DriverContact =rideSelected.DriverContact;
                    bookedRideModel.DriverUniqueId =rideSelected.DriverUniqueId;
                    bookedRideModel.DriverFirstName =rideSelected.DriverFirstName;

                    bookedRideModel.UserFirstName =session_userName;
                    bookedRideModel.UserUniqueId =session_UserUniqueId;
                    bookedRideModel.UserContact =session_UserContact;
                    bookedRideModel.UserEmail = session_UserEmail;
                    //store booked ride into db further use
                    _dbContext.Bookings_DBTable.Add(bookedRideModel);
                    list_BookingsModel.Add(bookedRideModel);// For Email Body
                    _dbContext.SaveChanges();

                    //string emailBody = bookedRideModel.BookedSeats+bookedRideModel.RideSource;

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<h3>Your Ride has Booked:</h3> <br /><h3>Booked Ride Details:</h3><ul>");

                    // Use GetProperties() to find all public elements
                    PropertyInfo[] properties = bookedRideModel.GetType().GetProperties();

                    foreach (PropertyInfo prop in properties)
                    {
                        string name = prop.Name;
                        if (name == "RideDate"||name == "RideSource"||name == "RideDesti"||name == "RideVia"
                            ||name == "RideTime"||name == "BookedSeats"||name == "RidePrice"|| name == "DriverContact"||name == "DriverFirstName")
                        {

                            object value = prop.GetValue(bookedRideModel) ?? "N/A"; // GetValue retrieves the actual data
                            sb.Append($"<li><b>{name}: </b> {value}</li><br />");
                        }
                    }
                    sb.Append("</ul>");
                    string emailBody = sb.ToString();
                    OnPostSendMailAsync(bookedRideModel.UserEmail,"ShriGo Booking Confirmation", emailBody);
                    //SendSms(bookedRideModel.UserContact, "RideBooked Sucessfully");

                    return RedirectToPage("/Passengers/PassengerProfile");
                }
                catch (Exception ex)
                {
                    return (IActionResult)ex1;

                }
            }

            return Page();

        }

        //Sending Mobile SMS 
        public void SendSms(string phoneNumber, string messageBody)
        {
            // Replace with your actual credentials from the Twilio Console
            string accountSid = "AC20a782fc1473c3682b6481adc266e7c9";
            string authToken = "ea781111b75e5871f3e47169962e977c";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: messageBody,
                from: new PhoneNumber("+918374499001"),
                to: new PhoneNumber(phoneNumber)
            );

            Console.WriteLine($"Message SID: {message.Sid}"); // Confirm success
        }

        //Sending email confirmation 
        public async Task<IActionResult> OnPostSendMailAsync(string Recipient, string Subject, string Body)
        {
            //if (!ModelState.IsValid) return Page();
            await _emailService.sendEmailAsync(Recipient, Subject, Body);
            TempData["Message"] = "Email sent successfully!";
            return RedirectToPage("/PassengerProfile");
        }
    }
}
