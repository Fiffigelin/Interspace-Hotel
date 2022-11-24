using Dapper;
using MySqlConnector;

class ReservationDB
{
    MySqlConnection _sqlconnection;
    public ReservationDB(MySqlConnection connection)
    {
        _sqlconnection = connection;
    }

    public int CreateRoomReservation(Reservation reserv)
    {
        string sql = @$"INSERT INTO reservation(room_id,customer_id,date_in,duration,economy)
        VALUES({reserv.room_id}, {reserv.customer_id}, '{reserv.date_in.ToString("yyyy-MM-dd")}', {reserv.duration}, {reserv.economy});SELECT LAST_INSERT_ID();";
        int reservation = _sqlconnection.QuerySingle<int>(sql);
        return reservation;
    }

    public List<Reservation> ListReservations()
    {
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

    public List<Reservation> SelectReservations(List<Reservation> reservList)
    {
        // https://stackoverflow.com/questions/14171794/how-to-retrieve-data-from-a-sql-server-database-in-c
        Reservation reservation = new();
        string sql = string.Empty;
        foreach (var item in reservList)
        {
            sql = $@"SELECT * FROM reservation WHERE reservation.customer_id= {item.customer_id}";
        }
        MySqlCommand cmd = new MySqlCommand(sql, _sqlconnection);
        _sqlconnection.Open();
        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                reservation.id = Convert.ToInt32(reader["id"].ToString());
                reservation.room_id = Convert.ToInt32(reader["room_id"].ToString());
                reservation.customer_id = Convert.ToInt32(reader["customer_id"].ToString());
                reservation.date_in = Convert.ToDateTime(reader["date_in"].ToString());
                reservation.duration = Convert.ToInt32(reader["duration"].ToString());
                reservation.economy = Convert.ToInt32(reader["economy"].ToString());
            }
            _sqlconnection.Close();
        }
        List<Reservation> objectReserv = new();
        objectReserv.Add(reservation);
        return objectReserv;
    }
}