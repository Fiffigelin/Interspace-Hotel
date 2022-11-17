using Dapper;
using MySqlConnector;

class ReservationDB
{
    MySqlConnection _sqlconnection;
    public ReservationDB(MySqlConnection connection)
    {
        _sqlconnection = connection;
    }

    public int CreateRoomReservation(int roomID, int customerID, int dateIn, int duration, int economy)
    {
        string sql = @$"INSERT INTO customer_to_room(room_id,customer_id,date_in,duration,economy)
        VALUES({roomID}, {customerID}, {dateIn}, {duration}, {economy});SELECT LAST_INSERT_ID();";
        int reservation = _sqlconnection.QuerySingle<int>(sql);
        return reservation;
    }
    public List<Reservation> ListReservations()
    {
        // behöver ses över med lösning för parse customer_to_room.date_in så programmet inte kraschar
        var reservations = _sqlconnection.Query<Reservation>(@$"SELECT customer_to_room.id, customer_to_room.room_id,
        customer_to_room.customer_id,customer_to_room.economy,customer_to_room.date_in,customer_to_room.duration FROM customer_to_room;").ToList();
        return reservations;
    }
    public void UpdateReservation(int customerID, int roomID, int dateIn, int duration, int economy)
    {
        _sqlconnection.Query<Reservation>(@$"UPDATE customer_to_room SET customer_to_room.room_id = {roomID}, customer_to_room.date_in = {dateIn},
        customer_to_room.duration = {duration}, customer_to_room.economy = {economy} WHERE customer_to_room.customer_id = {customerID};");
    }
    public void DeleteReservation(int ID)
    {
        _sqlconnection.Query<Reservation>(@$"DELETE FROM customer_to_room WHERE customer_to_room.id = {ID};");
    }

}