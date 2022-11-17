class Menu
{
    private int SelectedIndex;
    private string[] Options;
    private string Prompt = "Welcome to Interspace Hotel. Are you a guest or a employee";

    public Menu(string prompt, string[] options)
    {
        Prompt = prompt;
        Options = options;
        SelectedIndex = 0;
    }

    private void DisplayOptions()
    {
        Console.WriteLine(Prompt);
        for (int i = 0; i < Options.Length; i++)
        {
            string currentOption = Options[i];

            if (i == SelectedIndex)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.WriteLine($" << {currentOption} >> ");
        }
        Console.ResetColor();
    }

    public int Run()
    {
        ConsoleKey pressedKey;
        do
        {
            Console.Clear();
            DisplayOptions();

            ConsoleKeyInfo KeyInfo = Console.ReadKey(true);
            pressedKey = KeyInfo.Key;

            if (pressedKey == ConsoleKey.DownArrow)
            {
                SelectedIndex++;
                if (SelectedIndex == Options.Length)
                {
                    SelectedIndex = 0;
                }
            }
            else if (pressedKey == ConsoleKey.UpArrow)
            {
                SelectedIndex--;
                if (SelectedIndex == -1)
                {
                    SelectedIndex = Options.Length - 1;
                }
            }
        }
        while (pressedKey != ConsoleKey.Enter);
        return SelectedIndex;
    }
}