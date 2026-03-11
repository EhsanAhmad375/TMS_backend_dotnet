using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.src;


namespace TMS.src
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserService _userService;
        public UserController(UserService userService)
        {
            _userService=userService;
        }
        

        [HttpPost("login")]
        public async Task<ActionResult<GetUserLoginDetails>> userLogin([FromBody] UserLoginDTO userLogin)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new{ success = false , error = ModelState});

            }
            try
            {
                
                return await _userService.userLoginService(userLogin);
            }
            catch (ApiException ex)
            {
                return StatusCode(400,new {succsee=false,error=new Dictionary<string,string>{{ex.FieldName,ex.Message}}});
            }
            catch(Exception ex)
            {
                return StatusCode(500,new {succsee=false,message="internal server error",error=ex.Message});
            }
        }




        [HttpPost("register")]
        public async Task<ActionResult<GetUserRegisterDTo>> userRegister([FromBody] UserRegisterDTO userRegisterDTO)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400,new{success=false,error=ModelState});
            }
            try
            {
                var user=await _userService.userRegisterService(userRegisterDTO);
                return StatusCode(201,new{success=true,message="user created successfully",data=user});
            }catch(ApiException ex)
            {
                return StatusCode(400,new {success=false,error=new Dictionary<string,string>{{ex.FieldName,ex.Message}}});
            }catch(Exception ex)
            {
                return StatusCode(500,new {succsee=false,message="internal server error",error=ex.Message});
            }
        }


        

        
    }
}