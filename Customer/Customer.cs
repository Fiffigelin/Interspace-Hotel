using MySqlConnector;
using Dapper;
class Customer
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phonenumber { get; set; }


    public Customer(string name, string email, string phonenumber)
    {
        this.Name = name;
        this.Email = email;
        this.Phonenumber = phonenumber;
    }
    public override string ToString()
    {
        return @$"
        Customer id: {ID}
        Customer name: {Name}
        Customer email: {Email}
        Customer phonenumber: {Phonenumber}";
    }

}