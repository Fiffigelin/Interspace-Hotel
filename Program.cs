
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
    public static Reservation reserv = new();
    public static ReservationDB reservations = new(connection);
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
            //Vårt mål är att vi strävar mot att ha två ingångar, personal eller gäst
            //Huvudmeny: Customer, Employee, Exit
            //Ha en metod för CustomerUI och en för EmployeeUI.
            //Customer UI blir det som är i switchen nedan.
            //Employee UI blir lik den nedan, fast personalen måste logga för att kunna komma åt administrering utav rum och bokning osv.
            //Dvs innan de kommer in i switch med menyval. Hellre fler menyval än att man behöver gå djupt ner i undermenyer.
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
    public static void ReservationsMenu()
    {
        while (true)
        {
            string prompt = "";
            string[] options = { "Add reservation", "Update reservation", "Remove reservation", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    // UPDATE : fixa så man kan minimera sökningen ännu mer med ex, beds, guests
                    var search = BookingRoom();
                    roomList = roomDB.GetAvailableRooms(search.Item2, search.Item3);
                    PrintSearchedRooms(roomList);
                    room = roomDB.GetRoomByid(ChooseRoom());
                    customer = AddCustomer();
                    reserv = new(customer, room, search.Item2, search.Item1);
                    MakeReservation(custManager, reservations, customer, room, reserv);
                    break;

                case 1: // update reservation
                    reservationList = reservations.SearchReservationByString(SearchReservation());
                    reservationList = reservations.SelectReservations(reservationList);
                    PrintReservations(reservationList);
                    int updateID = ChooseReservationID();
                    int custID = custManager.GetIDFromReservation(updateID);
                    var update = UpdateReservationInfo();
                    roomList = roomDB.GetAvailableRooms(update.Item2, update.Item3);
                    PrintSearchedRooms(roomList);
                    customer = custManager.GetCustomerFromReservationID(updateID);
                    Console.ReadLine();
                    reserv = reservations.GetReservationById(updateID);
                    UpdateReservation(reservations, reserv, customer, update.Item2, update.Item1, updateID);
                    break;

                case 2: // remove reservation
                    reservationList = reservations.SearchReservationByString(SearchReservation());
                    reservationList = reservations.SelectReservations(reservationList);
                    PrintReservations(reservationList);
                    int removeID = ChooseReservationID();
                    DeleteReservation(removeID);
                    break;

                case 3: // return to main()
                    return;

                default:
                    break;
            }
        }
    }
    public static void CustomersMenu()
    {
        while (true)
        {
            string prompt = "";
            string[] options = { "Add guest", "Update guest", "Remove guest", "Exit" };
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
    public static void RoomMenu()
    {
        while (true)
        {
            string prompt = "";
            string[] options = { "Add room", "Update room", "Remove room", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:

                    break;

                case 1:
                    SearchRooms();

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
    public static void EmployeeMenu()
    {
        while (true)
        {
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
    private static void AddRoom() // Tomt
    {
    }
    private static (int, string, string) BookingRoom()
    {
        bool isSDCorrect = false;
        bool isEDCorrect = false;
        string startDate = String.Empty;
        string endDate = String.Empty;
        DateOnly sD;
        DateOnly eD;

        string pattern = @"\d{4}(-)\d{2}(-)\d{2}";


        while (!isSDCorrect)
        {
            Console.Write("Start date for your stay [YYYY-MM-DD] : ");
            startDate = Console.ReadLine()!;

            MatchCollection matches = Regex.Matches(startDate, pattern);
            int match = matches.Count;
            if (match == 1)
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
        int duration = (eD.DayNumber - sD.DayNumber);
        return (duration, startDate, endDate);
    }
    public static int ChooseRoom()
    {
        string stringRoom = string.Empty;
        do
        {
            Console.Write($"\nChoose rooms-id to book : ");
            stringRoom = Console.ReadLine();
        } while (!IsStringNumeric(stringRoom));
        return Convert.ToInt32(stringRoom);


        Console.Write("Price : ");
        string totalSum = Console.ReadLine();
        int totalSumConvert = Convert.ToInt32(totalSum);
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
    private static void PrintSearchedRooms(List<Room> roomList)
    {
        Header();
        if (roomList.Count > 0)
        {
            TableUI table = new();
            table.PrintRooms(roomList);
        }
        else
        {
            Console.WriteLine("No rooms found");
        }
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
            Header();
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
    private static void DeleteReservation(int removeID)
    {
        reservations.DeleteReservation(removeID);
        Console.WriteLine($"Reservation with ID : {removeID} has been deleted");
        Console.ReadKey();
    }
    private static int ChooseReservationID()
    {
        string input = String.Empty;
        do
        {
            Console.Write("Choose reservation with ID : ");
            input = Console.ReadLine();
        } while (!IsStringNumeric(input));
        return Convert.ToInt32(input);
    }
    private static (int, string, string) UpdateReservationInfo()
    {
        bool isSDCorrect = false;
        bool isEDCorrect = false;
        string startDate = String.Empty;
        string endDate = String.Empty;
        string pattern = @"\d{4}(-)\d{2}(-)\d{2}";

        Header();

        while (!isSDCorrect)
        {
            Console.Write("Start date for your stay : ");
            startDate = Console.ReadLine();

            MatchCollection matches = Regex.Matches(startDate, pattern);
            int match = matches.Count;
            if (match == 1)
            {
                isSDCorrect = true;
            }
            else
            {
                Console.WriteLine("Please try again, enter YYYY-MM-DD.");
            }
        }
        while (!isEDCorrect)
        {
            Console.Write("End date for your stay : ");
            endDate = Console.ReadLine();
            // int economy = 1; // ÄNDRA MIG 
            // int totalCosts = totalG * duration * economy; // Här skapade Emelie en metod för uträkningen ?
            //                                               // I DB heter det economy = pris, guests = antal gäster, duration = antalet nätter.
            //                                               // 

            MatchCollection matching = Regex.Matches(endDate, pattern);
            int match = matching.Count;
            if (match == 1)
            {
                isEDCorrect = true;
            }
            else
            {
                Console.WriteLine("Please try again, enter YYYY-MM-DD.");
            }
        }

        DateOnly sD = DateOnly.Parse(startDate);
        DateOnly eD = DateOnly.Parse(endDate);
        int duration = (eD.DayNumber - sD.DayNumber);
        return (duration, startDate, endDate);
    }
    private static void UpdateReservation(ReservationDB reservations, Reservation reserv, Customer customer, string startDate, int duration, int reservationID)
    {
        while (true)
        {
            DateTime sD = DateTime.Parse(startDate);
            string stringRoom = string.Empty;
            do
            {
                Console.Write($"\nChoose rooms-id to book : ");
                stringRoom = Console.ReadLine();
            } while (!IsStringNumeric(stringRoom) && !string.IsNullOrEmpty(stringRoom));
            int roomID = Convert.ToInt32(stringRoom);

            Console.Write("Price : ");
            string totalSum = Console.ReadLine();
            int totalSumConvert = Convert.ToInt32(totalSum);

            reserv = new(reservationID, roomID, customer.ID, totalSumConvert, sD, duration);
            reservations.UpdateReservation(reserv);

            Console.WriteLine("Here is your receipt to your reservation");
            TableUI table = new();
            table.PrintUpdatedReceipt(reserv, customer);
            Console.ReadLine();
            return;
        }
    }
    private static void MakeReservation(CustomerManagement custM, ReservationDB reservations, Customer cust, Room room, Reservation reserv)
    {
        try
        {
            int customerID = custM.AddCustomer(cust);
            reserv.customer_id = customerID;
            reserv.id = reservations.CreateRoomReservation(reserv);

            Console.WriteLine("Here is your receipt to your reservation");
            TableUI table = new();
            table.PrintReceipt(reserv, cust);
            Console.ReadLine();
        }
        catch (System.Exception e)
        {

            Console.WriteLine("Error: " + e);
            Console.ReadKey();
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
    private static void EmployeePWCheck()
    {
        bool isPWCorrect = false;
        string correctPW = "SuvNet22";

        while (!isPWCorrect)
        {
            Console.Write("Please enter your password : ");
            string pw = Console.ReadLine()!;

            if (pw == correctPW)
            {
                Console.WriteLine("Correct");
                isPWCorrect = true;
                Thread.Sleep(2000);
            }
            else
            {
                Console.Write("Sorry, that is not correct. ");
            }
        }
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
    }
    private static void ExitMenu()
    {
        Console.WriteLine("Please press any key to exit.");
        Console.ReadKey(true);
        Environment.Exit(0);
    }
}