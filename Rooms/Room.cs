class Room
{
    public string name { get; set; }
    public int price { get; }
    public int beds { get; set; }
    public double size { get; set; }
    public int guests { get; set; }

    // public Room(int price, int bed, double size, int guests)
    // {
    //     this.price = price;
    //     this.beds = bed;
    //     this.size = size;
    //     this.guests = guests;
    // }

    public override string ToString()
    {
        return
        @$" Room : {name}
        Roomid :
        Price : {price}
        Beds : {beds}
        Roomsize : {size} kvm
        Max amount guests : {guests}";
    }
}