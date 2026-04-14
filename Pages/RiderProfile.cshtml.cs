using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriGo.Model;
using System.Collections;

namespace ShriGo.Pages
{
    public class RiderProfileModel : PageModel
    {
        private readonly RideDBContext _dbContext;
        public List<UserModel> listUserModel = new List<UserModel>();
        public List<UserModel> activeUser = new List<UserModel>();

        public List<RideModel> listRideModel = new List<RideModel>();
        public List<RideModel> UserRides = new List<RideModel>();

        //Constructor
        public RiderProfileModel(RideDBContext context)
        {
            _dbContext = context;
        }

        public void OnGet()
        {
            string UserSessionName = HttpContext.Session.GetString("UserName");
            string userUniqueId = HttpContext.Session.GetString("UserUniqueId");

            //User profile display
            listUserModel = _dbContext.UserTb.ToList();
            foreach(var user in listUserModel)
            {
                int index = listUserModel.FindIndex(a => a.UserFirstName == UserSessionName);
                if (user.UserFirstName ==UserSessionName)
                {
                    //Major Milestone in achiving only wanted list out of selected index
                    activeUser.Add(listUserModel[index]);
                 }
             
            }

            //user ride list display 

            listRideModel = _dbContext.RideDBTable.ToList();
            foreach (var ride in listRideModel)
            {
                int index = listRideModel.FindIndex(a => a.UserUniqueId == userUniqueId);
                if (ride.UserUniqueId ==userUniqueId)
                {                    
                    //Major Milestone in achiving only wanted list out of selected index
                    UserRides.Add(listRideModel[index]);
                }

            }


        }
    }
}
