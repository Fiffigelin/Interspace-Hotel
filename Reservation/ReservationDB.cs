using Dapper;
using MySqlConnector;

class ReservationDB
{
    MySqlConnection _sqlconnection;
    public ReservationDB(MySqlConnection connection)
    {
        _sqlconnection = connection;
    }

    public int CreateRoomReservation(int roomID, int customerID, DateTime dateIn, int duration, int economy)
    {
        string sql = @$"INSERT INTO reservation(room_id,customer_id,date_in,duration,economy)
        VALUES({roomID}, {customerID}, '{dateIn.ToString("yyyy-MM-dd")}', {duration}, {economy});SELECT LAST_INSERT_ID();";
        int reservation = _sqlconnection.QuerySingle<int>(sql);
        return reservation;
    }
    public List<Reservation> ListReservations()
    {
        // behöver ses över med lösning för parse customer_to_room.date_in så programmet inte kraschar
        var reservations = _sqlconnection.Query<Reservation>(@$"SELECT * FROM reservation;").ToList();
        return reservations;
    }

    public Reservation GetReservationById(int id)
    {
        string sql = @$"SELECT * FROM reservation WHERE reservation.id = {id};";
        var reservation = _sqlconnection.QuerySingle<Reservation>(sql);
        return reservation;
    }

    public void UpdateReservation(Reservation reservation)
    {
        string sql = @$"UPDATE reservation SET reservation.room_id = {reservation.room_id}, reservation.date_in = '{reservation.date_in}',
        reservation.duration = {reservation.duration}, reservation.economy = {reservation.economy} WHERE reservation.id = {reservation.id};";
        _sqlconnection.Execute(sql);
    }
    public void DeleteReservation(int ID)
    {
        string sql = @$"DELETE FROM reservation WHERE reservation.id = {ID};";
        _sqlconnection.Query<Reservation>(sql);
    }

    public List<Reservation> SearchReservationByString(string search)
    {
        var reservationList = _sqlconnection.Query<Reservation>($@"
        SELECT * FROM reservation
        LEFT JOIN customer
        ON reservation.customer_id = customer.id
        LEFT JOIN room
        ON reservation.room_id = room.id
        WHERE customer.name LIKE '%{search}%'
        OR customer.id LIKE '%{search}%'
        OR customer.email LIKE '%{search}%'
        OR customer.phonenumber LIKE '%{search}%'
        OR room.name LIKE '%{search}%'
        OR room.id LIKE '%{search}%'").ToList();
        return reservationList;
    }
}