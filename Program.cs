
using System.Text.RegularExpressions;
using MySqlConnector;
using Dapper;
internal class Program
{
    const string CONNECTIONSTRING = "Server = localhost;Database = interspace_hotel;Uid=root; Convert Zero Datetime=True";
    public static MySqlConnection connection = new MySqlConnection(CONNECTIONSTRING);
    public static Room room = new();
    public static RoomDB roomDB = new(connection);
    public static RoomManagement roomManager = new(roomDB);
    public static EmployeeDB employeeDB = new(connection);
    public static EmployeeManagement empManager = new(employeeDB);
    public static Reservation reservation = new();
    public static ReservationDB reservationDB = new(connection);
    public static ReservationManager reservationManager = new(reservationDB);
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
                case 0:
                    GuestMenu();
                    break;

                case 1:
                    EmployeePWCheck();
                    EmployeeUI();
                    break;
                case 2:
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
            string[] options = { "Reservations", "Customers", "Room", "Return", "Exit" };
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
                    return;

                case 4:
                    ExitMenu();
                    break;
                default:
                    break;
            }
        }
    }
    public static void GuestMenu()
    {
        while (true)
        {
            string prompt = "";
            string[] options = { "Make reservation", "List all rooms", "Return" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    var search = BookingRoom();
                    reservation.duration = search.Item1;
                    DateTime startDate = Convert.ToDateTime(search.Item2);
                    reservation.date_in = startDate;
                    int bookGuests = NumberOFGuests();

                    roomList = roomManager.GetAvailableRoomsForBooking(bookGuests, search.Item2, search.Item3);
                    PrintRooms(roomList);
                    int roomID = ChooseRoom();
                    room = roomManager.GetRoomByID(roomID);
                    reservation.room_id = room.id;
                    reservation.economy = reservationManager.CalculateTotalCost(reservation, room, bookGuests);

                    customer = AddCustomer();
                    customer.ID = custManager.AddCustomer(customer);
                    reservation.customer_id = customer.ID;
                    reservation.id = reservationManager.Reservation(reservation);
                    PrintReservation(customer, reservation);
                    break;

                case 1:
                    roomList = roomDB.GetRooms();
                    PrintRooms(roomList);
                    Console.ReadKey();
                    break;

                case 2:
                    return;
            }
        }

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
                    var search = BookingRoom();
                    reservation.duration = search.Item1;
                    DateTime startDate = Convert.ToDateTime(search.Item2);
                    reservation.date_in = startDate;
                    int bookGuests = NumberOFGuests();

                    roomList = roomManager.GetAvailableRoomsForBooking(bookGuests, search.Item2, search.Item3);
                    PrintRooms(roomList);
                    int roomID = ChooseRoom();
                    room = roomManager.GetRoomByID(roomID);
                    reservation.room_id = room.id;
                    reservation.economy = reservationManager.CalculateTotalCost(reservation, room, bookGuests);

                    customer = AddCustomer();
                    customer.ID = custManager.AddCustomer(customer);
                    reservation.customer_id = customer.ID;
                    reservation.id = reservationManager.Reservation(reservation);
                    PrintReservation(customer, reservation);
                    break;

                case 1: // update reservation
                    reservationList = reservationManager.GetReservationByString(SearchReservation());
                    reservationList = reservationManager.GetReservationByCustomerID(reservationList);
                    PrintReservations(reservationList);

                    int updateID = ChooseReservationID(reservationList);
                    int custID = custManager.GetIDFromReservation(updateID);
                    reservation = reservationDB.GetReservationById(updateID);

                    string endDate = reservationManager.CalculateEndDate(reservation);
                    reservation = UpdateStartDate(reservation);
                    reservation = UpdateEndDate(reservation, endDate);
                    string updateEndDate = reservationManager.CalculateEndDate(reservation);
                    string updateDate = reservation.date_in.ToString();

                    int updateGuests = NumberOFGuests();
                    roomList = roomManager.GetAvailableRoomsForBooking(updateGuests, updateDate, updateEndDate);
                    PrintRooms(roomList);
                    int updateRoomID = ChooseRoom();
                    room = roomManager.GetRoomByID(updateRoomID);

                    customer = custManager.GetCustomerFromReservationID(updateID);
                    reservation = reservationManager.GetReservationsByID(updateID);
                    reservation.economy = reservationManager.CalculateTotalCost(reservation, room, updateGuests);
                    UpdateReservation(reservationDB, reservation, customer, room);
                    break;

                case 2: // remove reservation
                    reservationList = reservationDB.SearchReservationByString(SearchReservation());
                    reservationList = reservationDB.SelectReservations(reservationList);
                    PrintReservations(reservationList);
                    int removeID = ChooseReservationID(reservationList);
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
            string[] options = { "Add guest", "Update guest", "Return", "Exit" };
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
                    return;

                case 3:
                    ExitMenu();
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
                    roomList = roomDB.GetRooms();
                    PrintRooms(roomList);
                    Console.ReadKey();

                    int ID = SearchRooms();
                    room = roomDB.GetRoomByid(ID);

                    room = UpdateRoom(room);
                    roomDB.UpdateRoom(room);
                    UpdateRoomSuccess(ID);

                    break;

                case 2: // print
                    roomList = roomDB.GetRooms();
                    PrintRooms(roomList);
                    Console.ReadKey();
                    break;

                case 3: // remove
                    roomList = roomDB.GetRooms();
                    PrintRooms(roomList);
                    Console.ReadKey();
                    RemoveRoombyID();

                    break;

                case 4: // exit
                    return;

                case 5:
                    ExitMenu();
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
        bool isStartDateCorrect = false;
        bool isEndDateCorrect = false;
        bool isDateCorrect = false;
        int duration = 0;
        string startDate = String.Empty;
        string endDate = String.Empty;
        DateOnly fromDate;
        DateOnly toDate;

        string pattern = @"\d{4}(-)\d{2}(-)\d{2}";

        while (!isDateCorrect)
        {
            while (!isStartDateCorrect)
            {
                Header();
                Console.Write("Start date for your stay [YYYY-MM-DD] : ");
                startDate = Console.ReadLine()!;

                MatchCollection matches = Regex.Matches(startDate, pattern);
                int match = matches.Count;
                if (match == 1)
                {
                    try
                    {
                        fromDate = DateOnly.Parse(startDate);
                        isStartDateCorrect = true;
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
            while (!isEndDateCorrect)
            {
                Console.Write("End date for your stay [YYYY-MM-DD] : ");
                endDate = Console.ReadLine();

                MatchCollection matching = Regex.Matches(endDate, pattern);
                int match = matching.Count;
                if (match == 1)
                {
                    try
                    {
                        toDate = DateOnly.Parse(endDate);
                        isEndDateCorrect = true;
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
            duration = (toDate.DayNumber - fromDate.DayNumber);

            if (duration >= 1)
            {
                isDateCorrect = true;
            }
            else
            {
                Console.WriteLine("Please try again, type the dates in right order.");
                Console.ReadKey();
                isStartDateCorrect = false;
                isEndDateCorrect = false;
            }
        }
        return (duration, startDate, endDate);
    }
    public static int ChooseRoom() // NO HEADER!!
    {
        string stringRoom = string.Empty;
        do
        {
            Console.Write($"\nChoose room by ID: ");
            stringRoom = Console.ReadLine();
        } while (!IsStringNumeric(stringRoom));
        return Convert.ToInt32(stringRoom);
    }
    private static int SearchRooms()
    {
        string search = string.Empty;
        do
        {
            Header();
            Console.Write("Search room : ");
            search = Console.ReadLine();
        } while (string.IsNullOrEmpty(search));
        return Convert.ToInt32(search);
    }
    private static Room UpdateRoom(Room room) // MODIFIERA!!
    {
        Console.Write("Beds : ");
        string Beds = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(Beds))
        {
            room.beds = Convert.ToInt32(Beds);
        }

        Console.Write("Max amount of guests : ");
        string guests = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(guests))
        {
            room.guests = Convert.ToInt32(guests);
        }

        Console.WriteLine("Roomsize : ");
        string size = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(size))
        {
            room.size = Convert.ToInt32(size);
        }

        Console.WriteLine("Price : ");
        string price = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(price))
        {
            room.price = Convert.ToInt32(price);
        }

        return room;
    }
    private static void PrintRooms(List<Room> roomList)
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
    private static void RemoveRoombyID() // NO HEADER!!
    {
        Console.WriteLine("Please state the ID of the room you would like to delete:");
        string idRemove = Console.ReadLine();
        int idRemoveConvert = Convert.ToInt32(idRemove);
        roomDB.DeleteRoom(idRemoveConvert);
        Console.WriteLine($"Room with ID : {idRemoveConvert} deleted:");
        Console.ReadLine();
    }
    public static void UpdateRoomSuccess(int ID) // NO HEADER!!
    {
        Console.WriteLine($"Room updated with ID : {ID}");
        Console.ReadLine();
    }
    public static int NumberOFGuests()
    {
        string userInput = string.Empty;
        do
        {
            Console.Write("Guests : ");
            userInput = Console.ReadLine();
        } while (!IsStringNumeric(userInput));
        return Convert.ToInt32(userInput);
    }
    public static int UpdateNumberOfGuests()
    {
        int update = 0;
        string userInput = string.Empty;
        Console.Write("Guests : ");
        if (IsStringNumeric(userInput))
        {
            update = Convert.ToInt32(userInput);
        }
        userInput = Console.ReadLine();
        return Convert.ToInt32(userInput);
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
            } while (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName));
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
    private static void CustomerAddedSuccess(int ID) // NO HEADER!!
    {
        Console.WriteLine($"Customer added with ID : {ID}");
        Console.ReadLine();
    }
    public static int ChooseCustomer() // NO HEADER!!
    {
        string stringCustomer = string.Empty;
        do
        {
            Console.Write($"\nChoose customer by ID : ");
            stringCustomer = Console.ReadLine();
        } while (!IsStringNumeric(stringCustomer));
        return Convert.ToInt32(stringCustomer);
    }
    private static Customer UpdateCustomer(Customer customer, int id) // DENNA ÄR KNAS! TITTA PÅ DEN!
    {
        string firstName;
        string lastName;
        string email;
        string phoneNumber;
        while (true)
        {
            Header();
            do
            {
                Console.Write("Firstname : ");
                firstName = Console.ReadLine();
                Console.Write("Lastname : ");
                lastName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
                {
                    customer.Name = (firstName + " " + lastName);
                }
            } while (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName)
                    || string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName)
                    || !string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName)
                    || string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName));
            // DENNA ÄR JÄTTE KNAS! MAN SKA ALLTSÅ KOMMA UR LOOPEN,
            // OM : Båda variablerna är tomma

            do
            {
                Console.Write("Email : ");
                email = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(email) && IsEmailValid(email))
                {
                    customer.Email = email;
                }
            } while (string.IsNullOrWhiteSpace(email) || !IsEmailValid(email));

            do
            {
                Console.Write("Phonenumber : ");
                phoneNumber = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(phoneNumber) && IsStringNumeric(phoneNumber))
                {
                    customer.Phonenumber = phoneNumber;
                }
            } while (string.IsNullOrWhiteSpace(phoneNumber) || !IsStringNumeric(phoneNumber));
            return customer;
        }
    }
    public static void UpdateCustomerSuccess(int ID) // NO HEADER!!
    {
        Console.WriteLine($"Customer added with ID : {ID}");
        Console.ReadLine();
    }
    private static string SearchCustomer()
    {
        string search = string.Empty;
        do
        {
            Header();
            Console.Write("Search customer : ");
            search = Console.ReadLine();
        } while (string.IsNullOrEmpty(search));
        return search;
    }
    private static void PrintCustomers(List<Customer> customerList)
    {
        Header();
        TableUI table = new();
        table.PrintCustomers(customerList);
        Console.ReadLine();
    }
    private static void RemoveCustomer(int removeID)
    {
        reservationDB.DeleteReservation(removeID);
        Console.WriteLine($"Customer with ID : {removeID} has been deleted");
        Console.ReadKey();
    }
    private static string SearchReservation()
    {
        string search = string.Empty;
        do
        {
            Header();
            Console.Write("Search reservation : ");
            search = Console.ReadLine();
        } while (string.IsNullOrEmpty(search));
        return search;
    }
    private static void PrintReservations(List<Reservation> reservationsList)
    {
        Header();
        TableUI table = new();
        table.PrintReservations(reservationList);
        Console.ReadLine();
    }
    private static void DeleteReservation(int removeID) // NO HEADER!!
    {
        reservationDB.DeleteReservation(removeID);
        Console.WriteLine($"Reservation with ID : {removeID} has been deleted");
        Console.ReadKey();
    }
    private static int ChooseReservationID(List<Reservation> reservationList) // NO HEADER!!
    {
        string input = String.Empty;
        int convert = 0;
        while (true)
        {
            Console.Write("Choose reservation with ID : ");
            input = Console.ReadLine();
            if (IsStringNumeric(input))
            {
                convert = Convert.ToInt32(input);
                foreach (var reservation in reservationList)
                {
                    if (reservation.id == convert)
                    {
                        return convert;
                    }
                }
            }
        }
    }
        private static void UpdateReservation(ReservationDB reservations, Reservation reservation, Customer customer, Room room)
    {
        while (true)
        {
            reservation = new(reservation.id, room.id, customer.ID, reservation.economy, reservation.date_in, reservation.duration);
            reservations.UpdateReservation(reservation);

            Console.WriteLine("Here is your receipt to your reservation");
            TableUI table = new();
            table.PrintUpdatedReceipt(reservation, customer);
            Console.ReadLine();
            return;
        }
    }
    private static void PrintReservation(Customer customer, Reservation reservation)
    {
        Header();
        Console.WriteLine("Here is your receipt to your reservation");
        TableUI table = new();
        table.PrintReceipt(reservation, customer);
        Console.ReadLine();
    }
    private static Reservation CalculateReservationPrice(Reservation reservation, Room room, int guests)
    {
        int economy = 0;
        if (reservation.room_id == room.id)
        {
            economy = ((room.price * reservation.duration) * guests);
            reservation.economy = economy;
        }
        return reservation;
    }
    private static Reservation UpdateStartDate(Reservation reservation)
    {
        bool isStartDateCorrect = false;
        string startDate = String.Empty;
        string endDate = String.Empty;
        string updatedStartDate = string.Empty;
        string updatedEndDate = string.Empty;
        DateTime fromDate;
        DateTime toDate;
        string pattern = @"\d{4}(-)\d{2}(-)\d{2}";

        while (!isStartDateCorrect)
        {
            Header();
            Console.WriteLine(reservation.date_in.ToString());
            Console.Write("Update Start date for your stay : ");
            startDate = Console.ReadLine();
            ConsoleKeyInfo key = Console.ReadKey();

            MatchCollection matches = Regex.Matches(startDate, pattern);
            int match = matches.Count;
            if (match == 1)
            {
                isStartDateCorrect = true;
                if (!string.IsNullOrWhiteSpace(startDate))
                {
                    DateTime updateStartDate = Convert.ToDateTime(startDate);
                    reservation.date_in = updateStartDate;
                }
            }
            else if (string.IsNullOrEmpty(startDate) && (key.Key.Equals(ConsoleKey.Enter)))
            {
                isStartDateCorrect = true;
            }
            else
            {
                Console.WriteLine("Please try again, enter YYYY-MM-DD.");
            }
        }

        if (string.IsNullOrWhiteSpace(startDate))
        {
            return reservation;
        }
        if (!string.IsNullOrWhiteSpace(startDate))
        {
            fromDate = Convert.ToDateTime(startDate);
            reservation.date_in = fromDate;
        }
        return reservation;
    }
    private static Reservation UpdateEndDate(Reservation reservation, string updateDate)
    {
        bool isDateCorrect = false;
        bool isEndDateCorrect = false;
        string startDate = String.Empty;
        string endDate = String.Empty;
        string updatedStartDate = string.Empty;
        string updatedEndDate = string.Empty;
        DateOnly fromDate;
        DateOnly toDate;
        int duration = 0;
        string pattern = @"\d{4}(-)\d{2}(-)\d{2}";

        while (!isDateCorrect)
        {
            while (!isEndDateCorrect)
            {
                DateTime bookedStartDate = reservation.date_in;
                DateTime bookedEndDate = bookedStartDate.AddDays(reservation.duration);

                Console.WriteLine(updateDate);
                Console.Write("Update enddate for your stay : ");
                endDate = Console.ReadLine();
                ConsoleKeyInfo key = Console.ReadKey();

                MatchCollection matching = Regex.Matches(endDate, pattern);
                int match = matching.Count;
                if (match == 1)
                {
                    isEndDateCorrect = true;
                    if (!string.IsNullOrWhiteSpace(endDate))
                    {
                        isEndDateCorrect = true;
                    }
                }
                else if (string.IsNullOrWhiteSpace(endDate) && (key.Key.Equals(ConsoleKey.Enter)))
                {
                    isEndDateCorrect = true;
                }
                else
                {
                    Console.WriteLine("Please try again, enter YYYY-MM-DD.");
                }
            }

            if (string.IsNullOrWhiteSpace(endDate))
            {
                return reservation;
            }
            if (!string.IsNullOrWhiteSpace(endDate))
            {
                updatedStartDate = reservation.date_in.ToString("yyyy-MM-dd");
                fromDate = DateOnly.Parse(updatedStartDate);
                toDate = DateOnly.Parse(endDate);
                duration = (toDate.DayNumber - fromDate.DayNumber);

                if (duration >= 1)
                {
                    isDateCorrect = true;
                }
                else
                {
                    Console.WriteLine("Please try again, type the dates in right order.");
                    isEndDateCorrect = false;
                }
            }
        }
        return reservation;
    }
    private static void UpdateEmployee(EmployeeDB employeeDB) // MODIFIERA!!
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
    private static void CreateEmployee(EmployeeDB employeeDB) // MODIFIERA!!
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
                Thread.Sleep(1300);
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