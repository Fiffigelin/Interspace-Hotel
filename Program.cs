using MySqlConnector;
using Dapper;
internal class Program
{
    const string CONNECTIONSTRING = "Server = localhost;Database = interspace_hotel;Uid=root";

    private static void Main(string[] args)
    {
        MySqlConnection connection = new MySqlConnection(CONNECTIONSTRING);
        RoomDB roomDB = new(connection);
        EmployeeDB employeeDB = new(connection);
        EmployeeManagement empManager = new(employeeDB);
        ReservationDB reservations = new(connection);
        CustomerDB cDB = new CustomerDB();
        CustomerManagement cm = new(cDB);

        // AddCustomer(cm); - FUNKAR
        // int id = GetCustomer(cm); - FUNKAR
        // UpdateCustomer(cm, id); - !!!!!!FUNKAR INTE!!!!!
        // List<Customer> customerList = cm.GetAllCustomers(); - funkar
        // PrintCustomers(customerList); - funkar 
        // RemoveCustomer(cm); - FUNKAR


        // // Room listRooms = new();  EMELIE HAR TILLFÄLLIGT KOMMENTERAT UT DENNA!

        UI ui = new();
        ui.Start();

        //MakeReservation(reservations);
        //UpdateReservation(reservations);
        //DeleteReservation(reservations);

        //UpdateRoom(roomDB);
        //RemoveRoombyID(roomDB);

        //RoomManagement roomManager = new(roomDB);

        //CreateEmployee(employer);



        // var reservation = reservations.ListReservations();
        // foreach (Reservation item in reservation)
        // {
        //     Console.WriteLine(item);
        // }
        //Skriver ut lista på all personal i databasen med ID samt Namn
        // var emp = employer.ListEmployees();
        // foreach (Employee employees in emp)
        // {
        //     Console.WriteLine(employees);
        // }

        //UpdateEmployee(employer);
        // //metoden ger dig val att ge input för att uppdatera namn och lösenord på ett specifikt id

        // //skriver ut den nya updaterade listan på personal
        // var emp1 = employer.ListEmployees();
        // foreach (Employee employees in emp1)
        // {
        //     Console.WriteLine(employees);
        // }

        // var rooms = roomDB.GetRooms();
        // foreach (Room r in rooms)
        // {
        //     Console.WriteLine(r.name + " " + r.price + " " + r.size + " " + r.beds + " " + r.guests);
        // }

        // GUSTAVS TIPS
        //SqlConnect connection = kjfgkfjgkfg
        //RoomDB roomDb = new(connection)

    }
    private static void AddCustomer(CustomerManagement customerM)
    {
        // HEADER
        Console.WriteLine("ADD CUSTOMER");
        Console.Write("Firstname : ");
        string firstName = Console.ReadLine();
        Console.Write("Lastname : ");
        string lastName = Console.ReadLine();
        Console.Write("Email : ");
        string email = Console.ReadLine();
        Console.Write("Phonenumber : ");
        string phoneNumber = Console.ReadLine();
        Console.WriteLine(customerM.AddCustomer(email, firstName, lastName, phoneNumber));
    }

    private static void UpdateCustomer(CustomerManagement customerM, int id)
    {
        // HEADER
        Console.WriteLine("UPDATE CUSTOMER");
        Console.Write("Firstname : ");
        string firstName = Console.ReadLine();
        Console.Write("Lastname : ");
        string lastName = Console.ReadLine();
        Console.Write("Email : ");
        string email = Console.ReadLine();
        Console.Write("Phonenumber : ");
        string phoneNumber = Console.ReadLine();
        Console.WriteLine(customerM.UpdateCustomer(email, firstName, lastName, phoneNumber, id));

    }
    private static int GetCustomer(CustomerManagement customerM)
    {
        // HEADER
        int id = 0;
        Console.WriteLine("SELECT CUSTOMER BY ID");
        Console.Write("ID : ");
        try
        {
            id = Convert.ToInt32(Console.ReadLine());
        }
        catch (System.Exception)
        {
            Console.WriteLine("WRONG INPUT");
        }
        Console.WriteLine(customerM.GetCustomer(id));
        return id;
    }

    private static void PrintCustomers(List<Customer> customerList)
    {
        foreach (Customer information in customerList)
        {
            Console.WriteLine(information);
        }
    }

    private static void RemoveCustomer(CustomerManagement customerM)
    {
        // HEADER
        int id = 0;
        Console.WriteLine("SELECT CUSTOMER BY ID");
        Console.Write("ID : ");
        try
        {
            id = Convert.ToInt32(Console.ReadLine());
        }
        catch (System.Exception)
        {
            Console.WriteLine("WRONG INPUT");
        }
        Console.WriteLine(customerM.RemoveCustomer(id));
        
    }
    private static void DeleteReservation(ReservationDB reservations)
    {
        Console.WriteLine("Ange vilken reservation du vill ta bort, ange id:et");
        string deleteReservation = Console.ReadLine();
        int deleteConvert = Convert.ToInt32(deleteReservation);
        reservations.DeleteReservation(deleteConvert);
    }

    private static void UpdateReservation(ReservationDB reservations)
    {
        try
        {
            Console.WriteLine("ange customer ID");
            string customerid = Console.ReadLine();
            int idConvert = Convert.ToInt32(customerid);
            Console.WriteLine("Ange nya rums id du vill flytta gästen till");
            string updateRoom = Console.ReadLine();
            int roomConvert = Convert.ToInt32(updateRoom);
            Console.WriteLine("Ange datum ändring");
            string date = Console.ReadLine();
            int dateConvert = Convert.ToInt32(date);
            Console.WriteLine("Ange antal dagar");
            string duration = Console.ReadLine();
            int durationConvert = Convert.ToInt32(duration);
            Console.WriteLine("Ange nytt pris");
            string price = Console.ReadLine();
            int priceConvert = Convert.ToInt32(price);

            reservations.UpdateReservation(idConvert, roomConvert, dateConvert, durationConvert, priceConvert);
        }
        catch (System.Exception)
        {

            Console.WriteLine("Ange endast med siffror, inga tecken.");
        }
    }

    private static void MakeReservation(ReservationDB reservations)
    {
        //Behöver ses över med felhantering då det finns mycket ReadLines samt punkt2. då det just nu kräver användaren att ange ett ID.
        //skulle även behövas en form av validering som kollar ifall rummet är ledigt eller ej.
        try
        {
            Console.WriteLine("Välj rummet du vill boka");
            string roomChoice = Console.ReadLine();
            int roomChoiceConvert = Convert.ToInt32(roomChoice);
            Console.WriteLine("Ange ditt id.");
            string customerID = Console.ReadLine();
            int customerIDConvert = Convert.ToInt32(customerID);
            Console.WriteLine("Ange när du vill reservera rummet. Ange i siffror tex 20221125");
            string dateInput = Console.ReadLine();
            int dateInutConvert = Convert.ToInt32(dateInput);
            Console.WriteLine("Ange hur många dagar du vill stanna.");
            string duration = Console.ReadLine();
            int durationConvert = Convert.ToInt32(duration);
            Console.WriteLine("Ange totalsumma");
            string totalSum = Console.ReadLine();
            int totalSumConvert = Convert.ToInt32(totalSum);
            Console.WriteLine(reservations.ToString());
            reservations.CreateRoomReservation(roomChoiceConvert, customerIDConvert, dateInutConvert, durationConvert, totalSumConvert);

        }
        catch (System.Exception)
        {

            Console.WriteLine("Ange endast med siffror, inga tecken.");
        }
    }

    private static void UpdateEmployee(EmployeeDB employer)
    {
        Console.WriteLine("Vilken personal vill du uppdatera? skriv in id siffra.");
        string employeeIDInput = Console.ReadLine();
        int employeeIDConvert = Convert.ToInt32(employeeIDInput);
        Console.WriteLine("Skriv in namn");
        string employeeName = Console.ReadLine();
        Console.WriteLine("Skriv in lösenord");
        string employeePassword = Console.ReadLine();
        employer.UpdateEmployee(employeeIDConvert, employeeName, employeePassword);
    }

    private static void CreateEmployee(EmployeeDB employer)
    {
        Console.WriteLine("Skriv in namn");
        string nameInput = Console.ReadLine();
        Console.WriteLine("Skriv in lösenord");
        string passwordInput = Console.ReadLine();
        var createEmployee = employer.CreateEmployee(nameInput, passwordInput);
        Console.WriteLine($"Lägger in ny personal {nameInput}");
    }

    private static void RemoveRoombyID(RoomDB roomDB)
    {
        Console.WriteLine("Skriv in ett rums id du vill ta bort");
        string idRemove = Console.ReadLine();
        int idRemoveConvert = Convert.ToInt32(idRemove);
        roomDB.DeleteRoom(idRemoveConvert);
    }


    private static void UpdateRoom(RoomDB roomDB)
    {
        Console.WriteLine("type in a price");
        string inputPrice = Console.ReadLine();
        int priceConverter = Convert.ToInt32(inputPrice);
        Console.WriteLine("type in number of beds");
        string inputBeds = Console.ReadLine();
        int bedsConverter = Convert.ToInt32(inputBeds);
        Console.WriteLine("type in size");
        string inputSize = Console.ReadLine();
        int sizeConverter = Convert.ToInt32(inputSize);
        Console.WriteLine("type in id");
        string inputID = Console.ReadLine();
        int IDConverter = Convert.ToInt32(inputID);
        roomDB.UpdateRoom(priceConverter, bedsConverter, sizeConverter, IDConverter);
    }
}