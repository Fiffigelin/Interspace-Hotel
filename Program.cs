using MySqlConnector;
using Dapper;
internal class Program
{
    private static void Main(string[] args)
    {
        List<Room> fakeRooms = new List<Room>();

        Room oneBed = new(840, 1, 12, "oneBed", 1);
        Room twoBed = new(1000, 1, 24, "Twobed", 2);
        Room deluxe = new(3500, 2, 55, "Deluxe suite", 4);
        Room pentHouse = new(6400, 4, 150, "Penthouse suite", 6);

        fakeRooms.Add(oneBed);
        fakeRooms.Add(twoBed);
        fakeRooms.Add(deluxe);
        fakeRooms.Add(pentHouse);

        foreach (Room room in fakeRooms)
        {
            Console.WriteLine(room);
        }


        List<Customer> fakeCustomer = new();
        fakeCustomer.Add(new Customer());
        //SearchForCustomer(Connector.Connect());
    }


}