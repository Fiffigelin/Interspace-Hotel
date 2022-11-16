using MySqlConnector;
using Dapper;
internal class Program
{
    const string CONNECTIONSTRING = "Server = localhost;Database = interspace_hotel;Uid=root";

    private static void Main(string[] args)
    {  
        MySqlConnection connection = new MySqlConnection(CONNECTIONSTRING);
        // RoomDB roomDB = new(connection);
        // RoomManagement roomManager = new(roomDB);

        CustomerDB customerDB = new();
        CustomerManagement customerManager = new(customerDB);

        Console.Write("Searchword : ");
        string search = Console.ReadLine();
        List<Customer> cuList = customerManager.StringSearchCustomer(search);
        if (cuList.Count <= 0)
        {
            System.Console.WriteLine("NOTHING HERE");
        }
        else
        {
            foreach (var item in cuList)
            {
                System.Console.WriteLine(item);
            }
        }

        Console.Write("DELETE BY ID : ");
        int id = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine(customerManager.RemoveCustomer(id));
        
        Customer cu = customerManager.GetCustomer(id);
        if (cu.ID == id)
        {
            Console.WriteLine(cu);
        }
        else
        {
            Console.WriteLine("Customer not found");
        }

        Room listRooms = new();

        
        //UpdateRoom(roomDB);

        // int testInsert = roomDB.CreateRoom("Deluxe", 4500, 2, 64);
        // RemoveRoombyID(roomDB);

        // var rooms = roomDB.GetRooms();
        // foreach (Room r in rooms)
        // {
        //     Console.WriteLine(r.name + " " + r.price + " " + r.size + " " + r.beds + " " + r.guests);
        // }

        // TestCustomers();

        // GUSTAVS TIPS
        //SqlConnect connection = kjfgkfjgkfg
        //RoomDB roomDb = new(connection)
    }

    private static void RemoveRoombyID(RoomDB roomDB)
    {
        Console.WriteLine("Skriv in ett rums id du vill ta bort");
        string idRemove = Console.ReadLine();
        int idRemoveConvert = Convert.ToInt32(idRemove);
        roomDB.DeleteRoom(idRemoveConvert);
    }


    private static void UpdateRoom(RoomDB roomDB)
    {
        Console.WriteLine("type in a price");
        string inputPrice = Console.ReadLine();
        int priceConverter = Convert.ToInt32(inputPrice);
        Console.WriteLine("type in number of beds");
        string inputBeds = Console.ReadLine();
        int bedsConverter = Convert.ToInt32(inputBeds);
        Console.WriteLine("type in size");
        string inputSize = Console.ReadLine();
        int sizeConverter = Convert.ToInt32(inputSize);
        Console.WriteLine("type in id");
        string inputID = Console.ReadLine();
        int IDConverter = Convert.ToInt32(inputID);
        roomDB.UpdateRoom(priceConverter, bedsConverter, sizeConverter, IDConverter);
    }
}