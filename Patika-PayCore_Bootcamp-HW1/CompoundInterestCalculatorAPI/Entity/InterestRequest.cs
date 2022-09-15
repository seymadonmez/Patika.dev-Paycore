using System.ComponentModel.DataAnnotations;

namespace CompoundInterestCalculatorAPI.Entity
{
    public class InterestRequest
    {
        [Required]
        public double Capital { get; set; }
        [Required]
        public double InterestRate { get; set; }
        [Required]
        public int DueTime { get; set; }
    }
}
