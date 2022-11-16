class Room
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Price { get; }
    public int Beds { get; set; }
    public int Size { get; set; }
    public int Guests { get; set; }

    public Room(string name, int price, int beds, int size, int guests)
    {
        Name = name;
        Price = price;
        Beds = beds;
        Size = size;
        Guests = guests;
    }

    // MATERALIZING A OBJECT FROM DB
    public Room(int id, string name, int price, int beds, int size, int guests)
    {
        ID = id;
        Name = name;
        Price = price;
        Beds = beds;
        Size = size;
        Guests = guests;
    }

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