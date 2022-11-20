class TableUI
{
    // https://stackoverflow.com/questions/856845/how-to-best-way-to-draw-table-in-console-app-c
    private int TableWidth;
    public void PrintRooms(List<Room> objectList)
    {
        TableWidth = 85;
        PrintLine();
        PrintRow("ID", "NAME", "BEDS", "GUESTS", "SIZE", "PRICE");
        PrintLine();
        foreach (var item in objectList)
        {
            PrintRow(item.id.ToString(), item.name, item.beds.ToString(), item.guests.ToString(), item.size.ToString(), item.price.ToString());
            PrintLine();
        }
    }
    public void PrintCustomers(List<Customer> objectList)
    {
        TableWidth = 70;
        PrintLine();
        PrintRow("ID", "NAME", "EMAIL", "PHONENUMBER");
        PrintLine();
        foreach (var item in objectList)
        {
            PrintRow(item.ID.ToString(), item.Name, item.Email, item.Phonenumber.ToString());
            PrintLine();
        }
    }

    public void PrintReceipt(Reservation reserv, Customer cust)
    {
        TableWidth = 85;
        PrintLine();
        PrintRow("RESERVATION ID", reserv.id.ToString());
        PrintLine();
        PrintRow("BOOKED ROOM", reserv.room_id.ToString());
        PrintLine();
        PrintRow("CHECK-IN DATE", reserv.date_in.ToString("yyyy-MM-dd"));
        PrintLine();
        PrintRow("NUMBER OF NIGTHS", reserv.duration.ToString());
        PrintLine();
        PrintRow("TOTAL COST", reserv.economy.ToString());
        PrintLine();
        PrintRow("BOOKED BY", cust.Name);
        PrintLine();
    }
    public void PrintUpdatedReceipt(Reservation reservation, Customer customer)
    {
        TableWidth = 85;
        PrintLine();
        PrintRow("RESERVATION ID", reservation.id.ToString());
        PrintLine();
        PrintRow("BOOKED ROOM", reservation.room_id.ToString());
        PrintLine();
        PrintRow("CHECK-IN DATE", reservation.date_in.ToString("yyyy-MM-dd"));
        PrintLine();
        PrintRow("NUMBER OF NIGTHS", reservation.duration.ToString());
        PrintLine();
        PrintRow("TOTAL COST", reservation.economy.ToString());
        PrintLine();
        PrintRow("BOOKED BY", customer.Name);
        PrintLine();
    }
    public void PrintReservations(List<Reservation> reservationList)
    {
        TableWidth = 85;
        PrintLine();
        foreach (var item in reservationList)
        {
            PrintRow("RESERVATION ID", item.id.ToString());
            PrintLine();
            PrintRow("BOOKED ROOM", item.room_id.ToString());
            PrintLine();
            PrintRow("CHECK-IN DATE", item.date_in.ToString("yyyy-MM-dd"));
            PrintLine();
            PrintRow("NUMBER OF NIGTHS", item.duration.ToString());
            PrintLine();
            PrintRow("BOOKED BY", item.customer_id.ToString());
            PrintLine();
        }
    }

    public void PrintEmployees(List<Employee> objectList)
    {
        TableWidth = 50;
        PrintLine();
        PrintRow("ID", "NAME");
        PrintLine();
        foreach (var item in objectList)
        {
            PrintRow(item.id.ToString(), item.name);
            PrintLine();
        }
    }

    private void PrintLine()
    {
        Console.WriteLine(new string('-', TableWidth));
    }

    private void PrintRow(params string[] columns)
    {
        int width = (TableWidth - columns.Length) / columns.Length;
        string row = "|";

        foreach (string column in columns)
        {
            row += AlignCentre(column, width) + "|";
        }

        Console.WriteLine(row);
    }

    private string AlignCentre(string text, int width)
    {
        text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

        if (string.IsNullOrEmpty(text))
        {
            return new string(' ', width);
        }
        else
        {
            return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
        }
    }

    internal void PrintReceipt(int roomID, DateTime sD, int duration, int totalSumConvert, int custID, int reservationID)
    {
        throw new NotImplementedException();
    }
}

