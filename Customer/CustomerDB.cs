using Dapper;
using MySqlConnector;

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
            string sql =$@"INSERT INTO `customer` 
            (`id`, `email`, `first_name`, `last_name`, `phonenumber`)
            VALUES (NULL, '{cu.Email}', '{cu.First_Name}', '{cu.Last_Name}', '{cu.Phonenumber}');
            SELECT LAST_INSERT_ID()";

            int id = _sqlConnection.QuerySingle<int>(sql);
            return id;
        }

        public List<Customer> GetCustomers()
        {
            var customerList =_sqlConnection.Query<Customer>($@"SELECT * FROM `customer`").ToList();
            return customerList;   
        }

        public List<Customer> GetSingleCustomer(int id)
        {
            var customerList =_sqlConnection.Query<Customer>($@"SELECT * FROM `customer` WHERE `customer`.`id`= {id}").ToList();
            return customerList;   
        }

        public string DeleteCustomer(int id)
        {
            int deleteID = _sqlConnection.Execute($@"DELETE FROM `customer` WHERE `customer`.`id` = {id}");
        
        if (deleteID > 0)
            {
                return $"CUSTOMER WITH ID : {id} REMOVED";
            }
        return $"NO CUSTOMER FOUND WITH ID : {id}";
        }

}