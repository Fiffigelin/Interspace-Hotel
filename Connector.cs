using MySqlConnector;
using Dapper;
class Connector
{
    public static MySqlConnection Connect()
    {
        MySqlConnection connection = new MySqlConnection("Server = localhost;Database = interspace_hotel;Uid=root");
        return connection;
    }
}