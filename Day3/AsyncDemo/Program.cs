using System.Diagnostics;
using System.Net.Http.Json;

async Task<string> SimulateDownloadAsync(string filename)
{
    Console.WriteLine("Downloading started....");
    await Task.Delay(2000);
    return $"{filename} Downloaded";
}

long startTime = Stopwatch.GetTimestamp();

Console.WriteLine(await SimulateDownloadAsync("file1.txt"));
Console.WriteLine(await SimulateDownloadAsync("file2.txt"));
Console.WriteLine(await SimulateDownloadAsync("file3.txt"));

TimeSpan timeTaken = Stopwatch.GetElapsedTime(startTime);
Console.WriteLine($"Time taken for sequential downloading: {timeTaken.Seconds}");


startTime = Stopwatch.GetTimestamp();

Task<string> task1 = SimulateDownloadAsync("file1.txt");
Task<string> task2 = SimulateDownloadAsync("file2.txt");
Task<string> task3 = SimulateDownloadAsync("file3.txt");

await Task.WhenAll(task1, task2, task3);
timeTaken = Stopwatch.GetElapsedTime(startTime);
Console.WriteLine(task1.Result);
Console.WriteLine(task2.Result);
Console.WriteLine(task3.Result);
Console.WriteLine($"Time taken for parallel downloading: {timeTaken.Seconds}");


HttpClient httpClient = new HttpClient();
var posts = new List<Post>();
try
{
    posts = await httpClient.GetFromJsonAsync<List<Post>>("https://jsonplaceholder.typicode.com/posts");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
if (posts != null)
{
    foreach (Post post in posts)
    {
        post.PrintTitles();
    }
}

class Post
{
    public int userId { get; set; }
    public int id { get; set; }
    public string title { get; set; } = string.Empty;
    public string body { get; set; } = string.Empty;

    public void PrintTitles()
    {
        Console.WriteLine($"Title: {this.title}");
    }
}