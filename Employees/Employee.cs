class Employee
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }

    public override string ToString()
    {
        return
        @$"
        Employee id: {ID}
        Employee Name: {Name}";
    }
}