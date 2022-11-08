using MySqlConnector;
using Dapper;
class Customer
{
    public int ID {get; set;}
    public string Name {get; set;}
    public string Email {get; set;}
    public string Phonenumber {get; set;}

    public override string ToString()
    {
        return $"{ID} {Name}";
    }
    public List<Customer> SearchForCustomer(MySqlConnection connection)
    {
        List <Customer> printList = new();
        var sql = "SELECT * FROM customer";
        var getCustomers = connection.Query(sql);

        foreach (var customer in getCustomers)
        {
            printList.Add(customer.ToList());
        }

        return printList;

        // var printCustomer = connection.Query($"SELECT 'customer.name' from 'customer';");
        // System.Console.WriteLine(printCustomer);
    }
}