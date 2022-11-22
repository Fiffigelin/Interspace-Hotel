class Customer
{
    public int ID { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Phonenumber { get; set; }

    public Customer(string email, string name, string phonenumber)
    {
        Email = email;
        Name = name;
        Phonenumber = phonenumber;
    }

    public Customer(int id, string email, string name, string phonenumber)
    {
        ID = id;
        Email = email;
        Name = name;
        Phonenumber = phonenumber;
    }

    public Customer()
    {

    }
}