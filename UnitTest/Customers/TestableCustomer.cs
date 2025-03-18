using Domain.Customers;

namespace UnitTest.Customers
{
    internal class TestableCustomer : Customer
    {
        public TestableCustomer(string name, string phone, string email)
        {
            Name = name;
            Phone = phone;
            Email = email;
        }
    }
}
