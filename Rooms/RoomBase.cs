using MySqlConnector;
using Dapper;
abstract class RoomBase : IRooms
{
    public int ID { get; set; }
    public int Price { get; }
    public int Bed { get; set; }
    public double Size { get; set; }
    public string Type { get; }
    public int Guests { get; set; }

    public enum StarReview
    {
        oneStar = 1,
        twoStar,
        threeStar,
        fourStar,
        fiveStar
    }

    public override string ToString()
    {
        return base.ToString();
    }
    public void RoomDescription()
    {

    }
    public void AddRoom()
    {

    }
    public void RemoveRoom()
    {

    }
    public void UpdateRoom()
    {

    }
}