using MySqlConnector;
using Dapper;
class Customer
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phonenumber { get; set; }


    public override string ToString()
    {
        return $"{Name}{Phonenumber}";
    }

}