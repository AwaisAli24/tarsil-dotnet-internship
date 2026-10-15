Console.WriteLine("Enter Marks of Subject 1:");
int.TryParse(Console.ReadLine(), out int marks1);
Console.WriteLine("Enter Marks of Subject 2:");
int.TryParse(Console.ReadLine(), out int marks2);
Console.WriteLine("Enter Marks of Subject 3:");
int.TryParse(Console.ReadLine(), out int marks3);
Console.WriteLine("Enter Marks of Subject 4:");
int.TryParse(Console.ReadLine(), out int marks4);
Console.WriteLine("Enter Marks of Subject 5:");
int.TryParse(Console.ReadLine(), out int marks5);

int total = marks1 + marks2 + marks3 + marks4 + marks5;
double percentage = total / 500.0 * 100;

Console.WriteLine($"Total: {total}");
Console.WriteLine($"Percentage: {percentage}");
if (percentage > 90 && percentage <= 100)
{
    Console.WriteLine("You got A Grade");
}
else if (percentage > 80 && percentage <= 90)
{
    Console.WriteLine("You got B grade");
}
else if (percentage > 70 && percentage <= 80)
{
    Console.WriteLine("You got C grade");
}
else if (percentage > 60 && percentage <= 70)
{
    Console.WriteLine("You got D grade");
}
else if (percentage > 0 && percentage <= 60)
{
    Console.WriteLine("You got F grade");
}
else
{
    Console.WriteLine("Your marks are out of range.");
}




