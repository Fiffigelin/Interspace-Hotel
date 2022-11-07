using MySqlConnector;
using Dapper;
class OneBed : RoomBase, IRooms
{
    public string Type = "oneBed";
    public int Guests = 1;
    public int Price = 640;

    public string SearchForRoom(MySqlConnection connection)
    {
        string checkUser = connection.QuerySingle<string>($"SELECT * FROM room; ");
        return checkUser;
    }

    public void RoomDescription()
    {
        Console.WriteLine("This is a one-bed room. Guests will find them ideal for work and leisure with a work desk, No pets allowed");
    }
}