class DeluxeRoom : IRooms
{
    public string Type = "Deluxe";
    public int Guests;
    public int Price = 2300;

    public void RoomDescription()
    {
        Console.WriteLine(@"This suite is equipped with elegantly furnished with handmade furniture include luxury en-suite facilities with complimentary amenities pack,
        flat screen LCD TV, tea/coffee making facilities, fan,
        hairdryer and the finest pure white linen and towels.");
    }
}