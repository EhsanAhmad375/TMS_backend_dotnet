using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TMS.src;

namespace TMS.src{
public class IncomeModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int incomeId { get; set; }
    public int? trip_id { get; set; }
    [ForeignKey("trip_id")]
    public TripModel? trip { get; set; }
    public int? income_source_id { get; set; }
    [ForeignKey("income_source_id")]
    public IncomeSource? incomeSource { get; set; }
    public double? reveived_amount { get; set; }
    public double? total_amount { get; set; }
    public double? remaining_amount { get; set; }

    public DateTime date { get; set; } = DateTime.UtcNow; 
    
    public string? notes { get; set; }
    
    public int? added_by { get; set; }

    [ForeignKey("added_by")]
    public UserModel? user { get; set; }

    public DateTime created_at { get; set; } = DateTime.UtcNow;
    public DateTime updated_at { get; set; } = DateTime.UtcNow;
}
    public class IncomeSource
    {
    [Key]
    public int id { get; set; }
    public string? name { get; set; } // e.g., Advance, Full Payment, Refund
}
}