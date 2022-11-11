using Dapper;
using MySqlConnector;

class HotelDB
{
    MySqlConnection _sqlConnection;

    public HotelDB(MySqlConnection connection)
    {
        _sqlConnection = connection;
    }

    public void AddReview()
    {
        //Do SQL Magic
        //_sqlConnection.query osv osv..
    }

    // Om en användare ska lämna en review så görs det efter en vistelse.
    // Efter detta får man "länken" för att kunna ge ett betyg. Betyget samlas i en ***
    // Uträkning via en lista = samlar olika värden / antal röster. 

    // Ska alltid skrivas ut inför bokningen eller vart ska det synas? 
}