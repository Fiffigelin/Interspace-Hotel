using MySqlConnector;
using Dapper;
internal class Program
{
    private static void Main(string[] args)
    {
        OneBed test = new();
        Customer cust = new();
        List<Customer> c = new();

        Console.WriteLine(test.Price);
        Console.WriteLine(test.SearchForRoom(Connector.Connect()));
        
        c = cust.SearchForCustomer(Connector.Connect());
        foreach (var identification in c)
        {
            Console.WriteLine($"{identification}");
        }
    }
}