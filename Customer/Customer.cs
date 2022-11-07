using MySqlConnector;
using Dapper;
class Customer
{
    public int ID {get; set;}
    public string Name {get; set;}
    public string Email {get; set;}
    public string Phonenumber {get; set;}

    public void SearchForCustomer(MySqlConnection connection)
    {
        var printCustomer = connection.QuerySingle<string>($"SELECT customer.name from customer;");
        System.Console.WriteLine(printCustomer);
    }
}