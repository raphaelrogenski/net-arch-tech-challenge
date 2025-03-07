namespace NetArchTechChallenge.Persistence
{
    public class Program
    {
        public static async void Main(string[] args)
        {
            var application = new Application();
            await application.GetApplication(args).RunAsync();
        }
    }
}