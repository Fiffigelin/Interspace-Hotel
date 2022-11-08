using MySqlConnector;
using Dapper;
internal class Program
{
    private static void Main(string[] args)
    {
        List<Room> fakeRooms = new List<Room>();
        TestRooms(fakeRooms);
        TestCustomers();

        foreach (Room room in fakeRooms)
        {
            Console.WriteLine(room);
        }

        //SearchForCustomer(Connector.Connect());
    }

    private static void TestCustomers()
    {
        List<Customer> fakeCustomer = new();
        fakeCustomer.Add(new Customer("Johan", "johan@exempel.com", "0700102030"));
        fakeCustomer.Add(new Customer("Emelie", "emelie@exempel.com", "0710203040"));
        fakeCustomer.Add(new Customer("Elin", "elin@exempel.com", "0720304050"));

        foreach (Customer customer in fakeCustomer)
        {
            Console.WriteLine(customer);
        }
    }

    private static void TestRooms(List<Room> fakeRooms)
    {
        Room oneBed = new(840, 1, 12, "oneBed", 1);
        Room twoBed = new(1000, 1, 24, "Twobed", 2);
        Room deluxe = new(3500, 2, 55, "Deluxe suite", 4);
        Room pentHouse = new(6400, 4, 150, "Penthouse suite", 6);

        fakeRooms.Add(oneBed);
        fakeRooms.Add(twoBed);
        fakeRooms.Add(deluxe);
        fakeRooms.Add(pentHouse);
    }
}