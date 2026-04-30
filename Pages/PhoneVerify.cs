
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;
using Twilio.TwiML.Messaging;
using Twilio.Types;

namespace ShriGo.Pages
{
    public class PhoneVerify
    {
        private readonly IConfiguration _config;


        //Constructor
        public PhoneVerify(IConfiguration config)
        {
            _config = config;
        }

        //public void twilio(string mobileNo)
        //{
        //    var accountSid = _config["TwiloConnection.accountSid"];
        //    var authToken = _config["TwiloConnection.AuthToken"];
        //    TwilioClient.Init(accountSid, authToken);

        //    var verification = VerificationResource.Create(
        //        to: mobileNo,
        //        channel: "sms",
        //        _config["TwiloConnection.pathServiceSid"]
        //        );

        //    Console.WriteLine(verification.Sid);
        //    return verification;
        //}

    }
}
