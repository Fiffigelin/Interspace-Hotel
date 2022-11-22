using Dapper;
using MySqlConnector;
class CustomerDB
{
    private MySqlConnection _sqlconnection;
    public CustomerDB()
    {
        Connect();
    }

    public void Connect()
    {
        _sqlconnection = new MySqlConnection("Server = localhost;Database = interspace_hotel;Uid=root");
    }

    public int InsertCustomer(Customer customer)
    {
        string sql = $@"INSERT INTO customer 
            (email, name, phonenumber)
            VALUES ('{customer.Email}', '{customer.Name}', '{customer.Phonenumber}');
            SELECT LAST_INSERT_ID()";

        int id = _sqlconnection.QuerySingle<int>(sql);
        return id;
    }

    public List<Customer> GetCustomers()
    {
        var customerList = _sqlconnection.Query<Customer>($@"SELECT * FROM customer").ToList();
        return customerList;
    }

    public Customer SelectCustomer(int id)
    {
        // https://stackoverflow.com/questions/14171794/how-to-retrieve-data-from-a-sql-server-database-in-c
        Customer customer = new();
        string sql = $@"SELECT * FROM customer WHERE customer.id= {id}";
        MySqlCommand cmd = new MySqlCommand(sql, _sqlconnection);
        _sqlconnection.Open();
        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                customer.ID = Convert.ToInt32(reader["id"].ToString());
                customer.Email = reader["email"].ToString();
                customer.Name = reader["name"].ToString();
                customer.Phonenumber = reader["phonenumber"].ToString();
            }
            _sqlconnection.Close();
        }
        return customer;
    }

    public int UpdateCustomer(Customer customer)
    {
        string sql = $@"UPDATE customer SET customer.email = '{customer.Email}',
        customer.name = '{customer.Name}',
        customer.phonenumber = '{customer.Phonenumber}'
        WHERE id = '{customer.ID}';
        SELECT LAST_INSERT_ID()";

        return _sqlconnection.QuerySingle<int>(sql);
    }

    public void DeleteCustomer(int id)
    {
        int deleteID = _sqlconnection.Execute($@"DELETE FROM customer WHERE customer.id = {id}");
    }

    public List<Customer> SearchCustomerDB(string search)
    {
        var customerList = _sqlconnection.Query<Customer>($@"
        SELECT * FROM customer 
        WHERE email LIKE '%{search}%'
        OR name LIKE '%{search}%'
        OR phonenumber LIKE '%{search}%'").ToList();
        return customerList;
    }

    public int CustomerIDFromReservation(int id)
    {
        return _sqlconnection.Execute($@"SELECT reservation.customer_id FROM reservation WHERE reservation.id = '{id}'");
    }

    public Customer GetCustomerByReservation(int id)
    {
        Customer customer = new();
        string sql = ($@" SELECT * FROM customer
        LEFT JOIN reservation
        ON reservation.customer_id = customer.id
        WHERE reservation.id LIKE '%{id}%'");
        _sqlconnection.Open();
        MySqlCommand cmd = new MySqlCommand(sql, _sqlconnection);
        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                customer.ID = Convert.ToInt32(reader["id"].ToString());
                customer.Email = reader["email"].ToString();
                customer.Name = reader["name"].ToString();
                customer.Phonenumber = reader["phonenumber"].ToString();
            }
            _sqlconnection.Close();
            return customer;
        }
    }
}