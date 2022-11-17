class Employee
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }

    public Employee(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public Employee(int id, string name, string password)
    {
        ID = id;
        Name = name;
        Password = password;
    }

    public Employee()
    {
        
    }

    public override string ToString()
    {
        return
        @$"
        Employee id: {ID}
        Employee Name: {Name}";
    }
}