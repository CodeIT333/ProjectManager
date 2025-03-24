using Domain.Customers;

namespace UnitTest.Commons
{
    internal class TestableCustomer : Customer
    {
        public TestableCustomer(string name, string phone, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Phone = phone;
            Email = email;
        }
    }
}
