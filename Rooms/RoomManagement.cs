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
}