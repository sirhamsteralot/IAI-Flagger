using System.Security;

namespace Flagger.Models
{
    public class User
    {
        public enum UserType
        {
            Plebian,
            Almighty
        };

        public UserType Type { get; set; }
        public string SessionToken { get; set; }
    }
}
