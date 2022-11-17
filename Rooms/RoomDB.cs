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
    public void UpdateRoom(Room manageRoom)
    {
        string sql = $"UPDATE room SET room.beds = {manageRoom.beds}, room.guests = {manageRoom.guests}, room.size = {manageRoom.size}, room.price = {manageRoom.price} WHERE room.id = {manageRoom.id}";
        _sqlConnection.Query<Room>(sql);
    }
    public Room GetRoomByid(int id)
    {
        string sql = @$"SELECT * FROM room WHERE room.id = {id};";
        var query = _sqlConnection.QuerySingle<Room>(sql);
        return query;
    }
    public List<Room> GetRooms()
    {
        string sql = $"SELECT price, beds, size FROM room;";
        var rooms = _sqlConnection.Query<Room>(sql).ToList();
        return rooms;
    }
    public void DeleteRoom(int ID)
    {
        string sql = $"DELETE FROM room WHERE room.id = {ID}";
        _sqlConnection.Query<Room>(sql);
    }
}