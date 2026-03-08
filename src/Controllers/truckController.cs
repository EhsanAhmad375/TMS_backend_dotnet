using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace TMS.src
{
    [Route("api/truck/[controller]")]
    [ApiController]
    public class TruckController : ControllerBase
    {
        private readonly ITruckService _truckService;
        public TruckController(ITruckService truckService)
        {
            _truckService=truckService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<TruckCreatedDTO>> truckRegister([FromBody] CreateTruckDTO create)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400,new {success=false,error=ModelState});
            }
            try
            {
                return await _truckService.createTruckService(create);
            }catch(ApiException ex)
            {
                return StatusCode(400,new {success=false,error=new Dictionary<string,string>{{ex.FieldName,ex.Message}}});
            }catch(Exception ex)
            {
                return StatusCode(400,new {success=false, message="internal server error",error=ex});
            }
        }



        [HttpGet("trucklist")]
        public async Task<IActionResult> getTruckList()
        {
            try{
            var truckList=await _truckService.GetAllTruckListService();
            return StatusCode(200,new {success=true, message="truck list retrive successfully",data=truckList});
            }
            catch (Exception e)
            {
                return BadRequest(new{success=false,message="interna server error",error=e.Message});
            }

        }


        
        [HttpGet("truck/{id}")] 
        public async Task<IActionResult> getTruckDetails([FromRoute] int id)
        {
        try
        {
            var truckDetails = await _truckService.getTruckByIdService(id); 
            return Ok(new { 
            success = true, 
            message = "Truck Details retrieved successfully", 
            data = truckDetails 
            });
            }
            catch (ApiException ex)
            {
            return BadRequest(new { 
            success = false, 
            error = new Dictionary<string, string> { { ex.FieldName, ex.Message } } 
            });
            }
        catch (Exception e)
        {
        return StatusCode(500, new { 
            success = false, 
            message = "Internal server error", 
            error = e.Message 
        });
    }
}








    }
}