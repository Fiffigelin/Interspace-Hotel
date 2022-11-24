class CustomerManagement
{
    private CustomerDB customerDB { get; set; }
    public CustomerManagement(CustomerDB db)
    {
        customerDB = db;
    }

    public Customer GetCustomer(int id)
    {
        return customerDB.SelectCustomer(id);
    }

    public Customer GetCustomerFromReservationID(int id)
    {
        return customerDB.GetCustomerByReservation(id);
    }

    public int AddCustomer(Customer customer)
    {
        return customerDB.InsertCustomer(customer);
    }

    public void UpdateCustomer(Customer customer)
    {
        customerDB.UpdateCustomer(customer);
    }

    public void DeleteCustomer(int id)
    {
        customerDB.DeleteCustomer(id);
    }

    public List<Customer> StringSearchCustomer(string search)
    {
        return customerDB.SearchCustomerDB(search);
    }

    public List<Customer> GetAllCustomers()
    {
        return customerDB.GetCustomers();
    }

    public int GetIDFromReservation(int id)
    {
        return customerDB.CustomerIDFromReservation(id);
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