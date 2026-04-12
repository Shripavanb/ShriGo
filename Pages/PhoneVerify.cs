
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


        public void twilio(string mobileNo)
        {
            var accountSid = "AC20a782fc1473c3682b6481adc266e7c9";
            var authToken = "[AuthToken]";
            TwilioClient.Init(accountSid, authToken);

            var verification = VerificationResource.Create(
                to: mobileNo,//"+918374499001",
                channel: "sms",
                pathServiceSid: "VA632db498adb24bbc2a49be09a2fbbf73"
                );

            Console.WriteLine(verification.Sid);
            //return verification;
        }


//class Example
//    {
//        static void Main(string[] args)
//        {
//            var accountSid = "AC20a782fc1473c3682b6481adc266e7c9";
//            var authToken = "[AuthToken]";
//            TwilioClient.Init(accountSid, authToken);

//            var verification = VerificationResource.Create(
//                to: "+918374499001",
//                channel: "sms",
//                pathServiceSid: "VA632db498adb24bbc2a49be09a2fbbf73"
//            );

//            Console.WriteLine(verification.Sid);
//        }
//    }


}
}
