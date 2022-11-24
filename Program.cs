
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
                    bool found = PrintRooms(roomList);
                    if (found == false) break;

                    int roomID = ChooseRoom(roomList);
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
                    found = PrintRooms(roomList);
                    if (found == false) break;
                    Console.ReadKey();
                    break;

                case 2:
                    return;
            }
        }

    }
    public static void ReservationsMenu()
    {
        while (true)
        {
            string prompt = "";
            string[] options = { "Add reservation", "Update reservation", "Remove reservation", "Return", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0: //Add reservation
                    var search = BookingRoom();
                    reservation.duration = search.Item1;
                    DateTime startDate = Convert.ToDateTime(search.Item2);
                    reservation.date_in = startDate;
                    int bookGuests = NumberOFGuests();

                    roomList = roomManager.GetAvailableRoomsForBooking(bookGuests, search.Item2, search.Item3);
                    bool found = PrintRooms(roomList);
                    if (found == false) break;

                    int roomID = ChooseRoom(roomList);
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
                    found = PrintReservations(reservationList);
                    if (found == false) break;

                    int updateID = ChooseReservationID(reservationList);
                    int custID = custManager.GetIDFromReservation(updateID);
                    reservation = reservationDB.GetReservationById(updateID);

                    string endDate = reservationManager.CalculateEndDate(reservation);
                    reservation.date_in = UpdateStartDate(reservation);
                    reservation = UpdateEndDate(reservation, endDate);
                    string updateEndDate = reservationManager.CalculateEndDate(reservation);

                    string updateDate = reservation.date_in.ToString();

                    int updateGuests = NumberOFGuests();
                    roomList = roomManager.GetAvailableRoomsForBooking(updateGuests, updateDate, updateEndDate);
                    found = PrintRooms(roomList);
                    if (found == false) break;

                    int updateRoomID = ChooseRoom(roomList);
                    room = roomManager.GetRoomByID(updateRoomID);

                    customer = custManager.GetCustomerFromReservationID(updateID);
                    reservation = reservationManager.GetReservationsByID(updateID);
                    reservation.economy = reservationManager.CalculateTotalCost(reservation, room, updateGuests);
                    UpdateReservation(reservationDB, reservation, customer, updateDate, room);
                    break;

                case 2: // remove reservation
                    reservationList = reservationDB.SearchReservationByString(SearchReservation());
                    reservationList = reservationDB.SelectReservations(reservationList);
                    found = PrintReservations(reservationList);
                    if (found == false) break;
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
    public static void CustomersMenu()
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
                    customerList = custManager.StringSearchCustomer(SearchCustomer());
                    bool found = PrintCustomers(customerList);
                    if (found == false) break;

                    int customerID = ChooseCustomer(customerList);
                    customer = custManager.GetCustomer(customerID);

                    customer = UpdateCustomer(customer);
                    custManager.UpdateCustomer(customer);
                    UpdateCustomerSuccess(customerID);
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
                    bool found = PrintRooms(roomList);
                    if (found == false) break;
                    Console.ReadKey();
                    break;

                case 1: // update
                    roomList = roomManager.GetRooms();
                    found = PrintRooms(roomList);
                    if (found == false) break;
                    int ID = SearchRooms(roomList);
                    room = roomDB.GetRoomByid(ID);

                    room = UpdateRoom(room);
                    roomDB.UpdateRoom(room);
                    UpdateRoomSuccess(ID);

                    break;

                case 2: // print
                    roomList = roomDB.GetRooms();
                    found = PrintRooms(roomList);
                    if (found == false) break;
                    Console.ReadKey();
                    break;

                case 3: // remove
                    roomList = roomDB.GetRooms();
                    found = PrintRooms(roomList);
                    if (found == false) break;
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
    private static List<Room> AddRoom(Room room)
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

        DateTime fromToday = DateTime.Today;
        DateOnly testing;
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
                        testing = DateOnly.FromDateTime(fromToday);

                        if (fromDate >= testing)
                        {
                            isStartDateCorrect = true;
                        }
                        else
                        {
                            Console.WriteLine("Please try again, enter valid dates.");
                            Console.ReadLine();
                        }
                    }
                    catch (System.Exception)
                    {
                        Console.WriteLine("Please try again, enter valid dates.");
                        Console.ReadLine();
                    }
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

    public static int ChooseRoom(List<Room> roomList) // NO HEADER!!
    {
        string input = String.Empty;
        int convert = 0;
        bool isReservationExisting = false;
        while (!isReservationExisting)
        {
            Console.Write("Choose room with ID : ");
            input = Console.ReadLine();

            try
            {
                if (IsStringNumeric(input))
                {
                    convert = Convert.ToInt32(input);
                    foreach (var room in roomList)
                    {
                        if (room.id == convert)
                        {
                            isReservationExisting = true;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                Console.WriteLine("You need to write a number.");
                isReservationExisting = false;
            }
        }
        return convert;
    }
    private static int SearchRooms(List<Room> roomList)
    {
        string input = String.Empty;
        int convert = 0;
        bool isReservationExisting = false;
        while (!isReservationExisting)
        {
            Console.Write("Choose room with ID : ");
            input = Console.ReadLine();

            try
            {
                if (IsStringNumeric(input))
                {
                    convert = Convert.ToInt32(input);
                    foreach (var room in roomList)
                    {
                        if (room.id == convert)
                        {
                            isReservationExisting = true;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                Console.WriteLine("You need to write a number.");
                isReservationExisting = false;
            }
        }
        return convert;
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
    private static bool PrintRooms(List<Room> roomList)
    {
        Header();
        if (roomList.Count > 0)
        {
            TableUI table = new();
            table.PrintRooms(roomList);
            return true;
        }
        else
        {
            Console.WriteLine("No customers found");
            Console.ReadLine();
            return false;
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
            string pattern = @"\d{10}";
            bool isPhoneNrValid = false;
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

            while (isPhoneNrValid == false)
            {
                Console.Write("Phonenumber : ");
                phoneNumber = Console.ReadLine()!;

                MatchCollection matches = Regex.Matches(phoneNumber, pattern);
                int match = matches.Count;
                if (!string.IsNullOrWhiteSpace(phoneNumber) && IsStringNumeric(phoneNumber) && match == 1)
                {
                    customer.Phonenumber = phoneNumber;
                    isPhoneNrValid = true;
                }
                else
                {
                    Console.WriteLine("Please enter a valid phonenumber, 10 digits.");
                }
            }
            return new(email, firstName + " " + lastName, phoneNumber);
        }
    }
    private static void CustomerAddedSuccess(int ID) // NO HEADER!!
    {
        Console.WriteLine($"Customer added with ID : {ID}");
        Console.ReadLine();
    }

    public static int ChooseCustomer(List<Customer> customerList) // NO HEADER!!
    {
        string input = String.Empty;
        int convert = 0;
        bool isReservationExisting = false;
        while (!isReservationExisting)
        {
            Console.Write("Choose customer with ID : ");
            input = Console.ReadLine();

            try
            {
                if (IsStringNumeric(input))
                {
                    convert = Convert.ToInt32(input);
                    foreach (var customer in customerList)
                    {
                        if (customer.ID == convert)
                        {
                            isReservationExisting = true;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                Console.WriteLine("You need to write a number.");
                isReservationExisting = false;
            }
        }
        return convert;
    }

    private static Customer UpdateCustomer(Customer customer) // DENNA ÄR KNAS! TITTA PÅ DEN!
    {
        string firstName;
        string lastName;
        string email;
        string phoneNumber;

        int index = customer.Name.IndexOf(" ");
        string fullName = customer.Name;
        string beforeIndex = fullName.Substring(0, index);
        string afterIndex = fullName.Substring(index + 1);

        string pattern = @"\d{10}";
        bool isPhoneNrValid = false;


        while (true)
        {
            Header();
            Console.Write("Firstname : ");
            firstName = Console.ReadLine();
            if (!string.IsNullOrEmpty(firstName))
            {
                beforeIndex = firstName;
            }

            Console.Write("Lastname : ");
            lastName = Console.ReadLine();
            if (!string.IsNullOrEmpty(lastName))
            {
                afterIndex = lastName;
            }

            customer.Name = $"{beforeIndex} {afterIndex}";

            while (true)
            {
                Console.Write("Email : ");
                email = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(email) && IsEmailValid(email))
                {
                    customer.Email = email;
                    break;
                }
                else if (string.IsNullOrWhiteSpace(email)) break;
            }

            while (isPhoneNrValid == false)
            {
                Console.Write("Phonenumber : ");
                phoneNumber = Console.ReadLine()!;

                MatchCollection matches = Regex.Matches(phoneNumber, pattern);
                int match = matches.Count;
                if (!string.IsNullOrWhiteSpace(phoneNumber) && IsStringNumeric(phoneNumber) && match == 1)
                {
                    customer.Phonenumber = phoneNumber;
                    isPhoneNrValid = true;
                }
                else if (string.IsNullOrWhiteSpace(phoneNumber)) isPhoneNrValid = true;

                else
                {
                    Console.WriteLine("Please enter a valid phonenumber, 10 digits.");
                }
            }
            return customer;
        }
    }

    public static void UpdateCustomerSuccess(int id) // NO HEADER!!
    {
        Console.WriteLine($"Guest updated with ID : {id}");
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

    private static bool PrintCustomers(List<Customer> customerList)
    {
        Header();
        if (customerList.Count > 0)
        {
            TableUI table = new();
            table.PrintCustomers(customerList);
            return true;
        }
        else
        {
            Console.WriteLine("No customers found");
            Console.ReadLine();
            return false;
        }
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

    private static bool PrintReservations(List<Reservation> reservationsList)
    {
        Header();
        if (reservationList.Count > 0)
        {
            TableUI table = new();
            table.PrintReservations(reservationList);
            return true;
        }
        else
        {
            Console.WriteLine("No reservations found");
            Console.ReadLine();
            return false;
        }
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
        bool isReservationExisting = false;
        while (!isReservationExisting)
        {
            Console.Write("Choose reservation with ID : ");
            input = Console.ReadLine();

            try
            {
                if (IsStringNumeric(input))
                {
                    convert = Convert.ToInt32(input);
                    foreach (var reservation in reservationList)
                    {
                        if (reservation.id == convert)
                        {
                            isReservationExisting = true;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                Console.WriteLine("You need to write a number.");
                isReservationExisting = false;
            }
        }
        return convert;
    }

    private static void UpdateReservation(ReservationDB reservations, Reservation reservation, Customer customer, string startDate, Room room)
    {
        while (true)
        {
            DateTime sD = DateTime.Parse(startDate);

            reservation = new(reservation.id, room.id, customer.ID, reservation.economy, sD, reservation.duration);
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

    private static DateTime UpdateStartDate(Reservation reservation)
    {
        bool isStartDateCorrect = false;
        string startDate = String.Empty;
        string endDate = String.Empty;
        string updatedStartDate = string.Empty;
        string updatedEndDate = string.Empty;

        DateTime fromToday = DateTime.Today;
        DateOnly testing;
        DateTime fromDate;
        DateTime toDate;
        string pattern = @"\d{4}(-)\d{2}(-)\d{2}";

        while (!isStartDateCorrect)
        {
            Header();
            Console.WriteLine("Booked date : " + reservation.date_in.ToString("yyyy-MM-dd"));
            Console.Write("Update incheck for your stay : ");
            startDate = Console.ReadLine();
            ConsoleKeyInfo key = Console.ReadKey();

            MatchCollection matches = Regex.Matches(startDate, pattern);
            int match = matches.Count;
            if (match == 1)
            {
                isStartDateCorrect = true;
                if (!string.IsNullOrWhiteSpace(startDate))
                {
                    try
                    {
                        DateOnly newDate = DateOnly.Parse(startDate);
                        testing = DateOnly.FromDateTime(fromToday);

                        if (newDate >= testing)
                        {
                            DateTime updateStartDate = Convert.ToDateTime(startDate);
                            reservation.date_in = updateStartDate;
                            isStartDateCorrect = true;
                        }
                        else
                        {
                            Console.WriteLine("Please try again, enter valid dates.");
                            Console.ReadLine();
                            isStartDateCorrect = false;
                        }
                    }
                    catch (System.Exception)
                    {
                        Console.WriteLine("Please try again, enter valid dates.");
                        Console.ReadLine();
                        isStartDateCorrect = false;
                    }
                }
            }
            else if (string.IsNullOrEmpty(startDate) && (key.Key.Equals(ConsoleKey.Enter)))
            {
                return reservation.date_in;
                isStartDateCorrect = true;
            }
            else
            {
                Console.WriteLine("Please try again, enter YYYY-MM-DD.");
            }
        }
        return reservation.date_in;
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

                Console.WriteLine($"\nBooked check-out date : " + updateDate);
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

    private static void EmployeePWCheck()
    {
        bool isPWCorrect = false;
        string correctPW = "SuvNet22";
        int countTry = 1;

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
            else if (countTry == 3)
            {
                Console.WriteLine("You have now tried 3 times and you will go back to the menu.");
                Thread.Sleep(1300);
                string prompt = "";
                string[] options = { "Guest", "Employee", "Exit" };
                Menu mainMenu = new Menu(prompt, options);
                int selectedIndex = mainMenu.Run();
            }
            else
            {
                Console.Write("Sorry, that is not correct. ");
                countTry++;
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