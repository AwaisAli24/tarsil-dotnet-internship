bool IsPrime(int n)
{
    int count = 0;
    for (int i = 1; i <= n; i++)
    {
        if (n % i == 0)
        {
            count++;
        }
    }
    if (count == 2)
    {
        return true;
    }
    return false;
}

long Factorial(int n)
{
    if (n < 0)
    {
        Console.Write("Factorial of negative numbers is not possible");
        return -1;
    }
    if (n == 1 || n == 0)
    {
        return 1;
    }
    return n * Factorial(n - 1);
}

string ReverseString(string text)
{
    string newText = "";
    for (int i = text.Length - 1; i >= 0; i--)
    {
        newText += text[i];
    }
    return newText;
}

int CountVowels(string text)
{
    text = text.ToLower();
    int count = 0;
    for (int i = 0; i < text.Length; i++)
    {
        if (text[i] == 'a' || text[i] == 'e' || text[i] == 'i' || text[i] == 'o' || text[i] == 'u')
        {
            count++;
        }
    }
    return count;
}

bool IsPalindrome(string text)
{
    text = text.ToLower();
    return text == ReverseString(text);
}

Console.WriteLine(IsPrime(19));
Console.WriteLine(Factorial(-1));
Console.WriteLine(ReverseString("hello"));
Console.WriteLine(CountVowels("abcdefghijklmnopqrstuvwxyz"));
Console.WriteLine(IsPalindrome("civic"));