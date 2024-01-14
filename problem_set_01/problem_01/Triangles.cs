namespace Triangles
{
    public abstract class Triangle(double a, double b, double c)
    {

        private readonly double a = a;
        private readonly double b = b;
        private readonly double c = c;
        private readonly double p = (a + b + c) / 2;

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
            return "a = " + a + ", b = " + b + ", c = " + c + ", p = " + p;
        }
    }

    // trójkąt równoboczny
    public class EquilateralTriangle : Triangle
    {
        private readonly double a;
        private readonly double b;
        private readonly double c;
        private readonly double p;

        public EquilateralTriangle(double a) : base(a, a, a)
        {
            this.a = a;
            this.b = a;
            this.c = a;
            this.p = (a + a + a) / 2;
        }

    }

    // trójkąt równoramienny
    public class IsoscelesTriangle : Triangle
    {
        private readonly double a;
        private readonly double b;
        private readonly double c;
        private readonly double p;

        public IsoscelesTriangle(double a, double b) : base(a, b, b)
        {
            this.a = a;
            this.b = b;
            this.c = b;
            this.p = (a + b + c) / 2;
        }

    }

    // trójkąt prostokątny
    public class RightTriangle : Triangle
    {
        private readonly double a;
        private readonly double b;
        private readonly double c;
        private readonly double p;

        public RightTriangle(double a, double b) : base(a, b, Math.Sqrt(a * a + b * b))
        {
            this.a = a;
            this.b = b;
            this.c = Math.Sqrt(a * a + b * b);
            this.p = (a + b + c) / 2;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }

}
