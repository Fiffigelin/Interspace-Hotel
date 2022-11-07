using MySqlConnector;
using Dapper;
class OneBed : RoomBase
{
    public string Type = "oneBed";
    public int Price = 640;

    public string SearchForRoom(MySqlConnection connection)
    {
        string checkUser = connection.QuerySingle<string>($"SELECT * FROM room; ");
        return checkUser;
    }
}