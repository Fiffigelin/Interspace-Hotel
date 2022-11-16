using Dapper;
using MySqlConnector;
class HotelManagement
{
    HotelDB hotelDB;
    public HotelManagement(HotelDB connection)
    {
        hotelDB = connection;
    }


    int votes = 0;
    int i = 0;
    int calculation = 0;
    int totalValue = 0;

    public int GetValues()
    {
        var reviews = hotelDB.PrintReview();
        foreach (var value in reviews)
        {
            i = value.value;
            votes++;
            totalValue += i;
            calculation = totalValue / votes;
        }
        return calculation;
    }

}

    /* För att hantera reviews just nu
    Console.WriteLine(hotelManagement.GetValues());
    hotelDB.AddReview(25);
    */

        // HotelDB hotelDB = new(connection);
        // HotelManagement hotelManagement = new(hotelDB);
