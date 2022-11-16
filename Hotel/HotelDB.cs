using Dapper;
using MySqlConnector;

class HotelDB
{
    MySqlConnection _sqlConnection;

    public HotelDB(MySqlConnection connection)
    {
        _sqlConnection = connection;
    }

    public void AddReview(int i)
    {
        int sql = _sqlConnection.Execute(@$"INSERT INTO reviews (value) VALUES ('{i}')");
    }

    public List<Hotel> PrintReview()
    {
        var values = _sqlConnection.Query<Hotel>($"SELECT value FROM reviews;").ToList();
        return values;
    }

    // Ska alltid skrivas ut inför bokningen eller vart ska det synas? 
    // Skapa nya röster genom metod för användaren. Denna ska in i UI. 
    // Fixa felhantering. 
}