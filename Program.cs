using MySqlConnector;
using Dapper;
internal class Program
{
    const string CONNECTIONSTRING = "Server = localhost;Database = interspace_hotel;Uid=root; Convert Zero Datetime=True";

    private static void Main(string[] args)
    {
        MySqlConnection connection = new MySqlConnection(CONNECTIONSTRING);
        RoomDB roomDB = new(connection);
        Room listRooms = new();
        //RoomManagement roomManager = new(roomDB);
        EmployeeDB employeeDB = new(connection);
        EmployeeManagement empManager = new(employeeDB);
        ReservationDB reservations = new(connection);

        //MakeReservation(reservations);
        //UpdateReservation(reservations);
        //DeleteReservation(reservations);


        //UpdateRoom(roomDB);
        //RemoveRoombyID(roomDB);

        UpdateEmployee(employeeDB);
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
            Console.WriteLine("ange reservations ID");
            string reservationid = Console.ReadLine();
            int idConvert = Convert.ToInt32(reservationid);
            Reservation reservation = reservations.GetReservationById(idConvert);

            Console.WriteLine("Ange nya rums id:et du vill flytta gästen till");
            string updateRoom = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(updateRoom))
            {
                reservation.room_id = Convert.ToInt32(updateRoom);
            }

            Console.WriteLine("Ange datum ändring");
            string date = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(date))
            {
                reservation.date_in = DateTime.Parse(date);
            }

            Console.WriteLine("Ange antal dagar");
            string duration = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(duration))
            {
                reservation.duration = Convert.ToInt32(duration);
            }

            Console.WriteLine("Ange nytt pris");
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
            Console.WriteLine("Välj rummet du vill boka");
            string roomChoice = Console.ReadLine();
            int roomChoiceConvert = Convert.ToInt32(roomChoice);

            Console.WriteLine("Ange ditt id.");
            string customerID = Console.ReadLine();
            int customerIDConvert = Convert.ToInt32(customerID);

            Console.WriteLine("Ange när du vill reservera rummet. Ange i siffror tex 2022-11-25");
            string dateInput = Console.ReadLine();
            DateTime fromDate = DateTime.Parse(dateInput);

            Console.WriteLine("Du har bokat: " + fromDate);
            Console.WriteLine("Ange hur många dagar du vill stanna.");
            string duration = Console.ReadLine();
            int durationConvert = Convert.ToInt32(duration);

            Console.WriteLine("Ange totalsumma");
            string totalSum = Console.ReadLine();
            int totalSumConvert = Convert.ToInt32(totalSum);
            Console.WriteLine(reservations.ToString());
            int resultat = reservations.CreateRoomReservation(roomChoiceConvert, customerIDConvert, fromDate, durationConvert, totalSumConvert);
            Console.WriteLine("Reservation gjord: " + resultat);
        }
        catch (System.Exception e)
        {

            Console.WriteLine("Fel: " + e);
        }
    }

    private static void UpdateEmployee(EmployeeDB employeeDB)
    {
        Console.WriteLine("Vilken personal vill du uppdatera? skriv in id siffra.");
        string id = Console.ReadLine();
        int employeeIDConvert = Convert.ToInt32(id);
        Employee updateEmployee = employeeDB.GetEmployeeById(employeeIDConvert);

        Console.WriteLine("Skriv in namn");
        string employeeName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(employeeName))
        {
            updateEmployee.name = employeeName;
        }
        Console.WriteLine("Skriv in lösenord");
        string employeePassword = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(employeePassword))
        {
            updateEmployee.password = employeePassword;
        }
        employeeDB.UpdateEmployee(updateEmployee);
    }

    private static void CreateEmployee(EmployeeDB employeeDB)
    {
        Console.WriteLine("Skriv in namn");
        string nameInput = Console.ReadLine();
        Console.WriteLine("Skriv in lösenord");
        string passwordInput = Console.ReadLine();
        var createEmployee = employeeDB.CreateEmployee(nameInput, passwordInput);
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