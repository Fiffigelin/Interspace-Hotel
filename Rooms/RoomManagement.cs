class RoomManagement
{
    Dictionary<int, Room> hotelRooms = new();

    public void AddRoom(Room room)
    {
        hotelRooms.Add(hotelRooms.Count + 1, room);
    }
    // public Room UpdateRoom()
    // {
    //     return;
    // }
    // public Room RemoveRoom()
    // {
    //     return;
    // }
    // public Room SearchRoom()
    // {
    //     return;
    // }

    //IEnumerable interface istället för lista.
    public IEnumerable<Room> TestListInterface()
    {
        List<Room> rooms = new();

        foreach (var kvp in hotelRooms)
        {
            rooms.Add(kvp.Value);
        }

        return rooms;
    }
    public Room GetRoomByID(int id)
    {
        return hotelRooms[id];
    }

    // En sökfunktion som skriver ut lediga rum
    // Behöver avanceras så att tidsperioden som söks hanteras
    public IEnumerable<Room> GetAvailableRoom( )
    {
        List<Room> rooms = new();

        foreach (var kvp in hotelRooms)
        {
            if (kvp.Value.IsAvailable == true)
            {
                rooms.Add(kvp.Value);
            }
        }

        return rooms;
    }
    // Utgår från GetRoomByID() för att boka ett rum - få funktionen att funka
    public IEnumerable<Room> BookingRoom(int id)
    {
        List<Room> rooms = new();
        hotelRooms[id].IsAvailable = false;
        
        rooms.Add(hotelRooms[id]);
        return rooms;
    }
}