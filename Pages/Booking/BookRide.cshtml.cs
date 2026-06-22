using Google.Api;
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
        private readonly IConfiguration _config;

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
        public int BookedSeats { get; set; } = 1;

        //Email 
        private readonly EmailService _emailService;

        [BindProperty]
        public string UserEmail { get; set; }
        [BindProperty]
        public string Message { get; set; }

   
        //constructor
        public BookRideModel(RideDBContext context, EmailService emailservice, IConfiguration config)
        {
            _dbContext=context;
            _emailService = emailservice;
            _config=config;
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
                ViewData["Message"] = "Please SignIn/SignUp to Book a Ride..";
                Response.Redirect("/SignIn");
            }
            else
            {
                try
                {
                    int convert_dbseats = int.Parse(rideSelected.RideSeats);
                    // Access the selected value via BookedSeats
                    if (rideSelected != null && convert_dbseats >= BookedSeats)
                    {
                        convert_dbseats -= BookedSeats;

                        // Optional: prevent negative values
                        if (convert_dbseats < 0)
                        {
                            convert_dbseats = 0;
                        }
                        rideSelected.RideSeats = convert_dbseats.ToString();
                        await _dbContext.SaveChangesAsync();
                    }

                    var newBookingId =
                      _dbContext.Bookings_DBTable
                          .Select(x => (int?)x.BookingId)
                          .Max() ?? 0;

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
                    bookedRideModel.BookedSeats = BookedSeats.ToString();

                    int totalbookingamount = int.Parse(rideSelected.RidePrice);
                    bookedRideModel.RidePrice =(totalbookingamount*BookedSeats).ToString();
                    bookedRideModel.DriverContact =rideSelected.DriverContact;
                    bookedRideModel.DriverUniqueId =rideSelected.DriverUniqueId;
                    bookedRideModel.DriverFirstName =rideSelected.DriverFirstName;

                    bookedRideModel.PassengerFirstName=session_userName;
                    bookedRideModel.PassengerUniqueId =session_UserUniqueId;
                    bookedRideModel.PassengerContact =session_UserContact;
                    bookedRideModel.PassengerEmail = session_UserEmail;
                    //store booked ride into db further use
                    _dbContext.Bookings_DBTable.Add(bookedRideModel);


                    NotificationModel notification =
                        new NotificationModel
                        {

                            UserUniqueId = rideSelected.DriverUniqueId,


                            Title = "Notification V1",

                            Message =
                                bookedRideModel.PassengerFirstName +
                                " booked " +
                                bookedRideModel.BookedSeats +
                                " seat(s)",

                            NotificationType = "Booking",

                            IsRead = false,

                            CreatedDate = DateTime.Now
                        };
                    Console.WriteLine("Adding notification for DriverId = " + rideSelected.DriverUniqueId);
                    _dbContext.NotificationTb.Add(notification);
                    Console.WriteLine("Notification added to EF context");

                    list_BookingsModel.Add(bookedRideModel);// For Email Body
                    await _dbContext.SaveChangesAsync();

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
                    OnPostSendMailAsync(bookedRideModel.PassengerEmail,"ShriGo Booking Confirmation", emailBody);
                    //SendSms(bookedRideModel.UserContact, "RideBooked Sucessfully");

                    return RedirectToPage("/Passengers/PassengerProfile");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
            }

            return Page();

        }

        //Sending Mobile SMS 
        public void SendSms(string phoneNumber, string messageBody)
        {
            // Replace with your actual credentials from the Twilio Console
            var accountSid = _config["TwiloConnection.accountSid"];
            var authToken = _config["TwiloConnection.AuthToken"];
   

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
