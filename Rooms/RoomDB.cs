using Dapper;
using MySqlConnector;

class RoomDB
{
    MySqlConnection _sqlConnection;

    public RoomDB(MySqlConnection connection)
    {
        _sqlConnection = connection;
    }

    public int CreateRoom(Room room)
    {
        string sql = @$"INSERT INTO room(room.name, room.price ,room.beds, room.size, room.guests) VALUES ('{room.name}', {room.price}, {room.beds}, {room.size}, {room.guests});SELECT LAST_INSERT_ID();";
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
        string sql = $"SELECT * FROM room;";
        var rooms = _sqlConnection.Query<Room>(sql).ToList();
        return rooms;
    }
    public void DeleteRoom(int ID)
    {
        string sql = $"DELETE FROM room WHERE room.id = {ID}";
        _sqlConnection.Query<Room>(sql);
    }

    public List<Room> GetAvailableRooms()
    {
        string sql = $@"SELECT * FROM room 
        WHERE room.id NOT IN
        (SELECT reservation.room_id FROM reservation)";
        var availablerooms = _sqlConnection.Query<Room>(sql).ToList();
        return availablerooms;
    }

    public List<Room> GetAvailableRooms(string startDate, string endDate)
    {
        string sql = $@"SELECT * FROM room 
                    WHERE room.id IN
                    (SELECT reservation.room_id FROM reservation WHERE 
                    ('{startDate}' < reservation.date_in AND '{endDate}' <= reservation.date_in)
                    OR ('{startDate}' >= (reservation.date_in + reservation.duration) AND '{endDate}' > (reservation.date_in + reservation.duration)))
                    UNION
                    SELECT * FROM room 
                    WHERE room.id NOT IN
                    (SELECT reservation.room_id FROM reservation)";
        var availablerooms = _sqlConnection.Query<Room>(sql).ToList();
        return availablerooms;
    }

    // TILLFÄLLIG
    public List<Room> SearchRoomDB(int search)
    {
        var customerList = _sqlConnection.Query<Room>($@"
        SELECT * FROM room WHERE guests >= '{search}'").ToList();
        return customerList;
    } // gör om till en int

    //SearchCustomerByString
    public List<Room> SearchRoomByString(string search)
    {
        var roomList = _sqlConnection.Query<Room>($@"
        SELECT * FROM customer 
        WHERE id LIKE '%{search}%'
        OR room LIKE '%{search}%'
        OR price LIKE '%{search}%'
        OR beds LIKE '%{search}%'
        OR size LIKE '%{search}%'").ToList();
        return roomList;
    }
}