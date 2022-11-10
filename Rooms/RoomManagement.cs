using Dapper;
using MySqlConnector;
class RoomManagement
{
    Dictionary<int, Room> hotelRooms = new();
    public RoomDB db;
    public RoomManagement(RoomDB connection)
    {
        db = connection;
    }

    public void CreateRoom(List<Room> roomList)
    {
        //hotelRooms.Add(hotelRooms.Count + 1, room);
        foreach (var room in roomList)
        {
            db.CreateRoom(room); 
        }

    }

    public IEnumerable<Room> PrintRoom()
    {
        List<Room> rooms = new();

        foreach (var kvp in hotelRooms)
        {
            rooms.Add(kvp.Value);
        }

        return rooms;
    }

    public void UpdateRoom()
    {

    }

    public void DeleteRoom()
    {

    }

    // public Room GetRoomByID(int id)
    // {
    //     return hotelRooms[id];
    // }


}