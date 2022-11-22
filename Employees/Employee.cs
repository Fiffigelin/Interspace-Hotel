class Employee
{
    public int id { get; set; }
    public string name { get; set; }
    public string password { get; set; }

    public Employee(string name, string password)
    {
        name = name;
        password = password;
    }

    public Employee(int id, string name, string password)
    {
        id = id;
        name = name;
        password = password;
    }

    public Employee()
    {

    }
}