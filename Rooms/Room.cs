class Room
{
    public int id { get; set; }
    public string name { get; set; }
    public int price { get; set; }
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
        @$" Room : {name}
        Roomid : {id}
        Price : {price}
        Beds : {beds}
        Roomsize : {size} kvm
        Max amount guests : {guests}";
    }
}