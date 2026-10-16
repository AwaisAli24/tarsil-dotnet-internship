SavingAccount obj = new SavingAccount("Awais", 3);

bool flag = true;
while (flag)
{
    Console.WriteLine("1-Deposit\n2-Withdraw\n3-Check Balance\n4-Add Interest\n5-Exit");
    var choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            Console.WriteLine("Enter amount to deposit.");
            bool success = decimal.TryParse(Console.ReadLine(), out decimal amount);
            obj.Deposit(amount);
            break;
        case "2":
            Console.WriteLine("Enter amount to withdraw:");
            success = decimal.TryParse(Console.ReadLine(), out amount);
            obj.Withdraw(amount);
            break;
        case "3":
            Console.WriteLine($"Your balance is {obj.GetBalance()}");
            break;
        case "4":
            obj.AddMonthlyInterest();
            Console.WriteLine("Interest added successfully.");
            break;
        case "5":
            flag = false;
            Console.WriteLine("The program is exited Successfully");
            break;
        default:
            Console.WriteLine("Invalid input! Please try again.");
            break;
    }
}

public class BankAccount
{
    private decimal Balance { get; set; }
    public int AccountNumber;
    public String OwnerName;

    public BankAccount(string OwnerName)
    {
        Random rand = new Random();

        this.OwnerName = OwnerName;
        this.AccountNumber = rand.Next();
    }
    public void Deposit(decimal Amount)
    {
        if (Amount <= 0)
        {
            Console.WriteLine("The amount entered is invalid.");
            return;
        }
        this.Balance += Amount;
        Console.WriteLine($"Rs.{Amount} has been successfully deposited.");
    }
    public void Withdraw(decimal Amount)
    {
        if (Amount > this.Balance || Amount < 0)
        {
            Console.WriteLine("Error! Your amount is either invalid or greater than the balance.");
            return;
        }
        this.Balance -= Amount;
        Console.WriteLine($"Rs.{Amount} has been successfully withdrawn.");
    }
    public void ModifyBalance(decimal Amount)
    {
        this.Balance += Amount;
    }
    public decimal GetBalance()
    {
        return Balance;
    }
}
public class SavingAccount : BankAccount
{
    private decimal InterestRate;
    public SavingAccount(string OwnerName, decimal InterestRate) : base(OwnerName)
    {
        this.InterestRate = InterestRate;
    }
    public void AddMonthlyInterest()
    {
        decimal CurrentBalance = this.GetBalance();
        decimal InterestAmount = CurrentBalance * (this.InterestRate/100);

        ModifyBalance(InterestAmount);
    }
}