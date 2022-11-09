using Dapper;
using MySqlConnector;
class RoomManagement
{
    Dictionary<int, Room> hotelRooms = new();

    public RoomManagement(RoomDB connection)
    {

    }

    public void CreateRoom(Room room)
    {
        hotelRooms.Add(hotelRooms.Count + 1, room);
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