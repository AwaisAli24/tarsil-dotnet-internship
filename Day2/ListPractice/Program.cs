List<int> numbers = new List<int> { };

while (true)
{
    Console.WriteLine("Enter a number or type done to exit.");
    var value = Console.ReadLine();
    if (value == "done")
    {
        if (numbers.Count != 0)
        {
            Console.WriteLine("No numbers entered.");
        }
        break;
    }
    var success = int.TryParse(value, out int number);
    if (!success)
    {
        Console.WriteLine("The number you entered is invalid.");
        continue;
    }
    numbers.Add(number);
}
if (numbers.Count != 0)
{

    int sum = 0;
    for (int i = 0; i < numbers.Count; i++)
    {
        sum += numbers[i];
    }
    double average = (double)sum / numbers.Count;

    int min = int.MaxValue;
    int max = int.MinValue;

    //Finding minimum value
    for (int i = 0; i < numbers.Count; i++)
    {
        if (min > numbers[i])
        {
            min = numbers[i];
        }
    }
    //Finding maximum value
    for (int i = 0; i < numbers.Count; i++)
    {
        if (max < numbers[i])
        {
            max = numbers[i];
        }
    }

    Console.WriteLine($"Count:{numbers.Count}");
    Console.WriteLine($"Sum: {sum}");
    Console.WriteLine($"Average: {average}");
    Console.WriteLine($"Max Value: {max}");
    Console.WriteLine($"Min Value: {min}");

    //Printing all even Numbers
    Console.WriteLine("Even Numbers");
    for (int i = 0; i < numbers.Count; i++)
    {
        if (numbers[i] % 2 == 0)
        {
            Console.Write($"{numbers[i]} ");
        }
    }
    //Sorted List
    Console.WriteLine("\nSorted List");
    numbers.Sort();
    for (int i = 0; i < numbers.Count; i++)
    {
        Console.Write($"{numbers[i]} ");
    }
}