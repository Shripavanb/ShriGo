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

        //Constructor
        public RiderProfileModel(RideDBContext context)
        {
            _dbContext = context;
        }

        public void OnGet()
        {
            string userValue = HttpContext.Session.GetString("UserName");

            listUserModel = _dbContext.UserTb.ToList();
            foreach(var user in listUserModel)
            {
                int index = listUserModel.FindIndex(a => a.UserFirstName == userValue);
                if (user.UserFirstName ==userValue)
                {
                    //Major Milestone in achiving only wanted list out of selected index
                    activeUser.Add(listUserModel[index]);
                 }
             
            }
        }
    }
}
