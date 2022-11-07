using MySqlConnector;
using Dapper;
abstract class RoomBase
{
    public int ID { get; set; }
    public int Price { get; }
    public int Bed { get; set; }
    public double Size { get; set; }
    public string Type { get; }

    public enum StarReview
    {
        oneStar = 1,
        twoStar,
        threeStar,
        fourStar,
        fiveStar
    }
}