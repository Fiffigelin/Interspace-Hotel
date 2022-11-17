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
        var employees = _sqlconnection.Query<Employee>(@$"SELECT personal.id, personal.name, personal.password FROM personal;").ToList();
        return employees;
    }

    public void UpdateEmployee(int ID, string name, string password)
    {
        _sqlconnection.Query<Employee>(@$"UPDATE personal SET personal.name= '{name}', personal.password = '{password}' WHERE id = {ID};");
    }

    public void DeleteEmployee(int ID)
    {
        string sql = (@$"DELETE FROM personal WHERE personal.id = {ID}");
    }

}