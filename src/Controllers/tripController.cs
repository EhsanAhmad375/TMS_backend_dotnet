using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace TMS.src
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;
        public TripController(ITripService tripService)
        {
            _tripService=tripService;
        }


        [HttpPost("createtrip")]
        public async Task<ActionResult> createTrip([FromBody] CreateTripDTO createTrip)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400,new {success=false,error=ModelState});
            }
            try
            {
                var trip=await _tripService.createTripService(createTrip);
               return StatusCode(201,new {success=true,message="Trip Created Successfully",data=new{id=trip.tripId,status=trip.tripStatus,created_at=trip.created_at}});
            }catch(Exception ex)
            {
                 return StatusCode(500,new {success=false,message="internal server error",error=ex});
            }
        }

        [HttpGet("get-trip-list")]
        public async Task<IActionResult> getAllTripController([FromQuery] string? type,[FromQuery] string? status,[FromQuery] string? truckId,[FromQuery] string? driverId,[FromQuery] string? date)
        {
            try
            {
                var trip=await _tripService.getAllTripService(type,truckId,driverId,date);
                return StatusCode(200,new{success=true,message="All trip retrive successfully",data=trip});
                
            }catch(Exception ex)
            {
                 return StatusCode(500,new {success=false,message="internal server error",error=ex});
            }
            
        }
    
    
    

    [HttpGet("get-trip/{id}")]
    public async Task<ActionResult> getTripDetailsById([FromRoute] int id)
        {
            try
            {
                var trip=await _tripService.getTripDetailsById(id);
                return StatusCode(200,new{success=true,message="Trip Retrive Successfully",data=trip});
            }catch(ApiException ex)
            {
                return StatusCode(400,new{success=false,error=new Dictionary<string,string> {{ex.FieldName,ex.Message}},});
            }catch(Exception ex)
            {
                return StatusCode(500,new{success=false,message="internal server error",error=ex.Message});
            }
            
        }
    
    
    
    }
}