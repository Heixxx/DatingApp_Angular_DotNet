using System.ComponentModel.DataAnnotations;
using API.Extensions;

namespace API.Entities;

public class AppUser{
    public int Id {get;set;}
    [Required]
    public string UserName {get;set;}
    public byte[] PasswordHash {get;set;}    
    public byte[] PasswordSalt {get;set;}
    public DateOnly DateOfBirth {get;set;}
    public string KnowAs {get;set;}
    public DateTime Created {get;set;} = DateTime.UtcNow;   //utcNow - dla każdego niezależnie od miejsca pobytu.
    public DateTime LastActive {get;set;} = DateTime.UtcNow; 
    public string Gender {get;set;}
    public string Introduction {get;set;}
    public string LookingFod {get;set;}
    public string Interest {get;set;}    
    public string City {get;set;} 
    public List<Photo> Photos{get;set;} = new List<Photo>();  // lub new();
    // public int GetAhe(){                                  //AutoMapper automatycznie rozpoznaje metodę Get
    //     return DateOfBirth.CalculateAge();
    // }
}