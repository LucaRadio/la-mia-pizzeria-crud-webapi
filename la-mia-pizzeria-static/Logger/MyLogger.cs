namespace la_mia_pizzeria_static.Logger
{
    public class MyLogger : IMyLogger
    {
        public void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
