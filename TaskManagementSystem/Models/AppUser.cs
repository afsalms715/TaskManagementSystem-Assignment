using Microsoft.AspNetCore.Identity;

namespace TaskManagementSystem.Models
{
    public class AppUser:IdentityUser
    {
        public ICollection<TaskModel> Tasks { get; set; }
    }
}
