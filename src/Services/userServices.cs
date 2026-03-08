using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TMS.src;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace TMS.src
{
    public interface IUserService
    {
        Task<GetUserLoginDetails> userLoginService(UserLoginDTO userLoginDTO);
        Task<GetUserRegisterDTo> userRegisterService(UserRegisterDTO userRegisterDTO);


    }
    public class UserService
    {
        private  readonly UserRepo _userRepo;
        private readonly IConfiguration _config;
        public UserService(UserRepo userRepo ,IConfiguration configuration)
        {
            _userRepo=userRepo;
            _config=configuration;
            
        }



        public async Task<GetUserRegisterDTo> userRegisterService(UserRegisterDTO userRegisterDTO)
            {
                var user=await _userRepo.GetUserByEmail(userRegisterDTO.email);
            if (user != null)
            {
                throw new ApiException("message","User already exist");
            }
                    string salt=BCrypt.Net.BCrypt.GenerateSalt(12);
                    string hashPassword=BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.password);

            var newUser=new UserModel
            {
              f_Name=userRegisterDTO.f_name,
              l_Name=userRegisterDTO.L_name,
              email=userRegisterDTO.email,  
              phone_no=userRegisterDTO.phon_no,
              role=userRegisterDTO.role,
              password=hashPassword,
            };

            var register=await _userRepo.PostRegister(newUser);
            var details=new GetUserRegisterDTo
            {
                userId=register.userId,
                f_name=register.f_Name,
                L_name=register.l_Name,
                email=register.email,
                
            };
            return details;
            }

           










        public async Task<GetUserLoginDetails> userLoginService(UserLoginDTO userLoginDTO)
        {
             var user=await _userRepo.GetUserByEmail(userLoginDTO.userName);

            // if (user == null)
            // {
            //     user=await _userRepo.GetUserByNumber(userLoginDTO.userName);
            // }

            if (user == null)
            {
                throw new ApiException("email","invalid email address or Phone number ");
            }
            var isPasswordValid=BCrypt.Net.BCrypt.Verify(userLoginDTO.password,user.password);
            if (!isPasswordValid)
            {
                throw new ApiException("password","invalid your password");
            }
            var generatedToken = GenerateJwtToken(user);

            var userDetail=new GetUserLoginDetails
            {
                userId=user.userId,
                f_name=user.f_Name,
                L_name=user.l_Name,
                email=user.email,
                role=user.role,
                is_active=user.is_active,
                is_available=user.is_available,
                token=generatedToken
            };
            return userDetail;
        }

        private string GenerateJwtToken(UserModel user)
{
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    // Claims mein wo details rakhen jo aapko frontend par chahiye
    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.userId.ToString()),
        new Claim(ClaimTypes.Email, user.email),
        new Claim(ClaimTypes.Role, user.role ?? "User"),
        new Claim("FirstName", user.f_Name ?? "")
    };

    var token = new JwtSecurityToken(
        issuer: _config["Jwt:Key"],
        audience: _config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddHours(3), // Token 3 ghante baad expire hoga
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
}


    }
}