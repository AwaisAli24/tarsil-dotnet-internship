do
{
    Console.WriteLine("Enter First Number:");
    var input = Console.ReadLine();
    if(input == "exit")
    {
        break;
    }
    bool success = double.TryParse(input, out double num1);
    if (!success)
    {
        Console.WriteLine("Invalid number! Please enter a valid number");
        continue;
    }
    Console.WriteLine("Enter Second Number:");
    success = double.TryParse(Console.ReadLine(), out double num2);
    if (!success)
    {
        Console.WriteLine("Your input number is not valid.");
        continue;
    }
    Console.WriteLine("Choose an Operator: +,_,*,/,%");
    var operator_ = Console.ReadLine();
    if (num2 == 0 && operator_ =="/")
    {
        Console.WriteLine("Cannot divide a number by zero.");
        continue;
    }
    double result = 0;
    switch (operator_)
    {
        case "+":
            result = num1 + num2;
            Console.WriteLine($"{num1} {operator_} {num2} = {result}");
            break;
        case "-":
            result = num1 - num2;
            Console.WriteLine($"{num1} {operator_} {num2} = {result}");
            break;
        case "*":
            result = num1 * num2;
            Console.WriteLine($"{num1} {operator_} {num2} = {result}");
            break;
        case "/":
            result = num1 / num2;
            Console.WriteLine($"{num1} {operator_} {num2} = {result}");
            break;
        case "%":
            result = num1 % num2;
            Console.WriteLine($"{num1} {operator_} {num2} = {result}");
            break;
        default:
            Console.WriteLine("Invalid Operator!!");
            break;
    }
    Console.WriteLine("Type exit if you want to quit or else:");
} while (true);