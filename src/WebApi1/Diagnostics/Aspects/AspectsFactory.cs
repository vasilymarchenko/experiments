namespace WebApi1.Diagnostics.Aspects
{
    public static class AspectsFactory
    {
        public static IServiceProvider ServiceProvider { get; set; }
        
        public static object GetInstance(Type type)
        {
            var service = ServiceProvider.GetService(type);
            return service;
        }
    }
}
