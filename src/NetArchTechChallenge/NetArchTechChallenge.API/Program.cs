namespace NetArchTechChallenge.API
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var application = new Application();
            await application.GetApplication(args).RunAsync();
        }
    }
}