using MySqlConnector;
using Dapper;
class Customer
{
    public int ID { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Phonenumber { get; set; }

    // CREATING A OBJECT
    public Customer(string email, string name, string phonenumber)
    {
        Email = email;
        Name = name;
        Phonenumber = phonenumber;
    }

    // MATERALIZING A OBJECT FROM DB
    public Customer(int id, string email, string name, string phonenumber)
    {
        ID = id;
        Email = email;
        Name = name;
        Phonenumber = phonenumber;
    }
    public Customer(Customer cust)
    {
        ID = cust.ID;
        Email = cust.Email;
        Name = cust.Name;
        Phonenumber = cust.Phonenumber;
    }

    public Customer()
    {

    }

    public override string ToString()
    {
        return $@"
            ID          : {ID}
            Email       : {Email}
            Name        : {Name}
            Phonenumber : {Phonenumber}";
    }

}