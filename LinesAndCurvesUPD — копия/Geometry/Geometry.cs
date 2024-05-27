namespace Geometry
{
    // POINTS
    public interface IPoint
    {
        public double getX();
        public double getY();
        public void setX(double x);
        public void setY(double y);
    }

    public class Point : IPoint
    {
        public Point()
        {
            _x = 0;
            _y = 0;
        }
        public Point(double x, double y)
        {
            _x = x;
            _y = y;
        }
        public double getX()
        {
            return _x;
        }
        public double getY()
        {
            return _y;
        }
        public void setX(double x)
        {
            this._x = x;
        }
        public void setY(double y)
        {
            this._y = y;
        }
        private double _x, _y;
    }

    // CURVES
    public interface ICurve
    {
        public IPoint GetPoint(double t);
    }

    // DECORATORS
    public class Fragment : ICurve
    {
        public Fragment(ICurve curve, double start, double end)
        {
            _original = curve;
            _start = start;
            _end = end;
        }

        public void setCurve(ICurve curve)
        {
            _original = curve;
        }

        public void setStart(double start)
        {
            _start = start;
        }

        public void setEnd(double end)
        {
            _end = end;
        }

        public IPoint GetPoint(double t)
        {
            return _original.GetPoint(_start + t * (_end - _start));
        }

        private double _start, _end;
        private ICurve _original;
    }

    public class MoveTo : ICurve
    {
        public ICurve GetCurve()
        {
            return _original;
        }

        public MoveTo(ICurve curve, IPoint destination)
        {
            _original = curve;
            _destination = destination;
        }

        public void setCurve(ICurve curve)
        {
            _original = curve;
        }

        public void setDestination(IPoint destination)
        {
            _destination = destination;
        }

        public IPoint GetPoint(double t)
        {
            double xdiff, ydiff;
            xdiff = _original.GetPoint(0).getX() - _destination.getX();
            ydiff = _original.GetPoint(0).getY() - _destination.getY();
            IPoint np = new Geometry.Point();
            np.setX(_original.GetPoint(t).getX() - xdiff);
            np.setY(_original.GetPoint(t).getY() - ydiff);
            return np;
        }

        private IPoint _destination;
        private ICurve _original;
    }

    // ABSTRACT CURVE
    public abstract class ACurve : ICurve
    {
        public ACurve(IPoint a, IPoint b)
        {
            this._a = a;
            this._b = b;
        }
        public abstract IPoint GetPoint(double t);
        protected IPoint _a, _b;
    }


    // CONCRETE CURVES
    public class Line : ACurve
    {
        public Line(IPoint a, IPoint b) : base(a, b) { }

        public override IPoint GetPoint(double t)
        {
            IPoint np = new Point();
            np.setX((1 - t) * _a.getX() + t * _b.getX());
            np.setY((1 - t) * _a.getY() + t * _b.getY());
            return np;
        }
    }

    public class Bezier : ACurve
    {
        public Bezier(IPoint a, IPoint b, IPoint c, IPoint d) : base(a, b)
        {
            _c = c;
            _d = d;
        }

        public override IPoint GetPoint(double t)
        {
            IPoint np = new Point();
            np.setX(Math.Pow((1 - t), 3) * _a.getX() +
                     3 * t * Math.Pow((1 - t), 2) * _c.getX() +
                     3 * Math.Pow(t, 2) * (1 - t) * _d.getX() +
                     Math.Pow(t, 3) * _b.getX());
            np.setY(Math.Pow((1 - t), 3) * _a.getY() +
                     3 * t * Math.Pow((1 - t), 2) * _c.getY() +
                     3 * Math.Pow(t, 2) * (1 - t) * _d.getY() +
                     Math.Pow(t, 3) * _b.getY());
            return np;
        }

        private IPoint _c, _d;
    }

}
