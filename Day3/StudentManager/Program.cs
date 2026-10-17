using System.Text.Json;

List<Student> students = new List<Student>();
string file = "students1.json";

try
{
    string json = File.ReadAllText(file);
    students = JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

bool flag = true;
while (flag)
{
    Console.WriteLine("1.Add student\n2.View Students\n3.Search Student\n4.Update marks\n5.Delete Student\n6.Reports\n7.Exit");
    var choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            Console.WriteLine("Enter name:");
            var name = Console.ReadLine();
            if (name == null)
            {
                Console.WriteLine("Name cannot be empty! Please try again.");
                break;
            }
            Console.WriteLine("Enter City:");
            var city = Console.ReadLine();
            Console.WriteLine("Enter marks:");
            bool success = int.TryParse(Console.ReadLine(), out int marks);
            if (marks > 100 || marks < 0)
            {
                Console.WriteLine("Marks Should be between 0 and 100");
                break;
            }
            students.Add(new Student(students.Count + 1, name, city, marks));
            break;
        case "2":
            Console.WriteLine("Id\t\tName\t\tCity\t\tMarks");
            foreach (Student student in students)
            {
                Console.WriteLine($"{student.id}\t\t{student.name}\t\t{student.city}\t\t{student.marks}");
            }
            break;
        case "3":
            Console.WriteLine("Enter name to search:");
            var str = Console.ReadLine();
            var sstudent = students.Where(s => s.name.ToLower().Contains(str.ToLower())).ToList();
            foreach (var student in sstudent)
            {
                Console.WriteLine($"{student.id} {student.name}");
            }
            break;
        case "4":
            Console.Write("Enter student id:");
            success = int.TryParse(Console.ReadLine(), out int id);
            var studentToEdit = students.First(s => s.id == id);
            Console.WriteLine("Enter the new marks:");
            success = int.TryParse(Console.ReadLine(), out int newMarks);
            studentToEdit.marks = newMarks;
            Console.WriteLine($"Your marks for {studentToEdit.name} has been updated.");
            break;
        case "5":
            Console.WriteLine("Enter the id:");
            success = int.TryParse(Console.ReadLine(), out id);
            var studentToDelete = students.First(s => s.id == id);
            students.Remove(studentToDelete);
            break;
        case "6":
            Console.WriteLine("Top 3 Students by marks.");
            var top3StudentsByMarks = students.OrderByDescending(s => s.marks).Take(3);
            foreach (var student in top3StudentsByMarks)
            {
                Console.WriteLine($"{student.name}\t{student.marks}");
            }
            Console.WriteLine($"Class Average {students.Average(s => s.marks)}");
            Console.WriteLine($"Class Highest {students.Max(s => s.marks)}");
            Console.WriteLine($"Class Lowest {students.Min(s => s.marks)}");
            var studentsPassed = students.Select(s => s.marks >= 50).ToList();
            Console.WriteLine("Students Passed:");
            foreach (var (student, status) in students.Zip(studentsPassed))
            {
                Console.WriteLine($"{student.name}\t{status}");
            }
            var groups = students.GroupBy(s => s.city).ToList();
            foreach (var g in groups)
            {
                Console.WriteLine($"{g.Key} {g.Count()}");
            }
            Console.WriteLine("Enter first letter to get names:");
            var letter = Console.ReadLine();
            var studentsByFirstLetter = students.Where(s => s.name.StartsWith(letter)).ToList();
            Console.WriteLine($"Names starting with: {letter}");
            foreach (var student in studentsByFirstLetter)
            {
                Console.WriteLine(student.name);
            }
            break;
        case "7":
            flag = false;
            string json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(file, json);
            break;
    }
}
public class Student
{
    public int id { get; set; }
    public string name { get; set; }
    public string city { get; set; }
    public int marks { get; set; }
    public Student(int id, string name, string city, int marks)
    {
        this.id = id;
        this.name = name;
        this.city = city;
        this.marks = marks;
    }
}