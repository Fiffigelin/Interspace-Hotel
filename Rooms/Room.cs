class Room
{
    public string name { get; set; }
    public int price { get; }
    public int beds { get; set; }
    public double size { get; set; }
    public int guests { get; set; }

    // public Room(int beds, double size, int guests, int id)
    // {
    //     ID = id;
    //     Beds = beds;
    //     Size = size;
    //     Guests = guests;
    // }

    public override string ToString()
    {
        return
        @$" Room : {Name}
        Roomid : {ID}
        Price : {Price}
        Beds : {Beds}
        Roomsize : {Size} kvm
        Max amount guests : {Guests}";
    }
}