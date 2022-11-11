using MySqlConnector;
using Dapper;
class Customer
{
    public int ID { get; set; }
    public string Email { get; set; }
    public string First_Name { get; set; }
    public string Last_Name { get; set; }
    public string Phonenumber { get; set; }

    // CREATING A OBJECT
    public Customer(string email, string firstname, string lastname, string phonenumber)
    {
        Email = email;
        First_Name = firstname;
        Last_Name = lastname;
        Phonenumber = phonenumber;
    }

    // MATERALIZING A OBJECT FROM DB
    public Customer(int id, string email, string first_name, string last_name, string phonenumber)
    {
        ID = id;
        Email = email;
        First_Name = first_name;
        Last_Name = last_name;
        Phonenumber = phonenumber;
    }

    public override string ToString()
    {
        return $@"
            ID          : {ID}
            Email       : {Email}
            First name  : {First_Name}
            Last name   : {Last_Name}
            Phonenumber : {Phonenumber}";
    }

}