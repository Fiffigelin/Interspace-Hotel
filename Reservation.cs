using Dapper;
using MySqlConnector;

class Reservation
{
    // // En sökfunktion som skriver ut lediga rum
    // // Behöver avanceras så att tidsperioden som söks hanteras
    // public IEnumerable<Room> GetAvailableRoom( )
    // {
    //     List<Room> rooms = new();

    //     foreach (var kvp in hotelRooms)
    //     {
    //         if (kvp.Value.IsAvailable == true)
    //         {
    //             rooms.Add(kvp.Value);
    //         }
    //     }

    //     return rooms;
    // }

    // Utgår från GetRoomByID() för att boka ett rum - få funktionen att funka'
    // public IEnumerable<Room> BookingRoom(int id)
    // {
    //     List<Room> rooms = new();
    //     hotelRooms[id].IsAvailable = false;

    //     rooms.Add(hotelRooms[id]);
    //     return rooms;
    // }
}