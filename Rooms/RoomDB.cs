using Dapper;
using MySqlConnector;

class RoomDB
{
    MySqlConnection _sqlConnection;

    public RoomDB(MySqlConnection connection)
    {
        _sqlConnection = connection;
    }

    public int CreateRoom(string name, int price, int beds, int size)
    {
        string sql = @$"INSERT INTO room(room.name, room.price ,room.beds, room.size) VALUES ('{name}', {price}, {beds}, {size});SELECT LAST_INSERT_ID();";
        System.Console.WriteLine(sql);
        int create = _sqlConnection.QuerySingle<int>(sql);
        return create;
    }
    public void UpdateRoom(int roomPrice, int roomBeds, int roomSize, int ID)
    {
        _sqlConnection.Query<Room>($"UPDATE room SET room.price = {roomPrice},room.beds = {roomBeds},room.size = {roomSize} WHERE id = {ID};");
    }
    public List<Room> GetRooms()
    {
        var rooms = _sqlConnection.Query<Room>($"SELECT price, beds, size FROM room;").ToList();
        return rooms;
    }
    public void DeleteRoom(int ID)
    {
        _sqlConnection.Query<Room>($"DELETE FROM room WHERE room.id = {ID}");
    }
}