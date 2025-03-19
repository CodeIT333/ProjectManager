using Domain.Commons;

namespace UnitTest.Commons
{
    internal class TestableAddress : Address
    {
        public TestableAddress(string country, string zipCode, string count, string settlement, string street, string houseNumber, int? door = null)
        {
            Country = country;
            ZipCode = zipCode;
            County = count;
            Settlement = settlement;
            Street = street;
            HouseNumber = houseNumber;
            Door = door;
        }
    }
}
