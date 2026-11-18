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
    PINs.Add(bankInfo[i][1]);
}
List<double> money = new List<double>();
for (int i = 0; i<bankInfo.Count; i++)
{
    double.TryParse(bankInfo[i][2], out double result);
    money.Add(result);
}


Debug.Assert(CorrectlyLoaded(bankInfo));
Console.WriteLine("ATM information successfully loaded!");
Console.WriteLine("You only get three tries to log in, so do it successfully the first time!");
Console.WriteLine("What is your Username? ");
string userName = Console.ReadLine()!;

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
        userName = Console.ReadLine()!;
    }
}
Debug.Assert(tries < 3);
int currentUserPosition = -1;
for (int i = 0; i < userNames.Count; i++)
{
    if (userNames[i]==userName)
    {
        currentUserPosition = i;
    }
}
Debug.Assert(currentUserPosition >=0 && currentUserPosition < userNames.Count);

//PIN
tries = 0;
string inputPIN = "";
for (int i = 0; i<3; i++)
{
    
    Console.Clear();
    if (inputPIN != PINs[currentUserPosition] && tries < 3)
    {
        tries ++;
        if (tries > 1)
        {
            Console.Write($"That was an invalid PIN! You can try {4-tries} more time(s). Please try again.\nPlease enter your PIN. ");
            inputPIN = "";
        }
        else Console.WriteLine($"{userName}, please enter your PIN.");
        for (int x = 0; x<5; x++)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            inputPIN += key.KeyChar;
            Console.Write("*");
        }
    }
}

Debug.Assert(inputPIN == PINs[currentUserPosition]);

List<string> transactions = File.ReadLines("Transactions.txt").ToList();


//Welcome and Options
bool sessionGoing = true;
double currentBalance = CheckBalance(money, currentUserPosition);
double previousBalance;
string transactionInfo;
double depositAmount;
double widthdrawalAmount;


while(sessionGoing)
{
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
            Console.Clear();
            Console.WriteLine($"{userName}, your current balance is ${currentBalance}.");
            break;

        case 2:
            previousBalance = currentBalance;
            currentBalance = Widthdraw(money[currentUserPosition]);
            Debug.Assert(currentBalance <= previousBalance);
            Console.Clear();
            money[currentUserPosition] = currentBalance;
            bankInfo[currentUserPosition][2] = $"{currentBalance}";
            List<string> lines = new List<string>();
            foreach (List<string> row in bankInfo)
            {
                lines.Add(string.Join(",", row));
            }
            File.WriteAllLines("Bank.txt", lines);

            widthdrawalAmount =  previousBalance - currentBalance;
            transactionInfo = $"{userName} {RememberTransaction("widthdrew", widthdrawalAmount)}";
            transactions.Add(transactionInfo);
            File.WriteAllLines("Transactions.txt", transactions);


            Console.WriteLine($"Your new balance is ${currentBalance}.");
            Console.Write("Press any key to continue. ");
            Console.ReadKey(true);
            break;

        case 3:
            previousBalance = currentBalance;
            currentBalance = Deposit(money[currentUserPosition]);
            Debug.Assert(currentBalance >= previousBalance);
            Console.Clear();
            money[currentUserPosition] = currentBalance;
            bankInfo[currentUserPosition][2] = $"{currentBalance}";
            lines = new List<string>();
            foreach (List<string> row in bankInfo)
            {
                lines.Add(string.Join(",", row));
            }
            File.WriteAllLines("Bank.txt", lines);

            depositAmount = currentBalance - previousBalance;
            transactionInfo = $"{userName} {RememberTransaction("deposited", depositAmount)}";
            transactions.Add(transactionInfo);
            File.WriteAllLines("Transactions.txt", transactions);

            Console.WriteLine($"Your new balance is ${currentBalance}.");
            Console.Write("Press any key to continue. ");
            Console.ReadKey(true);
            break;

        case 4:
            Console.Clear();
            Console.WriteLine("These are the last five transactions:");
            List<string> currentUserTransactions = new List<string>();

            foreach (string transaction in transactions)
            {
                if (transaction.StartsWith(userName + " "))
                {
                    currentUserTransactions.Add(transaction);
                }
            }
            if (currentUserTransactions.Count < 5)
            {
                for (int i = currentUserTransactions.Count-1; i >= 0; i--)
                {
                    Console.WriteLine(currentUserTransactions[i]);
                }
                Console.WriteLine($"There is no more transaction history for {userName}.");
            }
            else
            {
                for (int i = currentUserTransactions.Count - 1; i >= currentUserTransactions.Count - 5; i--)
                {
                    Console.WriteLine(currentUserTransactions[i]);
                }
            }
            Console.WriteLine($"If you you would like to know your total balance, use the check balance option to see it.");
            Console.Write("Press any key to continue. ");
            Console.ReadKey(true);
            break;

        case 5:
            previousBalance = currentBalance;
            currentBalance = QuickDraw40(money[currentUserPosition]);
            Debug.Assert(currentBalance <= previousBalance);
            money[currentUserPosition] = currentBalance;
            bankInfo[currentUserPosition][2] = $"{currentBalance}";
            lines = new List<string>();
            foreach (List<string> row in bankInfo)
            {
                lines.Add(string.Join(",", row));
            }
            File.WriteAllLines("Bank.txt", lines);

            widthdrawalAmount =  previousBalance - currentBalance;
            transactionInfo = $"{userName} {RememberTransaction("widthdrew", widthdrawalAmount)}";
            transactions.Add(transactionInfo);
            File.WriteAllLines("Transactions.txt", transactions);


            Console.WriteLine($"Your new balance is ${currentBalance}.");
            Console.Write("Press any key to continue. ");
            Console.ReadKey(true);
            break;

        case 6:
            previousBalance = currentBalance;
            currentBalance = QuickDraw100(money[currentUserPosition]);
            Debug.Assert(currentBalance <= previousBalance);
            money[currentUserPosition] = currentBalance;
            bankInfo[currentUserPosition][2] = $"{currentBalance}";
            lines = new List<string>();
            foreach (List<string> row in bankInfo)
            {
                lines.Add(string.Join(",", row));
            }
            File.WriteAllLines("Bank.txt", lines);

            widthdrawalAmount =  previousBalance - currentBalance;
            transactionInfo = $"{userName} {RememberTransaction("widthdrew", widthdrawalAmount)}";
            transactions.Add(transactionInfo);
            File.WriteAllLines("Transactions.txt", transactions);


            Console.WriteLine($"Your new balance is ${currentBalance}.");
            Console.Write("Press any key to continue. ");
            Console.ReadKey(true);
            break;

        case 7:
            Console.Clear();
            Console.WriteLine($"Goodbye {userName}! Come again!");
            sessionGoing = false;
            lines = new List<string>();
            foreach (List<string> row in bankInfo)
            {
                lines.Add(string.Join(",", row));
            }
            File.WriteAllLines("Bank.txt", lines);
            break;

        default:
            Console.WriteLine("Invalid choice");
            break;
    }
}

static string RememberTransaction(string transactionType, double transactionValue)
{
    return $"{transactionType} ${transactionValue.ToString("##0.00")}.";
}

static double Deposit(double currentBalance)
{
    bool validDeposit = false;
    double newBalance = currentBalance;
    while (!validDeposit)
    {
        Console.Clear();
        Console.Write("How much money would you like to deposit? Use numbers only. ");
        string input = Console.ReadLine()!;
        input = input.Replace("+", "");
        if (input.Contains("-"))
        {
            Console.Write("That was an invalid deposit! Please try again!\nPress any key to continue. ");
            Console.ReadKey(true);
        }
        else if (double.TryParse(input, out double depositAmount))
        {
            if (depositAmount > 0) newBalance = currentBalance + depositAmount;
            if (newBalance != currentBalance) validDeposit = true;
        }
        else
        {
            Console.Write("That was an invalid deposit! Please try again!\nPress any key to continue. ");
            Console.ReadKey(true);
        }
    }
    return newBalance;
}

static double Widthdraw(double currentBalance)
{
    bool validWidthdrawal = false;
    double newBalance = currentBalance;
    while (!validWidthdrawal)
    {
        Console.Clear();
        Console.Write("How much money would you like to widthdraw? Use numbers only. ");
        string input = Console.ReadLine()!;
        input = input.Replace("+", "");
        if (input.Contains("-"))
        {
            Console.Write("That was an invalid widthdrawal! Please try again!\nPress any key to continue. ");
            Console.ReadKey(true);
        }
        else if (double.TryParse(input, out double widthdrawalAmount))
        {
            if (widthdrawalAmount > 0 && widthdrawalAmount <= currentBalance) newBalance = currentBalance - widthdrawalAmount;
            if (newBalance != currentBalance) validWidthdrawal = true;
        }
        else
        {
            Console.Write("That was an invalid widthdrawal! Please try again!\nPress any key to continue. ");
            Console.ReadKey(true);
        }
    }
    return newBalance;
}

static double QuickDraw40(double currentBalance)
{
    double newBalance = currentBalance;
    if (40 <= currentBalance)
    {
        Console.Clear();
        newBalance = currentBalance - 40;
        return newBalance;
    }
    else
    {
        Console.Clear();
        Console.WriteLine("You don't have enough money to widthdraw $40.");
        Console.ReadKey(true);
        return currentBalance;
    }

}

static double QuickDraw100(double currentBalance)
{

    double newBalance = currentBalance;
    if (100 <= currentBalance)
    {
        Console.Clear();
        newBalance = currentBalance - 100;
        return newBalance;
    }
    else
    {
        Console.Clear();
        Console.WriteLine("You don't have enough money to widthdraw $100.");
        Console.ReadKey(true);
        return currentBalance;
    }
}

static double CheckBalance(List<double> balances, int neededBalance)
{
    return balances[neededBalance];
}

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
