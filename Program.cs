using System.Linq;
using System.Diagnostics;
Console.Clear();
List<List<string>> bankInfo = File.ReadAllLines("Bank.txt").Select(line => line.Split(',').ToList()).ToList();
List<string> userNames = new List<string>();
for (int i = 0; i<bankInfo.Count; i++)
{
    userNames.Add(bankInfo[i][0]);
}
List<string> PINs = new List<string>();
for (int i = 0; i<bankInfo.Count; i++)
{
    PINs.Add(bankInfo[i][0]);
}
List<double> money = new List<double>();
for (int i = 0; i<bankInfo.Count; i++)
{
    double.TryParse(bankInfo[i][0], out double result);
    money.Add(result);
}


Debug.Assert(CorrectlyLoaded(bankInfo));
Console.WriteLine("ATM information successfully loaded!");
Console.WriteLine("You only get three tries to log in, so do it successfully the first time!");
Console.WriteLine("What is your Username? ");
string userName = Console.ReadLine();

//Check Username
int tries = 0;
for (int i = 0; i < 3; i++)
{
    if (CheckUsername(userNames, userName)) break;
    else
    {
        tries ++;
        if (tries >= 3) break;
        Console.Write($"That was an invalid username. You can try {3-tries} more time(s). Please try again.\nPlease enter your username. ");
        userName = Console.ReadLine();
    }
}
Debug.Assert(tries < 3);

//Welcome and Options
Console.Clear();
Console.WriteLine($"Welcome to the ATM machine, {userName}!\nPlease choose an action. Your options are:");
Console.WriteLine("1 - Check Balance\n2 - Withdraw\n3 - Deposit\n4 - Display last 5 transactions\n5 - Quick Withdraw $40\n6 - Quick Withdraw $100\n7 - End current session");
Console.Write("Which would you like to do? Use only numbers. ");
string actionInput = Console.ReadLine()!;
int action = CheckAction(actionInput);

Debug.Assert(action > 0 && action < 8);

switch (action)
{
    case 1:
        Console.WriteLine("You chose 1");
        break;

    case 2:
        Console.WriteLine("You chose 2");
        break;

    case 3:
        Console.WriteLine("You chose 3");
        break;

    case 4:
        Console.WriteLine("You chose 4");
        break;

    case 5:
        Console.WriteLine("You chose 5");
        break;

    case 6:
        Console.WriteLine("You chose 6");
        break;

    case 7:
        Console.WriteLine("You chose 7");
        break;

    default:
        Console.WriteLine("Invalid choice");
        break;
}




/* This is the next part I need to work on
static void CheckBalance(List<double> balances)
{
    
}
*/
static int CheckAction(string actionAttempt)
{
    if (int.TryParse(actionAttempt, out int result) && (result > 0 && result < 8))
    {
        return result;
    }
    else
    {
        Console.Clear();
        Console.WriteLine("That was an invalid action!\nPress any key to try again.");
        Console.ReadKey(true);
        Console.Clear();
        Console.WriteLine("Please choose an action. Your options are:");
        Console.WriteLine("1 - Check Balance Withdraw\n2 - Deposit\n3 - Display last 5 transactions\n4 - Quick Withdraw $40\n5 - Quick Withdraw $100\n6 - End current session");
        Console.Write("Which would you like to do? Use only numbers. ");
        string actionInput = Console.ReadLine()!;
        int action = CheckAction(actionInput);
        return action;
    }
}

static bool CheckUsername(List<string> validNames, string nameAttempt)
{
    for (int i = 0; i<validNames.Count; i++)
    {
        if (nameAttempt == validNames[i])
        {
            return true;
        }
        
    }
    return false;
}

static bool CorrectlyLoaded(List<List<string>> whatLoaded)
{
    string[] originalLines = File.ReadAllLines("Bank.txt");

    for (int i = 0; i < originalLines.Length; i++)
    {
        string reconstructedLine = string.Join(",", whatLoaded[i]);

        if (originalLines[i] != reconstructedLine)
        {
            return false;
        }
    }
    return true;
}
