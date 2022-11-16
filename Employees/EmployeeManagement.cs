using Dapper;
using MySqlConnector;
class EmployeeManagement
{
    EmployeeDB employeeDB;
    public EmployeeManagement(EmployeeDB connection)
    {
        employeeDB = connection;
    }
}