using Dapper;
using MySqlConnector;
class RoomManagement
{
    public RoomDB db;
    public RoomManagement(RoomDB connection)
    {
        db = connection;
    }

    private bool IsStringNumeric(string s)
    {
        foreach (char c in s)
        {
            if (c < '0' || c > '9')
                return false;
        }
        return true;
    }
    private bool IsEmailValid(string s)
    {
        if (string.IsNullOrEmpty(s) || !s.Contains("@"))
        {
            return false;
        }

        return true;
    }

    private bool IsStringValid(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        return true;
    }

    private bool IsCustomerListed(List<Customer> customerList)
    {
        if (customerList.Count <= 0)
        {
            return false;
        }

        return true;
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