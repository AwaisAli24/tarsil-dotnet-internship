Circle circ = new Circle(5);
Rectangle rect = new Rectangle(4, 3);
Triangle tri = new Triangle(4, 5, 6);

List<Shape> list = new List<Shape>();
list.Add(circ);
list.Add(rect);
list.Add(tri);

foreach (Shape shape in list)
{
    Console.WriteLine(shape.Describe());
    Console.WriteLine($"Area :{shape.CalculateArea()}");
    Console.WriteLine($"Perimeter :{shape.CalculatePerimeter()}");
}



public abstract class Shape : IDescribable
{
    public abstract double CalculateArea();
    public abstract double CalculatePerimeter();
    public abstract string Describe();
}

public class Circle : Shape
{
    public double radius;

    public Circle(double radius)
    {
        this.radius = radius;
    }
    public override double CalculateArea()
    {
        return Math.PI * radius * radius;
    }
    public override double CalculatePerimeter()
    {
        return 2 * Math.PI * radius;
    }
    public override string Describe()
    {
        return "This is a Circle";
    }
}
public class Rectangle : Shape
{
    public double length;
    public double width;

    public Rectangle(double length, double width)
    {
        this.length = length;
        this.width = width;
    }
    public override double CalculateArea()
    {
        return length * width;
    }
    public override double CalculatePerimeter()
    {
        return 2 * (length + width);
    }
    public override string Describe()
    {
        return "This is a Rectangle";
    }
}
public class Triangle : Shape
{
    public double a;
    public double b;
    public double c;

    public Triangle(double a, double b, double c)
    {
        this.a = a;
        this.b = b;
        this.c = c;
    }
    public override double CalculateArea()
    {
        double s = CalculatePerimeter() / 2;
        return Math.Sqrt(s * (s - this.a) * (s - this.b) * (s - this.c));
    }
    public override double CalculatePerimeter()
    {
        return this.a + this.b + this.c;
    }
    public override string Describe()
    {
        return "This is a triangle";
    }
}
public interface IDescribable
{
    string Describe();
}