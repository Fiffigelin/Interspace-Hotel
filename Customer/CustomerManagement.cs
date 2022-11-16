class CustomerManagement
{
    // Gör en massa små metoder som kan återanvändas i denna klassens metoder.
    // Är det detta som är interfaces? GUSTAV??? 
    private CustomerDB customerDB { get; set; }
    public CustomerManagement(CustomerDB db)
    {
        customerDB = db;
    }

    public Customer GetCustomer(int id)
    {
        Customer cu = customerDB.SelectCustomer(id);
        return cu;
    }

    public string AddCustomer(string email, string firstName, string lastName, string phonenumber)
    {
        if (!IsEmailValid(email)) return "Invalid input of email";
        if (!IsStringValid(firstName)) return "Invalid input of first name";
        if (!IsStringValid(lastName)) return "Invalid input of last name";
        if (!IsStringNumeric(phonenumber)) return "Invalid input of phonenumber";

        Customer cu = new(email, firstName, lastName, phonenumber);
        int id;
        try
        {
            id = customerDB.InsertCustomer(cu);
        }
        catch (System.Exception)
        {

            return $"ERROR ADDING CUSTOMER";
        }

        return $"NEW CUSTOMER CREATED WITH ID : {id}";
    }

    public string UpdateCustomer(string email, string firstName, string lastName, string phonenumber, int id)
    {
        if (!IsEmailValid(email)) return "Invalid input of email";
        if (!IsStringValid(firstName)) return "Invalid input of first name";
        if (!IsStringValid(lastName)) return "Invalid input of last name";
        if (!IsStringNumeric(phonenumber)) return "Invalid input of phonenumber";

        Customer cu = new(email, firstName, lastName, phonenumber);
        try
        {
            customerDB.UpdateCustomer(cu);
        }
        catch (System.Exception)
        {

            return $"ERROR MODYFYING CUSTOMER";
        }

        return $"MODIFIED CUSTOMER WITH ID : {id}";
    }
    public string RemoveCustomer(int id)
    {
        try
        {
            customerDB.DeleteCustomer(id);
            return $"CUSTOMER WITH ID : {id} REMOVED";
        }
        catch (System.Exception)
        {
            return $"NO CUSTOMER FOUND WITH ID : {id}";
        }
    }

    public List<Customer> StringSearchCustomer(string search)
    {
        return customerDB.SearchCustomerDB(search);
    }

    public List<Customer> GetAllCustomers()
    {
        return customerDB.GetCustomers();
    }

    private bool IsStringNumeric(string s)
    {
        foreach (char c in s)
        {
            if (c < '0' || c > '9')
                return false;
        }
        return true;
    }
    private bool IsEmailValid(string s)
    {
        if (string.IsNullOrEmpty(s) || !s.Contains("@"))
        {
            return false;
        }

        return true;
    }

    private bool IsStringValid(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        return true;
    }

    private bool IsCustomerListed(List<Customer> customerList)
    {
        if (customerList.Count <= 0)
        {
            return false;
        }

        return true;
    }
}