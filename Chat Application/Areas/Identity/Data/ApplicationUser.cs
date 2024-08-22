using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Chat_Application.Models;
using Microsoft.AspNetCore.Identity;

namespace Chat_Application.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public byte[]? Image { get; set; }
    public string? connectionId { get; set; }


    public string? CurrentRoom { get; set; }
    [InverseProperty("SenderUser")]
    public ICollection<UserMessage> SenderUsers { get; set; }

    [InverseProperty("RecepientUser")]

    public ICollection<UserMessage> RecepientUsers { get; set; }


}

