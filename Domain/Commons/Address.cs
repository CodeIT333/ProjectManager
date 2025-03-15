using Domain.Commons.Models;

namespace Domain.Commons
{
    public class Address : ValueObject
    {
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string County { get; set; }
        public string Settlement { get; set; }
        public string Street { get; set; }
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
