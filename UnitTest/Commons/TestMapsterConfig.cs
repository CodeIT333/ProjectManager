using Application.Configurations;
using Mapster;

namespace UnitTest.Commons
{
    internal class TestMapsterConfig
    {
        private static bool _isConfigured = false;

        public static void Configure()
        {
            if (_isConfigured) return;

            TypeAdapterConfig.GlobalSettings.Scan(typeof(ConfigureApplication).Assembly);
            _isConfigured = true;
        }
    }
}
