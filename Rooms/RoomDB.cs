using Dapper;
using MySqlConnector;

class RoomDB
{
    MySqlConnection _sqlConnection;

    public RoomDB(MySqlConnection connection)
    {
        _sqlConnection = connection;
    }

    public void CreateRoom(Room room)
    {
        //Do SQL Magic
        //_sqlConnection.Query<Room>($"INSERT INTO room(room.price,room.beds,room.size)VALUES {} {} {}");
    }
    public void UpdateRoom(int roomPrice, int roomBeds, int roomSize, int ID)
    {
        _sqlConnection.Query<Room>($"UPDATE room SET room.price = {roomPrice},room.beds = {roomBeds},room.size = {roomSize} WHERE id = {ID}");
    }
    public string FetchRoom(int ID)
    {
        string sql = $"SELECT price, beds, size, review FROM room WHERE room.id = {ID};";
        _sqlConnection.Query<Room>(sql);
        return sql;
    }
}