using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Domain.ReportGenerator.Function;

[Table("TableA32", Schema="ReportGenerator")]
public class TableA32
{
    [MaxLength(10)]
    public string Qualification { get; set; }

    [Precision(2, 2)]
    public decimal Percentage { get; set; }
}