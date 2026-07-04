namespace Fractal_Visualizer
{
    public class Complex(double real, double image)
    {
        public double Real => real;
        public double Image => image;
        public double SquaredAbs => real*real+image*image;
        public double Abs => Math.Pow(real * real + image * image, 0.5);
        public double Theta => Math.Atan2(image, real);

        public static Complex AbsThetaComplex(double abs, double theta)
        {
            double real = Math.Cos(theta) * abs;
            double image = Math.Sin(theta) * abs;
            return new Complex(real, image);
        }

        public static Complex Sum(Complex a, Complex b)
        {
            return new Complex(a.Real + b.Real, a.Image + b.Image);
        }

        public static Complex Product(Complex a, Complex b)
        {
            double abs = a.Abs * b.Abs;
            double theta = a.Theta + b.Theta;
            return AbsThetaComplex(abs, theta);
        }

        public static Complex Division(Complex a, Complex b)
        {
            double abs = a.Abs / b.Abs;
            double theta = a.Theta - b.Theta;
            return AbsThetaComplex(abs, theta);
        }

        public static Complex Square(Complex a)
        {
            double real = a.Real * a.Real - a.Image * a.Image;
            double image = 2 * a.Real * a.Image;
            return new Complex(real, image);
        }

        public static Complex Pow(Complex a, double power)
        {
            double abs = Math.Pow(a.Abs, power);
            double theta = a.Theta * power;
            return AbsThetaComplex(abs, theta);
        }

        public static Complex Sin(Complex a)
        {
            double real = Math.Sin(a.Real) * Math.Cosh(a.Image);
            double image = Math.Cos(a.Real) * Math.Sinh(a.Image);
            return new Complex(real, image);
        }

        public static Complex Cos(Complex a)
        {
            double real = Math.Cos(a.Real) * Math.Cosh(a.Image);
            double image = Math.Sin(a.Real) * Math.Sinh(a.Image);
            return new Complex(real, image);
        }
    }
}