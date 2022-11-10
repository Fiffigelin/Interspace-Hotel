using MySqlConnector;
using Dapper;
internal class Program
{
    const string CONNECTIONSTRING = "Server = localhost;Database = interspace_hotel;Uid=root";

    private static void Main(string[] args)
    {
        // ======================= INFÖR FREDAGEN =======================
        // ==  * Skriva om alla metoder som rör klassen Room så att    ==
        // ==    datan lagras med en gång i databasen                  ==
        // ==  * Skapa en tabell för bokningar i DB som innehåller     ==
        // ==    DATUM FRÅN och DATUM TILL alternativt DURATION och    ==
        // ==    room.id samt kund.id                                  ==
        // ==  * Så lätta kopplingar som möjligt!                      ==
        // ======================= INFÖR FREDAGEN =======================
        
        MySqlConnection connection = new MySqlConnection(CONNECTIONSTRING);
        RoomDB roomDB = new(connection);
        RoomManagement roomManager = new(roomDB);

        List<Room> fakeRooms = new List<Room>();

        TestRooms(fakeRooms);
        DictionaryTest(roomManager);
        //UpdateRoom(roomDB);


        while (true)
        {
            try
            {
                Console.WriteLine("type in id for room");
                string searchInput = Console.ReadLine().ToLower();
                int roomIDSearchConvert = Convert.ToInt32(searchInput);

                string searchForPerson = roomDB.FetchRoom(roomIDSearchConvert);
                Console.WriteLine($"Found {searchForPerson}");
                Thread.Sleep(1000);
            }
            catch (System.Exception)
            {
                Console.WriteLine("room doesn't exists in database.");
                Thread.Sleep(1000);
            }
        }

        // TestCustomers();

        // GUSTAVS TIPS
        //SqlConnect connection = kjfgkfjgkfg
        //RoomDB roomDb = new(connection)

        // foreach (Room room in fakeRooms)
        // {
        //     Console.WriteLine(room);
        // }

        // IEnumerable<Room> printListRooms = roomManager.PrintRoom();
        // // FÖR EN TYDLIGARE UTSKRIFT
        // Console.WriteLine("ALLA RUM");
        // foreach (Room room in printListRooms)
        // {
        //     Console.WriteLine(room);
        // }

        // Console.WriteLine("Type in a id number 1-4.");
        // string input = Console.ReadLine();
        // int converter = Convert.ToInt32(input);
        // // FÖR EN TYDLIGARE UTSKRIFT
        // Console.WriteLine("RUMMET VI SÖKTE EFTER");
        // //Console.WriteLine(roomManager.GetRoomByID(converter));

        // //printListRooms = roomManager.BookingRoom(converter);

        // // Copy paste utskrift av lediga rum
        // //printListRooms = roomManager.GetAvailableRoom();
        // // FÖR EN TYDLIGARE UTSKRIFT
        // Console.WriteLine("ALLA LEDIGA RUM");
        // foreach (Room room in printListRooms)
        // {
        //     Console.WriteLine(room);
        // }

        //SearchForCustomer(Connector.Connect());
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
        Room oneBed = new(840, 1, 12, 1);
        Room twoBed = new(1000, 1, 24, 2);
        Room deluxe = new(3500, 2, 55, 4);
        Room pentHouse = new(6400, 4, 150, 6);

        fakeRooms.Add(oneBed);
        fakeRooms.Add(twoBed);
        fakeRooms.Add(deluxe);
        fakeRooms.Add(pentHouse);
    }
    //detta är via en dictionary interface.
    private static void DictionaryTest(RoomManagement test)
    {
        Room oneBed = new(840, 1, 12, 1);
        Room twoBed = new(1000, 1, 24, 2);
        Room deluxe = new(3500, 2, 55, 4);
        Room pentHouse = new(6400, 4, 150, 6);

        test.CreateRoom(oneBed);
        test.CreateRoom(twoBed);
        test.CreateRoom(deluxe);
        test.CreateRoom(pentHouse);
    }
}