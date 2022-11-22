using Dapper;
using MySqlConnector;
class RoomManagement
{
    public RoomDB roomDB;
    public RoomManagement(RoomDB connection)
    {
        roomDB = connection;
    }

    public List<Room> GetAvailableRoomsForBooking(int guests, string startDate, string endDate)
    {
        List<Room> roomList = roomDB.GetAvailableRooms(startDate, endDate);
        List<Room> modifiedList = new();
        foreach (var room in roomList)
        {
            if (guests <= room.guests)
            {
                modifiedList.Add(room);
            }
        }
        return modifiedList;
    }

    public Room GetRoomByID(int id)
    {
        return roomDB.GetRoomByid(id);
    }

    public List<Room> GetRooms()
    {
        return roomDB.GetRooms();
    }
}