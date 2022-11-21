class ReservationManager
{
    public ReservationDB reservationDB;
    public ReservationManager(ReservationDB connection)
    {
        reservationDB = connection;
    }

    public int CalculateTotalCost(Reservation reservation, Room room, int guests)
    {
        int economy = ((room.price * reservation.duration) * guests);
        reservation.economy = economy;
        return reservation.economy;
    }

    public int Reservation (Reservation reservation)
    {
        return reservationDB.CreateRoomReservation(reservation);
    }

    public List<Reservation> GetReservationByString(string search)
    {
        return reservationDB.SearchReservationByString(search);
    }

    public List<Reservation> GetReservationByCustomerID(List<Reservation> reservation)
    {
        return reservationDB.SelectReservations(reservation);
    }

    public Reservation GetReservationsByID(int id)
    {
        return reservationDB.GetReservationById(id);
    }
}