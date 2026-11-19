

using System.ComponentModel.DataAnnotations;


using Sindvamb.Web.Shared.Models;

namespace Sindvamb.Web.Shared.Models.Account
{
    public class EditUser
    {
        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        [Required] public string Username { get; set; }

        [MinLength(6, ErrorMessage = "The Password field must be a minimum of 6 characters")]
        public string Password { get; set; }

        public EditUser()
        {
        }

        public EditUser(UserVM user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
        }
    }
}
