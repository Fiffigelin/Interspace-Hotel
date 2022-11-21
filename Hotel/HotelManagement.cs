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
    string stars = string.Empty;

// Visar medelvärdet i betyget på hotellet.
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
// Denna ska användas under header för att visa antalet stjärnor, använder sig av GetValues().
    public string ShowStars()
    {
        if (calculation == 1)
        return stars = "★";
        else if (calculation == 2)
        return stars = "★★";
        else if (calculation == 3)
        return stars = "★★★";
        else if (calculation == 4)
        return stars = "★★★★";
        else if (calculation == 5)
        return stars = "★★★★★";

        return calculation.ToString();
    }

}

    /* För att hantera reviews just nu
    Console.WriteLine(hotelManagement.GetValues());
    hotelDB.AddReview(25);
    
            Console.WriteLine("Vänligen ange antalet stjärnor. ");
        int userStars = Convert.ToInt32(Console.ReadLine());
        hotelDB.AddReview(userStars);
    */

        // HotelDB hotelDB = new(connection);
        // HotelManagement hotelManagement = new(hotelDB);