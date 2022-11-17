class UI
{
      public void Start()
    {
        RunMainMenu();
        string title = "Interspace Hotel";
    }

    private void RunMainMenu()
    {
        string prompt = @"Welcome to Interspace Hotel. Are you a guest or a employee? 
      Cycle through the options using arrow keys and press enter to select your option.";
        string[] options = { "Guest", "Employee", "Exit" };
        Menu mainMenu = new Menu(prompt, options);
        int selectedIndex = mainMenu.Run();

        switch (selectedIndex)
        {
            case 0:
                GuestMenu();
                break;

            case 1:
                EmployeeMenu();
                break;

            case 2:
                ExitMenu();
                break;

            default:
                break;
        }
    }

    private void GuestMenu()
    {
        Console.WriteLine("Guestmenu will display here");
    }

    private void EmployeeMenu()
    {
        Console.WriteLine("Employeemenu will display here");
    }

    private void ExitMenu()
    {
        Console.WriteLine("Please press any key to exit.");
        Console.ReadKey(true);
        Environment.Exit(0);
    }


    //     userInput = Console.ReadKey();
    //     public void MainMenu()
    //     {
    //     switch(Console.ReadKey()) 
    // {
    //   case 1:
    //     // bokning av ett rum
    //      // - 1 visa lediga rum
    //      // - 2 söka bland lediga rum
    //       // -  Boka rummet
    //            // Som gäst
    //            // Som personal
    //         // - Bekräftelse av bokning.
    //     break;
    //   case 2:
    //   // Personalens inlogg
    //      // [1] visa bokat rum
    //           // . Skriv in beställningsid.
    //           // - Bekräftelse ska visas
    //           // 1 - Ändra bokningen
    //           // 2 Avsluta/ångra bokningen.

    //       // [2] Visa alla bokade rum
    //       // [3]Visa alla lediga rum
    //     break;
    //   default:
    //     // code block
    //     break;
    // }
    //     }


}