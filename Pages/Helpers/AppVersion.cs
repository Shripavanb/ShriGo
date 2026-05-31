
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static Google.Protobuf.Compiler.CodeGeneratorResponse.Types;

namespace ShriGo.Pages.Helpers
{
    public class AppVersion
    {
        public const string Version ="v1.0.0";
    }
}


//Major = big architectural changes-v2.0.0
//Minor = new feature-v1.1.0
//Patch = bug fix-v1.1.1

//Example roadmap:

//v1.0.0 → Website stable
//v1.1.0 → Ride expiry logic
//v1.2.0 → Android login
//v1.3.0 → Driver upload ride
//v1.4.0 → Passenger booking
//v1.5.0 → Notifications