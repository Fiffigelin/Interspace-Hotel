using MySqlConnector;
using Dapper;
internal class Program
{
    const string CONNECTIONSTRING = "Server = localhost;Database = interspace_hotel;Uid=root; Convert Zero Datetime=True";

    private static void Main(string[] args)
    {
        MySqlConnection connection = new MySqlConnection(CONNECTIONSTRING);
        RoomDB roomDB = new(connection);
        //RoomManagement roomManager = new(roomDB);
        EmployeeDB employeeDB = new(connection);
        EmployeeManagement empManager = new(employeeDB);
        ReservationDB reservations = new(connection);
        HotelDB hotelDB = new HotelDB(connection);
        HotelManagement hotelM = new(hotelDB);

        CustomerDB cDB = new CustomerDB();
        CustomerManagement custManager = new(cDB);

Console.WriteLine(hotelM.GetValues());
        while (true)
        {
            string prompt = @"
            Welcome to Interspace Hotel ADMIN";
            string[] options = { "Booking", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    string search = BookingRoom();
                    List<Room> roomList = roomDB.SearchRoomDB(search);
                    int roomID = PrintSearchedRooms(roomList);
                    int customerID = AddCustomer(custManager);
                    MakeReservation(reservations);
                    break;

                case 1:
                    //ExitMenu();
                    break;

                default:
                    break;
            }
        }

        // List<Customer> customerList = cm.GetAllCustomers(); - funkar
        // PrintCustomers(customerList); - funkar 

        // // Room listRooms = new();  EMELIE HAR TILLFÄLLIGT KOMMENTERAT UT DENNA!

        // UI ui = new();
        // ui.Start();

        // AddCustomer(cm); - FUNKAR
        // UpdateCustomer(cm, id); - !!!!!!FUNKAR INTE!!!!!
        // int id = GetCustomer(cm); - FUNKAR
        // RemoveCustomer(cm); - FUNKAR

        //MakeReservation(reservations);


        //     @$" {roomChoiceConvert}

        // int resultat = reservations.CreateRoomReservation(roomChoiceConvert, customerIDConvert, fromDate, durationConvert, totalSumConvert);
        //     Console.WriteLine(Reservation gjord:  + resultat);

        //     ");
        //UpdateReservation(reservations);
        //DeleteReservation(reservations);

        //UpdateRoom(roomDB);
        //RemoveRoombyID(roomDB);

        //UpdateEmployee(employeeDB);
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
    }
    private static string BookingRoom()
    {
        Console.Clear();
        Console.WriteLine("----: : INTERSPACE HOTEL : :----");
        Console.WriteLine($"Psst, nicer header here :)\n");
        Console.Write("Number of guests : ");
        string guests = Console.ReadLine();
        return guests;
    }
    private static int PrintSearchedRooms(List<Room> roomList)
    {
        Console.Clear();
        Console.WriteLine("----: : INTERSPACE HOTEL : :----");
        Console.WriteLine($"Psst, nicer header here :)\n");
        foreach (Room singleroom in roomList)
        {
            Console.WriteLine(singleroom);
        }
        Console.Write($"\nChoose rooms-id to book : ");
        return Convert.ToInt32(Console.ReadLine());
    }
    private static int AddCustomer(CustomerManagement customerM)
    {
        int id = 0;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("----: : INTERSPACE HOTEL : :----");
            Console.WriteLine($"Psst, nicer header here :)\n");
            Console.Write("Firstname : ");
            string firstName = Console.ReadLine();
            Console.Write("Lastname : ");
            string lastName = Console.ReadLine();
            Console.Write("Email : ");
            string email = Console.ReadLine();
            Console.Write("Phonenumber : ");
            string phoneNumber = Console.ReadLine();
            try
            {
                id = customerM.AddCustomer(email, firstName, lastName, phoneNumber);
                Console.WriteLine("Registration succeeded");
            }
            catch (System.Exception)
            {
                Console.WriteLine("Registration failed");
                Thread.Sleep(1000);
            }
            return id;
        }

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
        Console.WriteLine("Please state wich reservation you would like to delete, specify by ID.");
        string deleteReservation = Console.ReadLine();
        int deleteConvert = Convert.ToInt32(deleteReservation);
        reservations.DeleteReservation(deleteConvert);
    }

    private static void UpdateReservation(ReservationDB reservations)
    {
        try
        {
            Console.WriteLine("Please state reservation ID:");
            string reservationid = Console.ReadLine();
            int idConvert = Convert.ToInt32(reservationid);
            Reservation reservation = reservations.GetReservationById(idConvert);

            Console.WriteLine("Please state the new room ID that you would like to move the guest to:");
            string updateRoom = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(updateRoom))
            {
                reservation.room_id = Convert.ToInt32(updateRoom);
            }

            Console.WriteLine("Please state dates that need to be changed:");
            string date = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(date))
            {
                reservation.date_in = DateTime.Parse(date);
            }

            Console.WriteLine("Please change durations of days:");
            string duration = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(duration))
            {
                reservation.duration = Convert.ToInt32(duration);
            }

            Console.WriteLine("Ange nytt pris - Denna måste ändras");
            string price = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(price))
            {
                reservation.economy = Convert.ToInt32(price);
            }

            reservations.UpdateReservation(reservation);
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static void MakeReservation(ReservationDB reservations)
    {
        //Behöver ses över med felhantering då det finns mycket ReadLines samt punkt2. då det just nu kräver användaren att ange ett ID.
        //skulle även behövas en form av validering som kollar ifall rummet är ledigt eller ej.
        try
        {
            Console.WriteLine("Choose the room you would like to book:");
            string roomChoice = Console.ReadLine();
            int roomChoiceConvert = Convert.ToInt32(roomChoice);

            Console.WriteLine("Please state your ID:");
            string customerId = Console.ReadLine();
            int customerIDConvert = Convert.ToInt32(customerId);

            Console.WriteLine("Please state when you would like to reserve the room. Write like this 2022-11-25");
            string dateInput = Console.ReadLine();
            DateTime fromDate = DateTime.Parse(dateInput);

            Console.WriteLine("You have booked: " + fromDate);
            Console.WriteLine("Please state how many nights you would like to stay:");
            string duration = Console.ReadLine();
            int durationConvert = Convert.ToInt32(duration);

            Console.WriteLine("Ange totalsumma - ÄNDRA DENNA ");
            string totalSum = Console.ReadLine();
            int totalSumConvert = Convert.ToInt32(totalSum);
            Console.WriteLine(reservations.ToString());
            int result = reservations.CreateRoomReservation(roomChoiceConvert, customerIDConvert, fromDate, durationConvert, totalSumConvert);
            Console.WriteLine("Reservation went succesfully: " + result);

            Console.WriteLine($@"
            Here is your receipt to your reservation:
            Booked room : {roomChoiceConvert}
            Check-in date: {fromDate}
            Durations of nights: {duration}
            Total costs : {totalSumConvert}
            Booked by : {customerIDConvert}
            
            With contact needed for changes or cancellation, please state {result}");
        }
        catch (System.Exception e)
        {

            Console.WriteLine("Error: " + e);
        }
    }
    private static void UpdateEmployee(EmployeeDB employeeDB)
    {
        Console.WriteLine("Which employee would you like to update? Write the number ID by number, ex 1.");
        string id = Console.ReadLine();
        int employeeIDConvert = Convert.ToInt32(id);
        Employee updateEmployee = employeeDB.GetEmployeeById(employeeIDConvert);

        Console.WriteLine("Enter name:");
        string employeeName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(employeeName))
        {
            updateEmployee.name = employeeName;
        }
        Console.WriteLine("State password");
        string employeePassword = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(employeePassword))
        {
            updateEmployee.password = employeePassword;
        }
        employeeDB.UpdateEmployee(updateEmployee);
    }

    private static void CreateEmployee(EmployeeDB employeeDB)
    {
        Console.WriteLine("Enter name");
        string nameInput = Console.ReadLine();
        Console.WriteLine("Enter password");
        string passwordInput = Console.ReadLine();
        var createEmployee = employeeDB.CreateEmployee(nameInput, passwordInput);
        Console.WriteLine($"Adding new employee {nameInput}");
    }

    private static void RemoveRoombyID(RoomDB roomDB)
    {
        Console.WriteLine("Please state the ID of the room you would like to delete:");
        string idRemove = Console.ReadLine();
        int idRemoveConvert = Convert.ToInt32(idRemove);
        roomDB.DeleteRoom(idRemoveConvert);
    }

    private static void UpdateRoom(RoomDB roomDB)
    {
        Console.WriteLine("type in room id");
        string id = Console.ReadLine();
        int idconvert = Convert.ToInt32(id);
        Room updateRoom = roomDB.GetRoomByid(idconvert);

        Console.WriteLine("type in number of beds");
        string Beds = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(Beds))
        {
            updateRoom.beds = Convert.ToInt32(Beds);
        }

        Console.WriteLine("Type in max amount of guests");
        string guests = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(guests))
        {
            updateRoom.guests = Convert.ToInt32(guests);
        }

        Console.WriteLine("type in size");
        string size = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(size))
        {
            updateRoom.size = Convert.ToInt32(size);
        }

        Console.WriteLine("type in price");
        string price = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(price))
        {
            updateRoom.price = Convert.ToInt32(price);
        }
        roomDB.UpdateRoom(updateRoom);
    }
}