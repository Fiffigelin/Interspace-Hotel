using MySqlConnector;
using Dapper;
class OneBed : RoomBase
{
    public string Type = "oneBed";
    public int Guests = 1;
    public int Price = 640;

    public string SearchForRoom(MySqlConnection connection)
    {
        string checkUser = connection.QuerySingle<string>($"SELECT * FROM room; ");
        return checkUser;
    }

    public override string ToString()
    {
        return $"This is a simple one-bed room. No pets allowed";
    }
}