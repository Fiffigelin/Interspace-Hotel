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
        HotelDB hotelDB = new(connection);
        HotelManagement hotelManagement = new(hotelDB);

        Room listRooms = new();

        //UpdateRoom(roomDB);

        // int testInsert = roomDB.CreateRoom("Deluxe", 4500, 2, 64);
        // RemoveRoombyID(roomDB);

        // var rooms = roomDB.GetRooms();
        // foreach (Room r in rooms)
        // {
        //     Console.WriteLine(r.name + " " + r.price + " " + r.size + " " + r.beds + " " + r.guests);
        // }

    /* För att hantera reviews just nu
    Console.WriteLine(hotelManagement.GetValues());
    hotelDB.AddReview(25);
    */



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