namespace Infrastructure.Exceptions
{
    public static class ErrorMessages
    {
        /*-----------------------------------------------Programmer--------------------------------------------*/
        public const string NOT_FOUND_PROGRAMMER = "The programmer was not found.";

        public const string REQUIRED_PROGRAMMER_NAME = "The programmer name is required.";
        public const string REQUIRED_PROGRAMMER_EMAIL = "The programmer email is required.";
        public const string REQUIRED_PROGRAMMER_PHONE = "The programmer phone is required.";
        public const string REQUIRED_PROGRAMMER_ADDRESS = "The programmer address is required.";
        public const string REQUIRED_PROGRAMMER_DATE_OF_BIRTH = "The programmer date of birth is required.";
        public const string REQUIRED_PROGRAMMER_ROLE = "The programmer role is required.";
        public const string REQUIRED_PROGRAMMER_IS_INTERN = "The programmer is intern is required.";

        public const string TOO_LONG_PROGRAMMER_NAME = "The programmer name is too long.";
        public const string TOO_LONG_PROGRAMMER_EMAIL = "The programmer email is too long.";
        public const string TOO_LONG_PROGRAMMER_PHONE = "The programmer phone is too long.";

        public const string TAKEN_PROGRAMMER_EMAIL = "The programmer email is already taken.";

        /*-----------------------------------------------ProjectManager--------------------------------------------*/
        public const string NOT_FOUND_PROJECT_MANAGER = "The project manager was not found.";

        /*-----------------------------------------------Project--------------------------------------------*/
        public const string NOT_FOUND_PROJECT = "The project was not found.";

        /*-----------------------------------------------Address--------------------------------------------*/
        public const string REQUIRED_ADDRESS_COUNTRY = "The address country is required.";
        public const string REQUIRED_ADDRESS_ZIP_CODE = "The address zip code is required.";
        public const string REQUIRED_ADDRESS_COUNTY = "The address county is required.";
        public const string REQUIRED_ADDRESS_SETTLEMENT = "The address settlement is required.";
        public const string REQUIRED_ADDRESS_STREET = "The address street is required.";
        public const string REQUIRED_ADDRESS_HOUSE_NUMBER = "The address house number is required.";

        public const string TOO_LONG_ADDRESS_COUNTRY = "The address country is too long.";
        public const string TOO_LONG_ADDRESS_ZIP_CODE = "The address zip ocde is too long.";
        public const string TOO_LONG_ADDRESS_COUNTY = "The address county is too long.";
        public const string TOO_LONG_ADDRESS_SETTLEMENT = "The address settlement is too long.";
        public const string TOO_LONG_ADDRESS_STREET = "The address street is too long.";
        public const string TOO_LONG_ADDRESS_HOUSE_NUMBER = "The address house number is too long.";

    }
}
