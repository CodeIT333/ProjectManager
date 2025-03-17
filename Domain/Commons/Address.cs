using Domain.Commons.Models;
using System.ComponentModel.DataAnnotations;

namespace Domain.Commons
{
    public class Address : ValueObject
    {
        [Required, MaxLength(50)]
        public string Country { get; protected set; }
        [Required, MaxLength(10)]
        public string ZipCode { get; protected set; }
        [Required, MaxLength(50)]
        public string County { get; protected set; }
        [Required, MaxLength(100)]
        public string Settlement { get; protected set; }
        [Required, MaxLength(50)]
        public string Street { get; protected set; }
        [Required, MaxLength(10)]
        public string HouseNumber { get; protected set; } // 13/a, 24.
        public int? Door { get; protected set; } // 11

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
