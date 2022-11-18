using Dapper;
using MySqlConnector;
class EmployeeManagement
{
    EmployeeDB employeeDB;
    public EmployeeManagement(EmployeeDB connection)
    {
        employeeDB = connection;
    }

    public string AddEmployee(string name, string password)
    {
        if (!IsStringValid(name)) return "Invalid input of name";
        if (!IsStringValid(password)) return "Invalid input of password";

        int id;
        try
        {
            id = employeeDB.CreateEmployee(name, password);
        }
        catch (System.Exception)
        {

            return $"ERROR ADDING EMPLOYEE";
        }

        return $"NEW EMPLOYEE CREATED WITH ID : {id}";
    }

    // public string ModifyCustomer(Customer customer)
    // {
    //     if (!IsStringValid(customer.Name)) return "Invalid input of name";
    //     //if (!IsStringValid(customer.)) return "Invalid input of password";

    //     try
    //     {
    //         employeeDB.UpdateCustomer(customer);
    //     }
    //     catch (System.Exception)
    //     {

    //         return $"ERROR UPDATING FAILED";
    //     }

    //     return $"UPDATING EMPLOYEE WITH ID : {customer.ID}";
    // }
    public string RemoveEmployee(int id)
    {
        try
        {
            employeeDB.DeleteEmployee(id);
            return $"EMPLOYEE WITH ID : {id} REMOVED";
        }
        catch (System.Exception)
        {
            return $"NO EMPLOYEE FOUND WITH ID : {id}";
        }
    }

    public List<Employee> GetAllEmployees()
    {
        return employeeDB.ListEmployees();
    }

    private bool IsStringValid(string s)
    {
        if (string.IsNullOrEmpty(s)) return false;

        return true;
    }
}