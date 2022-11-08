class Room
{
    public int ID { get; set; }
    public int Price { get; }
    public int Bed { get; set; }
    public double Size { get; set; }
    public string Type { get; }
    public int Guests { get; set; }

    public Room (int price, int bed, double size, string type, int guests)
    {
        this.Price = price;
        this.Bed = bed;
        this.Size = size;
        this.Type = type;
        this.Guests = guests;
    }

    public override string ToString()
    {
        return 
        @$"Roomid : {ID}
        Price : {Price}
        Beds : {Bed}
        Roomsize : {Size} kvm
        Roomtype : {Type}
        Max amount guests : {Guests}";
    }

    public enum StarReview
    {
        oneStar = 1,
        twoStar,
        threeStar,
        fourStar,
        fiveStar
    }
}