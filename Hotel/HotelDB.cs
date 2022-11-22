using Dapper;
using MySqlConnector;

class HotelDB
{
    MySqlConnection _sqlConnection;

    public HotelDB(MySqlConnection connection)
    {
        _sqlConnection = connection;
    }

// För att lägga till ett nytt betyg.
    public void AddReview(int i)
    {
        int sql = _sqlConnection.Execute(@$"INSERT INTO reviews (value) VALUES ('{i}')");
    }
    
    public List<Hotel> PrintReview()
    {
        var values = _sqlConnection.Query<Hotel>($"SELECT value FROM reviews;").ToList();
        return values;
    }
}