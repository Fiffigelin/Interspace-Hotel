class Employee
{
    public int id { get; set; }
    public string name { get; set; }
    public string password { get; set; }

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
        Employee id: {id}
        Employee Name: {name}";
    }
}