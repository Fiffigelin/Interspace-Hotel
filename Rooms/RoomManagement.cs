using Dapper;
using MySqlConnector;
class RoomManagement
{
    public RoomDB db;
    public RoomManagement(RoomDB connection)
    {
        db = connection;
    }

    public string CreateRoom(string name, int price, int beds, int size, int guests) // ska guests vara med?
    {
        int id;
        if (!IsStringValid(name)) return "Invalid input of name";
        if (!IsInt(price)) return "Invalid input of price";
        if (!IsInt(beds)) return "Invalid input of beds";
        if (!IsDouble(size)) return "Invalid input of size";
        if (!IsInt(guests)) return "Invalid input of guests";
        try
        {
            // ska vi skicka in dem som objekt? Eller i string eller int-variabler? 
            // id = db.CreateRoom(room);
            id = db.CreateRoom(name, price, beds, size);
        }
        catch (System.Exception)
        {

            return "ERROR ADDING ROOM";
        }
        return $"NEW ROOM CREATED WITH ID : {id}";

    }

    public string UpdateRoom(string name, int price, int beds, int size, int guests, int id)
    {
        if (!IsStringValid(name)) return "Invalid input of name";
        if (!IsInt(price)) return "Invalid input of price";
        if (!IsInt(beds)) return "Invalid input of beds";
        if (!IsDouble(size)) return "Invalid input of size";
        if (!IsInt(guests)) return "Invalid input of guests";

        // Ska vi kunna uppdatera rumsnamnen? 
        // Room room = new(name, price, beds, size, guests);
        try
        {
            db.UpdateRoom(price, beds, size, id);
        }
        catch (System.Exception)
        {

            return $"ERROR ADDING ROOM";
        }

        return $"NEW ROOM CREATED WITH ID : {id}";
    }

    public List<Room> PrintRooms()
    {
        return db.GetRooms();
    }

    public string DeleteRoom(int id)
    {
        try
        {
            db.DeleteRoom(id);
            return $"ROOM WITH ID : {id} REMOVED";
        }
        catch (System.Exception)
        {
            return $"NO ROOM FOUND WITH ID : {id}";
        }
    }
    private bool IsDouble(double d)
    {
        try
        {
            Convert.ToDouble(d);
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }
    private bool IsInt(int i)
    {
        try
        {
            Convert.ToInt32(i);
            return true;
        }
        catch (System.Exception)
        {

            return false;
        }
    }
    private bool IsStringValid(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        return true;
    }

}