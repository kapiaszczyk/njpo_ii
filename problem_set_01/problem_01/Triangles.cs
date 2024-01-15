public abstract class Triangle
{
    protected readonly double a;
    protected readonly double b;
    protected readonly double c;
    protected readonly double p;

    protected Triangle(double a, double b, double c)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.p = (a + b + c) / 2;
    }

    public double Area()
    {
        return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
    }

    public double Perimeter()
    {
        return a + b + c;
    }

    public override string ToString()
    {
        return string.Format("a: {0}, b: {1}, c: {2}, area: {3}, perimeter: {4}", a, b, c, Area(), Perimeter());
    }
}

// trójkąt równoboczny
public class EquilateralTriangle : Triangle
{
    public EquilateralTriangle(double a) : base(a, a, a) { }
}

// trójkąt równoramienny
public class IsoscelesTriangle : Triangle
{
    public IsoscelesTriangle(double a, double b) : base(a, b, b) { }
}

// trójkąt prostokątny
public class RightTriangle : Triangle
{
    public RightTriangle(double a, double b) : base(a, b, Math.Sqrt(a * a + b * b)) { }
}
