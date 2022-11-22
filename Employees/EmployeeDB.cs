using Dapper;
using MySqlConnector;

class EmployeeDB
{
    MySqlConnection _sqlconnection;
    public EmployeeDB(MySqlConnection connection)
    {
        _sqlconnection = connection;
    }

    public int CreateEmployee(string name, string password)
    {
        string sql = @$"INSERT INTO personal (personal.name, personal.password)VALUES ('{name}', '{password}');SELECT LAST_INSERT_ID();";
        // string createEmployee = _sqlconnection.QuerySingle<string>(sql);
        // return createEmployee;
        int id = _sqlconnection.QuerySingle<int>(sql);
        return id;
    }

    public List<Employee> ListEmployees()
    {
        string sql = @$"SELECT personal.id, personal.name, personal.password FROM personal;";
        var employees = _sqlconnection.Query<Employee>(sql).ToList();
        return employees;
    }
    public Employee GetEmployeeById(int id)
    {
        string sql = @$"SELECT * FROM personal WHERE personal.id = {id};";
        var employee = _sqlconnection.QuerySingle<Employee>(sql);
        return employee;
    }

    public void UpdateEmployee(Employee employee)
    {
        string sql = @$"UPDATE personal SET personal.name = '{employee.name}', personal.password = '{employee.password}' WHERE id = {employee.id};";
        _sqlconnection.Query<Employee>(sql);
    }

    public void DeleteEmployee(int ID)
    {
        string sql = (@$"DELETE FROM personal WHERE personal.id = {ID}");
        _sqlconnection.Query<Employee>(sql);
    }

    public Employee SelectEmployee(int id)
    {
        // https://stackoverflow.com/questions/14171794/how-to-retrieve-data-from-a-sql-server-database-in-c
        Employee em = new();
        string sql = $@"SELECT * FROM `personal` WHERE `personal`.`id`= {id}";
        MySqlCommand cmd = new MySqlCommand(sql, _sqlconnection);
        _sqlconnection.Open();
        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                em.id = Convert.ToInt32(reader["id"].ToString());
                em.name = reader["name"].ToString();
                em.password = reader["password"].ToString();
            }
            _sqlconnection.Close();
        }
        return em;
    }
}