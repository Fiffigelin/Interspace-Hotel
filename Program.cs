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

        Customer cust = new();
        CustomerDB cDB = new CustomerDB();
        CustomerManagement custManager = new(cDB);

        while (true)
        {
            string prompt = @"
            Welcome to Interspace Hotel ADMIN";
            string[] options = { "Booking", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();
            //Vårt mål är att vi strävar mot att ha två ingångar, personal eller gäst
            //Huvudmeny: Customer, Employee, Exit
            //Ha en metod för CustomerUI och en för EmployeeUI.
            //Customer UI blir det som är i switchen nedan.
            //Employee UI blir lik den nedan, fast personalen måste logga för att kunna komma åt administrering utav rum och bokning osv.
            //Dvs innan de kommer in i switch med menyval. Hellre fler menyval än att man behöver gå djupt ner i undermenyer.
            //Då blir det istället en stor switch men fördelen blir att man slipper gå så långt ner i en eventuell felsökning.
            //Exempel på many innehåll: ta bort kund, lägg till kund, ändra kund,  
            //                          ta bort bokning, lägg till bokning, ändra bokning.
            //                          ta bort rum (tar även bort alla dess reservationer), lägg till rum, ändra rum.
            //                          ta bort anställd, lägg till anstäld, ändra anställd
            //Eventuell ordningsföljd i personalmenyn: Customers, Bookings, Rooms, Employees sen menyerna ovan.
            switch (selectedIndex)
            {
                case 0:
                    //Uppdatera till sök med datum.
                    //Detta medför att man får räkna ut antal dagar från start datum till slut datum.
                    //Önskvärt att filtrera sökning efteråt på antal rum kanske. Men det är nog overkill i detta fall. 
                    int search = BookingRoom();
                    List<Room> roomList = roomDB.SearchRoomDB(search);
                    int roomID = PrintSearchedRooms(roomList);
                    //Testa om det går att ta bort cust = new();
                    //Måste en kund vara ny? Kan ju finnas i DB :)
                    //Fråga om kund är ny, om ja skapa ny, annars sök upp i databas.
                    cust = new();
                    cust = AddCustomer();
                    //Om sökning utav datum redan är besämt innan metoden nedan. 
                    //Uppdatera metoden nedan att ta följande indata med:
                    // - Start datum
                    // - Längd på bokning
                    MakeReservation(custManager, reservations, roomID, cust);
                    break;

                case 1:
                    //Enda exit, alla andra är return
                    ExitMenu();
                    break;

                default:
                    break;
            }
        }
    }
    private static int BookingRoom()
    {
        int guests;
        Console.Clear();
        Console.WriteLine("----: : INTERSPACE HOTEL : :----");
        Console.WriteLine($"Psst, nicer header here :)\n");
        Console.Write("Number of guests : ");
        guests = Convert.ToInt32(Console.ReadLine());
        return guests;
    }
    private static int PrintSearchedRooms(List<Room> roomList)
    {
        Console.Clear();
        Console.WriteLine("----: : INTERSPACE HOTEL : :----");
        Console.WriteLine($"Psst, nicer header here :)\n");
        if (roomList.Count >= 1)
        {
            foreach (Room singleroom in roomList)
            {
                Console.WriteLine(singleroom);
            }
            Console.Write($"\nChoose rooms-id to book : ");
            return Convert.ToInt32(Console.ReadLine());
        }
        else
        {
            Console.WriteLine("No rooms found");
        }
        return 0;
    }
    private static Customer AddCustomer()
    {
        int id = 0;
        while (true)
        {
            string firstName;
            string lastName;
            string email;
            string phoneNumber;
            Console.Clear();
            Console.WriteLine("----: : INTERSPACE HOTEL : :----");
            Console.WriteLine($"Psst, nicer header here :)\n");
            do
            {
                Console.Write("Firstname : ");
                firstName = Console.ReadLine();
                Console.Write("Lastname : ");
                lastName = Console.ReadLine();
            } while (!IsStringValid(firstName) && !IsStringValid(lastName));
            do
            {
                Console.Write("Email : ");
                email = Console.ReadLine();
            } while (!IsEmailValid(email));
            do
            {
                Console.Write("Phonenumber : ");
                phoneNumber = Console.ReadLine();
            } while (!IsStringNumeric(phoneNumber));

            return new(email, firstName + " " + lastName, phoneNumber);

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

    private static void MakeReservation(CustomerManagement custM, ReservationDB reservations, int roomID, Customer cust)
    {
        //Behöver ses över med felhantering då det finns mycket ReadLines samt punkt2. då det just nu kräver användaren att ange ett ID.
        //skulle även behövas en form av validering som kollar ifall rummet är ledigt eller ej.
        try
        {
            // Console.WriteLine("Välj rummet du vill boka");
            // string roomChoice = Console.ReadLine();
            // int roomChoiceConvert = Convert.ToInt32(roomChoice);

            // Console.WriteLine("Ange ditt id.");
            // string customerID = Console.ReadLine();
            // int customerIDConvert = Convert.ToInt32(customerID);

            Console.WriteLine("Choose time-span. Ex 2022-11-25");
            Console.Write("Start date :  ");
            string dateInput = Console.ReadLine();
            DateTime fromDate = DateTime.Parse(dateInput);

            // Console.WriteLine("Du har bokat: " + fromDate);
            Console.Write("How many nights : ");
            string duration = Console.ReadLine();
            int durationConvert = Convert.ToInt32(duration);

            Console.Write("Price : ");
            string totalSum = Console.ReadLine();
            int totalSumConvert = Convert.ToInt32(totalSum);
            // Console.WriteLine(reservations.ToString());
            Console.WriteLine(cust);
            int customerID = custM.AddCustomer(cust);
            Console.WriteLine(customerID);
            Console.ReadKey();
            int resultat = reservations.CreateRoomReservation(roomID, customerID, fromDate, durationConvert, totalSumConvert);
            Console.WriteLine("Reservationid : " + resultat);
            Console.ReadLine();
            Thread.Sleep(3000);
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
    private static void ExitMenu()
    {
        Console.WriteLine("Please press any key to exit.");
        Console.ReadKey(true);
        Environment.Exit(0);
    }
    private static bool IsEmailValid(string s)
    {
        if (string.IsNullOrEmpty(s) || !s.Contains("@"))
        {
            return false;
        }

        return true;
    }

    private static bool IsStringValid(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        return true;
    }

    private static bool IsStringNumeric(string s)
    {
        foreach (char c in s)
        {
            if (c < '0' || c > '9')
                return false;
        }
        return true;
    }

}