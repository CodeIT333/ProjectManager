using Domain.Commons.Models;
using System.ComponentModel.DataAnnotations;

namespace Domain.Commons
{
    public class Address : ValueObject
    {
        [Required, MaxLength(50)]
        public string Country { get; set; }
        [Required, MaxLength(10)]
        public string ZipCode { get; set; }
        [Required, MaxLength(50)]
        public string County { get; set; }
        [Required, MaxLength(100)]
        public string Settlement { get; set; }
        [Required, MaxLength(50)]
        public string Street { get; set; }
        [Required, MaxLength(10)]
        public string HouseNumber { get; set; } // 13/a, 24.
        public int? Door { get; set; } // 11

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Country;
            yield return ZipCode;
            yield return County;
            yield return Settlement;
            yield return Street;
            yield return HouseNumber;
            if (Door != null) yield return Door;
        }
    }
}
