using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Quiztle.CoreBusiness
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
            Claims =
            [
                new Claim("name", Name),
                new Claim("email", Email),
                new Claim("role", Roles.RegularUser),
                new Claim("permission", "regular_user")
            ];
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string? PhoneNumber { get; set; }
        public string Password { get; set; } = "";

        [NotMapped]
        [JsonIgnore]
        public List<Claim> Claims { get; set; }
        public string Role { get; set; } = Roles.RegularUser;
    }
}
