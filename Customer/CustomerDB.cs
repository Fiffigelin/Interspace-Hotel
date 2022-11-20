using Dapper;
using MySqlConnector;

// FRÅGA KRISTER : vill man uppdatera de unika id:n vid en delete? Vad kan det få för konsekvenser?
class CustomerDB
{
    private MySqlConnection _sqlConnection;
    public CustomerDB()
    {
        Connect();
    }

    public void Connect()
    {
        _sqlConnection = new MySqlConnection("Server = localhost;Database = interspace_hotel;Uid=root");
    }

    public int InsertCustomer(Customer cu)
    {
        string sql = $@"INSERT INTO customer 
            (email, name, phonenumber)
            VALUES ('{cu.Email}', '{cu.Name}', '{cu.Phonenumber}');
            SELECT LAST_INSERT_ID()";

        int id = _sqlConnection.QuerySingle<int>(sql);
        return id;
    }

    public List<Customer> GetCustomers()
    {
        var customerList = _sqlConnection.Query<Customer>($@"SELECT * FROM customer").ToList();
        return customerList;
    }

    public Customer SelectCustomer(int id)
    {
        // https://stackoverflow.com/questions/14171794/how-to-retrieve-data-from-a-sql-server-database-in-c
        Customer cu = new();
        string sql = $@"SELECT * FROM customer WHERE customer.id= {id}";
        MySqlCommand cmd = new MySqlCommand(sql, _sqlConnection);
        _sqlConnection.Open();
        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                cu.ID = Convert.ToInt32(reader["id"].ToString());
                cu.Email = reader["email"].ToString();
                cu.Name = reader["name"].ToString();
                cu.Phonenumber = reader["phonenumber"].ToString();
            }
            _sqlConnection.Close();
        }
        return cu;
    }

    public void UpdateCustomer(Customer cu)
    {
        string sql = $@"UPDATE customer SET
            (email, name, phonenumber)
            VALUES ('{cu.Email}', '{cu.Name}', '{cu.Phonenumber}'
            WHERE id = {cu.ID});
            SELECT LAST_INSERT_ID()";

        int id = _sqlConnection.QuerySingle<int>(sql);
    }

    public void DeleteCustomer(int id)
    {
        int deleteID = _sqlConnection.Execute($@"DELETE FROM customer WHERE customer.id = {id}");
    }

    public List<Customer> SearchCustomerDB(string search)
    {
        var customerList = _sqlConnection.Query<Customer>($@"
        SELECT * FROM customer 
        WHERE email LIKE '%{search}%'
        OR name LIKE '%{search}%'
        OR phonenumber LIKE '%{search}%'").ToList();
        return customerList;
    }

    public int CustomerIDFromReservation(int id)
    {
        return _sqlConnection.Execute($@"SELECT reservation.customer_id FROM reservation WHERE reservation.id = '{id}'");
    }

    public Customer GetCustomerByReservation(int id)
    {
        Customer cu = new();
        string sql = ($@" SELECT * FROM customer
        LEFT JOIN reservation
        ON reservation.customer_id = customer.id
        WHERE reservation.id LIKE '%{id}%'");
        _sqlConnection.Open();
        MySqlCommand cmd = new MySqlCommand(sql, _sqlConnection);
        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                cu.ID = Convert.ToInt32(reader["id"].ToString());
                cu.Email = reader["email"].ToString();
                cu.Name = reader["name"].ToString();
                cu.Phonenumber = reader["phonenumber"].ToString();
            }
            _sqlConnection.Close();
            return cu;
        }
    }
}