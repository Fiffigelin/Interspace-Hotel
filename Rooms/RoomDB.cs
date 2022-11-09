using Dapper;
using MySqlConnector;

class RoomDB
{
    MySqlConnection _sqlConnection;

    public RoomDB(MySqlConnection connection)
    {
        _sqlConnection = connection;
    }

    public void CreateRoom(Room room)
    {
        //Do SQL Magic
        //_sqlConnection.query osv osv..
    }
}