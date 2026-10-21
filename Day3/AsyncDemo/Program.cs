using System.Diagnostics;
using System.Net.Http.Json;

async Task<string> SimulateDownloadAsync(string filename)
{
    Console.WriteLine($"Downloading started....{filename}");
    await Task.Delay(2000);
    return $"{filename} Downloaded";
}

long startTime = Stopwatch.GetTimestamp();

Console.WriteLine(await SimulateDownloadAsync("file1.txt"));
Console.WriteLine(await SimulateDownloadAsync("file2.txt"));
Console.WriteLine(await SimulateDownloadAsync("file3.txt"));

TimeSpan timeTaken = Stopwatch.GetElapsedTime(startTime);
Console.WriteLine($"Time taken for sequential downloading: {timeTaken.TotalSeconds} s");


startTime = Stopwatch.GetTimestamp();

Task<string> task1 = SimulateDownloadAsync("file1.txt");
Task<string> task2 = SimulateDownloadAsync("file2.txt");
Task<string> task3 = SimulateDownloadAsync("file3.txt");

await Task.WhenAll(task1, task2, task3);
timeTaken = Stopwatch.GetElapsedTime(startTime);
Console.WriteLine(task1.Result);
Console.WriteLine(task2.Result);
Console.WriteLine(task3.Result);
Console.WriteLine($"Time taken for parallel downloading: {timeTaken.TotalSeconds} s");


HttpClient httpClient = new HttpClient();
var posts = new List<Post>();
try
{
    posts = await httpClient.GetFromJsonAsync<List<Post>>("https://jsonplaceholder.typicode.com/posts");
}
catch (HttpRequestException e)
{
    Console.WriteLine(e.Message);
}
if (posts != null)
{
    for (int i = 0; i < 5; i++)
    {
        posts[i].PrintTitle();
    }
}

class Post
{
    public int UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    public void PrintTitle()
    {
        Console.WriteLine($"Title: {this.Title}");
    }
}