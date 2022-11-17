using Dapper;
using MySqlConnector;
class RoomManagement
{
    public RoomDB db;
    public RoomManagement(RoomDB connection)
    {
        db = connection;
    }

    // public void CreateRoom(List<Room> roomList)
    // {
    //     //hotelRooms.Add(hotelRooms.Count + 1, room);
    //     foreach (var room in roomList)
    //     {
    //         db.CreateRoom(room);
    //     }
    // public string UpdateRoom(int price, int beds, int size, int id)
    // {
    //     if (IsPriceValid(price.ToString())) return "Invalid input";
    // }
    public bool IsPriceValid(string input)
    {
        foreach (char chars in input)
        {
            if (chars < '0' || chars > '9')
                return false;
        }
        return true;
    }
    public bool IsBedsInputValid(string beds)
    {
        if (string.IsNullOrEmpty(beds))
        {
            return false;
        }
        return true;
    }
}
