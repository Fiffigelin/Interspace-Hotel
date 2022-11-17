class Employee
{
    public int id { get; set; }
    public string name { get; set; }
    public string password { get; set; }

    public override string ToString()
    {
        return
        @$"
        Employee id: {id}
        Employee Name: {name}";
    }
}