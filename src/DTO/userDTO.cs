using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TMS.src.Data;
using TMS.src;
using System.ComponentModel.DataAnnotations;

namespace TMS.src
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage ="User Name is Required")]
        public string userName{get;set;}

        [Required(ErrorMessage ="Password is Required")]
        public string password{get;set;}
    }

    public class UserRegisterDTO
    {
        
        [Required(ErrorMessage ="First name is required")]
        public string? f_name{get;set;}

        [Required(ErrorMessage ="Last name is required")]
        public string? L_name{get;set;}

        [Required(ErrorMessage ="Email address is required")]
        public string? email{get;set;}

        [Required(ErrorMessage ="Phone number is required")]
        public string? phon_no{get;set;}

        [Required(ErrorMessage ="Role is required")]
        public string? role{get;set;}

        [Required(ErrorMessage ="Password is required")]
        public string password{get;set;}="pak@12345";
    }

    public class GetUserLoginDetails
    {
        public int? userId{get;set;}
        public string? f_name{get;set;}
        public string? L_name{get;set;}
        public string? email{get;set;}
        public string? role{get;set;}
        public bool? is_active{get;set;}=false;
        public bool? is_available{get;set;}=false;
        public string? token{get;set;}
        
    }
    public class GetUserRegisterDTo
    {
       public int? userId{get;set;}
       public string? f_name{get;set;}
        public string? L_name{get;set;}
        public string? email{get;set;}
    }
    
}

