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
        private readonly IncomeRepo _incomeRepo;
        public TripController(ITripService tripService, IncomeRepo incomeRepo)
        {
            _tripService=tripService;
            _incomeRepo=incomeRepo;
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
                 return StatusCode(500,new {success=false,message="internal server error",error=ex.Message,stackTrace = ex.StackTrace, detail = ex.InnerException?.Message});
            }
        }

        [HttpGet("get-trip-list")]
        public async Task<IActionResult> getAllTripController([FromQuery] string? type,[FromQuery] string? status,[FromQuery] string? truckId,[FromQuery] string? driverId,[FromQuery] string? date,string? clientName)
        {
            try
            {
                var trip=await _tripService.getAllTripService(type,truckId,driverId,date,clientName);
                return StatusCode(200,new{success=true,message="All trip retrive successfully",data=trip});
                
            }catch(Exception ex)
            {
                 return StatusCode(500,new {success=false,message="internal server error",error=ex});
            }
            
        }
    
    
    

    [HttpGet("get-trip/{id}")]
    public async Task<IActionResult> getTripDetailsById([FromRoute] int id)
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
    
    
    

    [HttpGet("get-all-trip-status")]
    public async Task<ActionResult> getAllTripStatus()
{
    try
    {
        // Service ab list return karega
        var tripStatus = await _tripService.getAllTripStatus();
        
        return StatusCode(200, new { 
            success = true, 
            message = "Trip Status Retrieved Successfully", 
            data = tripStatus // Ab yahan poori array aayegi
        });
    }
    catch(Exception ex)
    {
        return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
    }
}

    [HttpPut("update-trip-status")]
    public async Task<ActionResult> updateTripStatus([FromBody] updateStatusOfTripDTO dTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400,new {success=false,error=ModelState});
                }

                var isUpdated=await _tripService.UpdateTripStatusService(dTO.tripId,dTO.statusId);
                if(isUpdated)                {
                    return StatusCode(200,new {success=true,message="Trip status updated successfully"});
                }else
                {                   
                    return StatusCode(400,new {success=false,message="Failed to update trip status"});
                }
            }catch(Exception ex)
            {
                return StatusCode(500, new {success=false,message="internal server error",error=ex.Message});
            }

        }


    

    [HttpPut("update-trip-location")]
    public async Task<ActionResult> updateTripLocation([FromBody] addCurrentLocationDTO addCurrentLocationDTO)
        {
            try
            {                if (!ModelState.IsValid)
                {
                    return StatusCode(400,new {success=false,error=ModelState});}
                var isUpdated=await _tripService.addTripLocationService(addCurrentLocationDTO);
                if(isUpdated)                {
                    return StatusCode(201,new {success=true,message="Trip location updated successfully"});
                }else
                {                   
                    return StatusCode(400,new {success=false,message="Failed to update trip location"});        

                }}catch(Exception ex)
            {                return StatusCode(500, new {success=false,message="internal server error",error=ex.Message});
            }}
 
    [HttpDelete("delete-trip/{id}")]
    public async Task<ActionResult> deleteTrip([FromRoute] int id)
    {
        try
        {
            var isDeleted = await _tripService.deleteTripById(id);
            if (isDeleted)
            {
                return StatusCode(200, new { success = true, message = "Trip deleted successfully" });
            }
            else 
            {
                return StatusCode(404, new { success = false, message = "Trip not found" });
            }
        } 
        catch (Exception ex)
        {            
            return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
        }
    } 
}}