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

    // public void CreateRoom(List<Room> roomList)
    // {
    //     //hotelRooms.Add(hotelRooms.Count + 1, room);
    //     foreach (var room in roomList)
    //     {
    //         db.CreateRoom(room);
    //     }

}
// public void CreateRoom(List<Room> roomList)
// {
//     //hotelRooms.Add(hotelRooms.Count + 1, room);
//     foreach (var room in roomList)
//     {
//         db.CreateRoom(room);
//     }

//     //         foreach (var kvp in hotelRooms)
//     //         {
//     //             rooms.Add(kvp.Value);
//     //         }

//     public IEnumerable<Room> PrintRoom()
//     {
//         List<Room> rooms = new();

//         foreach (var kvp in hotelRooms)
//         {
//             rooms.Add(kvp.Value);
//         }

//         return rooms;
//     }
// }

//     public void DeleteRoom()
//     {

//     }

//     // public Room GetRoomByID(int id)
//     // {
//     //     return hotelRooms[id];
//     // }


// }