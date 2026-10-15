Console.WriteLine("Enter your Name:");
var name = Console.ReadLine();
Console.WriteLine("Enter your age:");
// var input = Console.ReadLine();
// int age = int.Parse(input);
int age;
do
{
    var input = Console.ReadLine();
    bool success = int.TryParse(input, out age);
    if (success)
    {
        break;
    }
    Console.WriteLine("Please enter your valid age:");
} while (true);
Console.WriteLine("Enter your city:");
var city = Console.ReadLine();

string message = $"Hello {name}! You are {age} years old and live in {city}.\nYou were born around {DateTime.Now.Year - age}. In 5 years you will be {age + 5}.";

Console.WriteLine(message);