using MySqlConnector;
using Dapper;
internal class Program
{
    private static void Main(string[] args)
    {
        OneBed test = new();

        Console.WriteLine(test.Price);
        Console.WriteLine(test.SearchForRoom(Connector.Connect()));

    }
}