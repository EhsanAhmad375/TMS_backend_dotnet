using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace TMS.src
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboard;

        public DashboardController(IDashboardService dashboard)
        {
            _dashboard=dashboard;
        }
        



        
        [HttpGet("get-dashboard")]
        public async Task<ActionResult> getDashboard([FromQuery] string? date)
        {
            try
            {
                var dashboardData=await _dashboard.getDashbaordService(date);
                return StatusCode(200,new {success=true, message="Dashboard data retrive successfully", data=dashboardData});
            }catch(Exception ex)
            {
                return BadRequest(new {success=false, message=ex.Message});
            }
        }

    }
}
