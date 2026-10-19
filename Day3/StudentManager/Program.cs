using System.Text.Json;

List<Student> students = new List<Student>();
string file = "students.json";

try
{
    string json = File.ReadAllText(file);
    students = JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
}
catch (Exception e)
{
    Console.WriteLine("No file exist");
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
            if (name == "" || name == null)
            {
                Console.WriteLine("Name cannot be empty! Please try again.");
                break;
            }
            Console.WriteLine("Enter City:");
            var city = Console.ReadLine();
            if (city == "" || city == null)
            {
                Console.WriteLine("City cannot be empty! Please try again.");
                break;
            }
            Console.WriteLine("Enter marks:");
            bool success = int.TryParse(Console.ReadLine(), out int marks);
            if (!success)
            {
                Console.WriteLine("Invalid Marks");
                break;
            }
            if (marks > 100 || marks < 0)
            {
                Console.WriteLine("Marks Should be between 0 and 100");
                break;
            }
            students.Add(new Student(students[^1].ID, name, city, marks));
            string json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(file, json);
            break;
        case "2":
            Console.WriteLine("Id\t\tName\t\tCity\t\tMarks");
            foreach (Student student in students)
            {
                Console.WriteLine($"{student.ID}\t\t{student.Name}\t\t{student.City}\t\t{student.Marks}");
            }
            break;
        case "3":
            Console.WriteLine("Enter name to search:");
            var str = Console.ReadLine();
            if (!str.IsWhiteSpace() && str != null)
            {
                var sstudent = students.Where(s => s.Name.ToLower().Contains(str.ToLower())).ToList();
                foreach (var student in sstudent)
                {
                    Console.WriteLine($"{student.ID} {student.Name}");
                }
            }
            break;
        case "4":
            Console.Write("Enter student id:");
            success = int.TryParse(Console.ReadLine(), out int id);
            var studentToEdit = students.FirstOrDefault(s => s.ID == id);
            if (studentToEdit == null)
            {
                Console.WriteLine("No matching id found");
                return;
            }
            Console.WriteLine("Enter the new marks:");
            success = int.TryParse(Console.ReadLine(), out int newMarks);
            if (!success)
            {
                Console.WriteLine("Invalid Marks");
            }
            if (newMarks > 100 || newMarks < 0)
            {
                Console.WriteLine("Marks Should be between 0 and 100");
                break;
            }
            studentToEdit.Marks = newMarks;
            Console.WriteLine($"Your marks for {studentToEdit.Name} has been updated.");
            json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(file, json);
            break;
        case "5":
            Console.WriteLine("Enter the id:");
            success = int.TryParse(Console.ReadLine(), out id);
            var studentToDelete = students.FirstOrDefault(s => s.ID == id);
            if (studentToDelete == null)
            {
                Console.WriteLine("No matching id found");
                return;
            }
            students.Remove(studentToDelete);
            json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(file, json);
            break;
        case "6":
            if (students.Count == 0)
            {
                Console.WriteLine("There is no student record to show.");
                break;
            }
            Console.WriteLine("Top 3 Students by marks.");
            var top3StudentsByMarks = students.OrderByDescending(s => s.Marks).Take(3);
            foreach (var student in top3StudentsByMarks)
            {
                Console.WriteLine($"{student.Name}\t{student.Marks}");
            }
            Console.WriteLine($"Class Average {students.Average(s => s.Marks)}");
            Console.WriteLine($"Class Highest {students.Max(s => s.Marks)}");
            Console.WriteLine($"Class Lowest {students.Min(s => s.Marks)}");
            var passedStudents = students.Where(s => s.Marks >= 50);
            var failedStudents = students.Where(s => s.Marks <= 50);

            Console.WriteLine("Passed Students");
            foreach (var student in passedStudents)
            {
                Console.WriteLine(student.Name);
            }
            Console.WriteLine("Failed Students");
            foreach (var student in failedStudents)
            {
                Console.WriteLine(student.Name);
            }

            var groups = students.GroupBy(s => s.City).ToList();
            foreach (var g in groups)
            {
                Console.WriteLine($"{g.Key} {g.Count()}");
            }
            Console.WriteLine("Enter first letter to get names:");
            var letter = Console.ReadLine();
            if (!letter.IsWhiteSpace() && letter != null)
            {
                var studentsByFirstLetter = students.Where(s => s.Name.StartsWith(letter)).ToList();
                Console.WriteLine($"Names starting with: {letter}");
                foreach (var student in studentsByFirstLetter)
                {
                    Console.WriteLine(student.Name);
                }
            }
            break;
        case "7":
            flag = false;
            break;
        default:
            Console.WriteLine("Invalid Choice");
            break;
    }
}
public class Student
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public int Marks { get; set; }
    public Student(int id, string name, string city, int marks)
    {
        this.ID = id;
        this.Name = name;
        this.City = city;
        this.Marks = marks;
    }
}