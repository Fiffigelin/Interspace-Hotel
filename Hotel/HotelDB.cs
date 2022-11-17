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


// För att skapa en lista med alla betyg.
    public List<Hotel> PrintReview()
    {
        var values = _sqlConnection.Query<Hotel>($"SELECT value FROM reviews;").ToList();
        return values;
    }


    // Skapa nya röster genom metod för användaren. Denna ska in i UI. 
    // Fixa felhantering. (Måste vara mellan siffran 1-5)
}