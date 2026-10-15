int[] marks = new int[5];

for(int i = 0; i < marks.Length; i++)
{
    Console.WriteLine($"Enter the marks of subject {i + 1}");
    var val = Console.ReadLine();
    bool success = int.TryParse(val, out marks[i]);
    if (!success||marks[i]>100||marks[i]<0)
    {
        Console.Write("Invalid!! ");
        i--;
    }
}

int total = 0;
for(int i = 0; i < marks.Length; i++)
{
    total += marks[i];
}
double percentage = total / 500.0 * 100;

Console.WriteLine($"Total: {total}");
Console.WriteLine($"Percentage: {percentage}");
if (percentage >= 90 && percentage <= 100)
{
    Console.WriteLine("You got A Grade");
}
else if (percentage >= 80 && percentage < 90)
{
    Console.WriteLine("You got B grade");
}
else if (percentage >= 70 && percentage < 80)
{
    Console.WriteLine("You got C grade");
}
else if (percentage >= 60 && percentage < 70)
{
    Console.WriteLine("You got D grade");
}
else if (percentage >= 0 && percentage <= 60)
{
    Console.WriteLine("You got F grade");
}
else
{
    Console.WriteLine("Your marks are out of range.");
}




