class Reservation
{
    public int id { get; set; }
    public int room_id { get; set; }
    public int customer_id { get; set; }
    public int economy { get; set; }
    public DateTime date_in { get; set; }
    public int duration { get; set; }

    public Reservation()
    {

    }

    public Reservation(int id, int roomid, int customerid, int economy, DateTime datein, int duration)
    {
        this.id = id;
        this.room_id = roomid;
        this.customer_id = customerid;
        this.economy = economy;
        this.date_in = datein;
        this.duration = duration;
    }

    public Reservation(Customer cust, Room room, string startDate, int duration)
    {
        room_id = room.id;
        customer_id = cust.ID;
        economy = room.price;
        date_in = Convert.ToDateTime(startDate);
        this.duration = duration;
    }
}