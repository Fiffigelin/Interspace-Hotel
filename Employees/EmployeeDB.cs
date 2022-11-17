using Dapper;
using MySqlConnector;

class EmployeeDB
{
    MySqlConnection _sqlconnection;
    public EmployeeDB(MySqlConnection connection)
    {
        _sqlconnection = connection;
    }

    public string CreateEmployee(string name, string password)
    {
        string sql = @$"INSERT INTO personal (personal.name, personal.password)VALUES ('{name}', '{password}');SELECT LAST_INSERT_ID();";
        string createEmployee = _sqlconnection.QuerySingle<string>(sql);
        return createEmployee;
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
        string sql = @$"UPDATE personal SET personal.name= '{employee.name}', personal.password = '{employee.password}' WHERE id = {employee.id};";
        _sqlconnection.Query<Employee>(sql);
    }

    public void DeleteEmployee(int ID)
    {
        string sql = (@$"DELETE FROM personal WHERE personal.id = {ID}");
        _sqlconnection.Query<Employee>(sql);
    }

}