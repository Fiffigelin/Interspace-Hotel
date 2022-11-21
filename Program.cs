
using System.Text.RegularExpressions;
using MySqlConnector;
using Dapper;
internal class Program
{
    const string CONNECTIONSTRING = "Server = localhost;Database = interspace_hotel;Uid=root; Convert Zero Datetime=True";
    public static MySqlConnection connection = new MySqlConnection(CONNECTIONSTRING);
    public static Room room = new();
    public static RoomDB roomDB = new(connection);
    //RoomManagement roomManager = new(roomDB);
    public static EmployeeDB employeeDB = new(connection);
    public static EmployeeManagement empManager = new(employeeDB);
    public static Reservation reservation = new();
    public static ReservationDB reservationDB = new(connection);
    public static HotelDB hotelDB = new HotelDB(connection);
    public static HotelManagement hotelM = new(hotelDB);
    public static Customer customer = new();
    public static CustomerDB cDB = new CustomerDB();
    public static CustomerManagement custManager = new(cDB);
    public static List<Room> roomList = new();
    public static List<Reservation> reservationList = new();
    public static List<Customer> customerList = new();
    public static List<Employee> employeeList = new();

    private static void Main(string[] args)
    {
        // Console.WriteLine(hotelM.GetValues()); // HAR INTE REVIEWS I DB
        while (true)
        {
            string prompt = "";
            string[] options = { "Guest", "Employee", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();
            switch (selectedIndex)
            {
                case 0: // empty
                default:
                    break;

                case 1:
                    EmployeePWCheck();
                    EmployeeUI();
                    break;
                case 2:
                    //Enda exit, alla andra är return
                    ExitMenu();
                    break;
            }
        }
    }
    public static void EmployeeUI()
    {
        while (true)
        {
            string prompt = "";
            string[] options = { "Reservations", "Customers", "Room", "Employees", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    ReservationsMenu();
                    break;

                case 1:
                    CustomersMenu();
                    break;

                case 2:
                    RoomMenu();
                    break;

                case 3:
                    EmployeeMenu();
                    break;

                case 4:
                    return;

                default:
                    break;
            }
        }
    }
    public static void GuestMenu()
    {

    }
    public static void ReservationsMenu() // FUNKAR!! 
    {
        while (true)
        {
            string prompt = "";
            string[] options = { "Add reservation", "Update reservation", "Remove reservation", "Return", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    // UPDATE : fixa så man kan minimera sökningen ännu mer med ex, beds, guests
                    var search = BookingRoom();
                    roomList = roomDB.GetAvailableRooms(search.Item2, search.Item3);
                    PrintRooms(roomList);
                    room = roomDB.GetRoomByid(ChooseRoom());
                    reservation = CalculateReservationPrice(reservation, room, NumberOFGuests(room));
                    customer = AddCustomer();
                    reservation = new(customer, room, search.Item2, search.Item1);
                    MakeReservation(custManager, reservationDB, customer, room, reservation);
                    break;

                case 1: // update reservation
                    reservationList = reservationDB.SearchReservationByString(SearchReservation());
                    reservationList = reservationDB.SelectReservations(reservationList);
                    PrintReservations(reservationList);
                    int updateID = ChooseReservationID();
                    int custID = custManager.GetIDFromReservation(updateID);
                    reservation = reservationDB.GetReservationById(updateID);
                    var update = UpdateReservationRoom(reservation);
                    roomList = roomDB.GetAvailableRooms(update.Item2, update.Item3);
                    PrintRooms(roomList);
                    room = roomDB.GetRoomByid(updateID);
                    reservation = CalculateReservationPrice(reservation, room, UpdateGuests(reservation, room));
                    customer = custManager.GetCustomerFromReservationID(updateID);
                    Console.ReadLine();
                    reservation = reservationDB.GetReservationById(updateID);
                    UpdateReservation(reservationDB, reservation, customer, update.Item2, updateID);
                    break;

                case 2: // remove reservation
                    reservationList = reservationDB.SearchReservationByString(SearchReservation());
                    reservationList = reservationDB.SelectReservations(reservationList);
                    PrintReservations(reservationList);
                    int removeID = ChooseReservationID();
                    DeleteReservation(removeID);
                    break;

                case 3: // return to main()
                    return;

                case 4:
                    ExitMenu();
                    break;

                default:
                    break;
            }
        }
    }
    public static void CustomersMenu() // UPDATE CUSTOMER FUNGERAR EJ!!
    {
        int custID = 0;
        while (true)
        {
            string prompt = "";
            string[] options = { "Add guest", "Update guest", "Remove guest", "Return", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    customer = AddCustomer();
                    custID = custManager.AddCustomer(customer);
                    customerList.Add(custManager.GetCustomer(custID));
                    PrintCustomers(customerList);
                    CustomerAddedSuccess(custID);
                    break;

                case 1:
                    // FUNKAR EJ!!!
                    customerList = custManager.StringSearchCustomer(SearchCustomer());
                    PrintCustomers(customerList);
                    customer = UpdateCustomer(customer, ChooseCustomer());
                    custID = custManager.UpdateCustomer(customer);
                    UpdateCustomerSuccess(custID);
                    break;

                case 2:
                    customerList = custManager.StringSearchCustomer(SearchCustomer());
                    PrintCustomers(customerList);
                    customer = custManager.GetCustomer(ChooseCustomer());
                    custManager.DeleteCustomer(customer.ID);
                    RemoveCustomer(customer.ID);
                    break;

                case 3:
                    return;

                case 4:
                    ExitMenu();
                    break;

                default:
                    break;
            }
        }
    }
    public static void RoomMenu()
    {
        while (true)
        {
            Header();
            string prompt = "";
            string[] options = { "Add room", "Update room", "Print rooms", "Remove room", "Return", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0: // add
                    roomList = AddRoom(room);
                    room.id = roomDB.CreateRoom(room);
                    PrintRooms(roomList);
                    Console.ReadKey();
                    break;

                case 1: // update
                    SearchRooms();

                    break;

                case 2: // print
                    roomList = roomDB.GetRooms();
                    PrintRooms(roomList);
                    Console.ReadKey();
                    break;

                case 3: // remove
                    roomList = roomDB.GetRooms();
                    PrintRooms(roomList);
                    break;

                case 4:
                    break;

                default:
                    break;
            }
        }
    }
    public static void EmployeeMenu()
    {
        while (true)
        {
            Header();
            string prompt = "";
            string[] options = { "Add employee", "Update employee", "Remove employee", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:

                    break;

                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    break;

                case 4:
                    break;

                default:
                    break;
            }
        }
    }
    private static List<Room> AddRoom(Room room) // Tomt
    {
        string input = string.Empty;
        while (true)
        {
            Header();
            string prompt = "";
            string[] options = { "Singleroom", "Familyroom", "Deluxeroom", "Penthouse" };
            Menu roomNames = new Menu(prompt, options);
            int selectedIndex = roomNames.Run();
            switch (selectedIndex)
            {
                case 0:
                    room.name = "Singleroom";
                    room.beds = 1;
                    room.guests = 1;
                    room.size = 24;
                    room.price = 800;
                    break;
                case 1:
                    room.name = "Familyroom";
                    room.beds = 4;
                    room.guests = 4;
                    room.size = 65;
                    room.price = 2200;
                    break;
                case 2:
                    room.name = "Deluxeroom";
                    room.beds = 3;
                    room.guests = 3;
                    room.size = 120;
                    room.price = 5000;
                    break;
                case 3:
                    room.name = "Penthouse";
                    room.beds = 2;
                    room.guests = 9;
                    room.size = 155;
                    room.price = 9999;
                    break;
                default:
                    break;
            }
            List<Room> outputList = new();
            outputList.Add(room);
            return outputList;
        }
    }
    private static (int, string, string) BookingRoom()
    {
        bool isSDCorrect = false;
        bool isEDCorrect = false;
        bool isDateCoorect = false;
        string startDate = String.Empty;
        string endDate = String.Empty;
        int duration = 0;
        DateOnly sD;
        DateOnly eD;

        string pattern = @"\d{4}(-)\d{2}(-)\d{2}";

        while (!isDateCoorect)
        {
            while (!isSDCorrect)
            {
                Console.Write("Start date for your stay [YYYY-MM-DD] : ");
                startDate = Console.ReadLine()!;
            Header();
            Console.Write("Start date for your stay [YYYY-MM-DD] : ");
            startDate = Console.ReadLine()!;

            MatchCollection matches = Regex.Matches(startDate, pattern);
            int match = matches.Count;
            if (match == 1)
            {
                try
                {
                    try
                    {
                        sD = DateOnly.Parse(startDate);
                        isSDCorrect = true;
                    }
                    catch (System.Exception)
                    {
                        Console.WriteLine(" Please try again, enter valid dates.");
                    }
                }
                else
                {
                    Console.WriteLine("Please try again.");
                }
            }
            while (!isEDCorrect)
            {
                Console.Write("End date for your stay [YYYY-MM-DD] : ");
                endDate = Console.ReadLine();

            MatchCollection matching = Regex.Matches(endDate, pattern);
            int match = matching.Count;
            if (match == 1)
            {
                try
                {
                    try
                    {
                        eD = DateOnly.Parse(endDate);
                        isEDCorrect = true;
                    }
                    catch (System.Exception)
                    {
                        Console.WriteLine(" Please try again, enter valid dates.");
                    }
                }
                else
                {
                    Console.WriteLine("Please try again.");
                }
            }
            duration = (eD.DayNumber - sD.DayNumber);
            if (duration >= 1)
            {
                Console.WriteLine("okay");
                isDateCoorect = true;
            }
            else
            {
                Console.WriteLine("nope");
            }
            Thread.Sleep(2000);
        }
        
            return (duration, startDate, endDate);
        }
        private static void SearchRooms()
        {
            Console.Write("Search room : ");
            string search = Console.ReadLine();
            custManager.StringSearchCustomer(search);
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
        private static int PrintSearchedRooms(List<Room> roomList)
        {
            Header();
            if (roomList.Count >= 1)
            {
                TableUI table = new();
                table.PrintRooms(roomList);
            }
            else
            {
                Console.WriteLine("No rooms found");
            }
            return 0;
        }
        private static void RemoveRoombyID(RoomDB roomDB)
        {
            Console.WriteLine("Please state the ID of the room you would like to delete:");
            string idRemove = Console.ReadLine();
            int idRemoveConvert = Convert.ToInt32(idRemove);
            roomDB.DeleteRoom(idRemoveConvert);
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
            TableUI table = new();
            table.PrintCustomers(customerList);
            Console.ReadLine();
        }
        private static void RemoveCustomer(CustomerManagement customerM)
        {
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
        private static string SearchReservation()
        {
            string search = string.Empty;
            do
            {
                Console.Write("Search reservation : ");
                search = Console.ReadLine();
            } while (string.IsNullOrEmpty(search));
            return search;
        }
        private static void PrintReservations(List<Reservation> reservationsList)
        {
            TableUI table = new();
            table.PrintReservations(reservationList);
            Console.ReadLine();
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
        private static void MakeReservation(CustomerManagement custM, ReservationDB reservations, Customer cust, string startDate, int duration)
        {
            try
            {
                Console.Write($"\nChoose rooms-id to book : ");
                int roomID = Convert.ToInt32(Console.ReadLine());
                DateTime dateTime = Convert.ToDateTime(startDate);
                // Detta skall ske automatiskt. Skapa en funktion som räknar ut kostnaden beroende på antal nätter, gästantal och valt rum
                Console.Write("Number of guests : ");
                string totalGuests = Console.ReadLine();
                int totalG = Convert.ToInt32(totalGuests);

                int economy = 1; // ÄNDRA MIG 
                int totalCosts = totalG * duration * economy; // Här skapade Emelie en metod för uträkningen ?
                                                              // I DB heter det economy = pris, guests = antal gäster, duration = antalet nätter.
                                                              // 

                int customerID = custM.AddCustomer(cust);
                Console.WriteLine(cust); //skapa en snyggare utskrift där inte id visas
                int reservationID = reservations.CreateRoomReservation(roomID, customerID, dateTime, duration, totalG); // här stod det int totalSumConvert innan Emelie pillade 

                Console.WriteLine("Here is your receipt to your reservation");
                TableUI table = new();
                table.PrintReceipt(roomID, dateTime, duration, totalG, cust, reservationID); // här stod det int totalSumConvert innan Emelie pillade 
                Console.ReadLine();
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
        private static void Header()
        {
            Console.Clear();
            Console.WriteLine(@"
██╗███╗   ██╗████████╗███████╗██████╗ ███████╗██████╗  █████╗  ██████╗███████╗    ██╗  ██╗ ██████╗ ████████╗███████╗██╗     
██║████╗  ██║╚══██╔══╝██╔════╝██╔══██╗██╔════╝██╔══██╗██╔══██╗██╔════╝██╔════╝    ██║  ██║██╔═══██╗╚══██╔══╝██╔════╝██║     
██║██╔██╗ ██║   ██║   █████╗  ██████╔╝███████╗██████╔╝███████║██║     █████╗      ███████║██║   ██║   ██║   █████╗  ██║     
██║██║╚██╗██║   ██║   ██╔══╝  ██╔══██╗╚════██║██╔═══╝ ██╔══██║██║     ██╔══╝      ██╔══██║██║   ██║   ██║   ██╔══╝  ██║     
██║██║ ╚████║   ██║   ███████╗██║  ██║███████║██║     ██║  ██║╚██████╗███████╗    ██║  ██║╚██████╔╝   ██║   ███████╗███████╗
╚═╝╚═╝  ╚═══╝   ╚═╝   ╚══════╝╚═╝  ╚═╝╚══════╝╚═╝     ╚═╝  ╚═╝ ╚═════╝╚══════╝    ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚══════╝╚══════╝
 "); // http://patorjk.com/software/taag
            Console.WriteLine($"Psst, nicer header here :)\n");
        }
        private static void ExitMenu()
        {
            Console.WriteLine("Please press any key to exit.");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
    }