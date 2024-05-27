using Geometry;
using System.Drawing;

namespace Visual
{
    public interface IDrawer
    {
        Color Color { get; }
        void DrawPoint(IPoint p);
        void DrawLine(IPoint a, IPoint b, bool EnableEndCap, Color color);

    }

    public class HorizontalReflection : IDrawer
    {
        public Color Color { get { return _innerDrawer.Color; } }

        private IDrawer _innerDrawer;
        private int _width;

        public HorizontalReflection(IDrawer D, int Width)
        {
            _innerDrawer = D;
            _width = Width;
        }

        public void setDrawer(IDrawer D)
        {
            _innerDrawer = D;
        }

        public void DrawPoint(IPoint p)
        {
            p.setX(_width - p.getX());
            _innerDrawer.DrawPoint(p);
        }

        public void DrawLine(IPoint A, IPoint B, bool EnableEndCap, Color color)
        {
            A.setX(_width - A.getX());
            B.setX(_width - B.getX());
            _innerDrawer.DrawLine(A, B, EnableEndCap, _innerDrawer.Color);
        }
    }

    public class VerticalReflection : IDrawer
    {
        public Color Color { get { return _innerDrawer.Color; } }

        private IDrawer _innerDrawer;
        private int _height;

        public VerticalReflection(IDrawer D, int Height)
        {
            _innerDrawer = D;
            _height = Height;
        }

        public void DrawPoint(IPoint p)
        {
            p.setY(_height - p.getY());
            _innerDrawer.DrawPoint(p);
        }

        public void DrawLine(IPoint A, IPoint B, bool EnableEndCap, Color color)
        {
            A.setY(_height - A.getY());
            B.setY(_height - B.getY());
            _innerDrawer.DrawLine(A, B, EnableEndCap, _innerDrawer.Color);
        }
    }

    public class GreenDrawer : IDrawer
    {
        private Pen _pen;
        private Color _color;
        private Graphics _g;

        public Color Color { get { return _color; } }
        public GreenDrawer(Graphics g, Color color)
        {
            this._color = color;
            this._pen = new Pen(color, 3);
            this._g = g;
        }

        public void DrawPoint(IPoint p)
        {
            _g.DrawEllipse(_pen, (int)p.getX(), (int)p.getY(), (float)4, (float)4);
        }

        public void DrawLine(IPoint A, IPoint B, bool EnableEndCap, Color color)
        {
            Pen pen = new Pen(color, 3);
            if (EnableEndCap) pen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(3, 3);
            _g.DrawLine(pen, (int)A.getX(), (int)A.getY(), (int)B.getX(), (int)B.getY());
            if (EnableEndCap) pen.EndCap = new System.Drawing.Drawing2D.LineCap();
        }

    }

    public class BlackDrawer : IDrawer
    {
        public Color Color { get { return _color; } }

        private Pen _pen;
        private Color _color;
        private Graphics _g;

        public BlackDrawer(Graphics g, Color color)
        {
            this._color = color;
            this._pen = new Pen(color, 3);
            this._g = g;
            _pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

        }

        public void DrawPoint(IPoint p)
        {
            _g.DrawRectangle(_pen, (int)(p.getX() - 2.5), (int)(p.getY() - 2.5), 5, 5);
        }

        public void DrawLine(IPoint A, IPoint B, bool EnableEndCap, Color color)
        {
            _pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            _pen.Color = color; // Устанавливаем цвет перед рисованием линии
            _g.DrawLine(_pen, (int)A.getX(), (int)A.getY(), (int)B.getX(), (int)B.getY());
            if (EnableEndCap)
            {
                _pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                DrawPoint(B);
            }
        }




    }

    // ABSTRACTION
    public interface IDrawable
    {
        public void Draw(IDrawer d);
    }

    public class AVisualCurve : IDrawable, ICurve
    {
        private ICurve _c;
        private Color _color;

        public AVisualCurve(ICurve c, Color color)
        {
            _c = c;
            _color = color;
        }

        public void Draw(IDrawer d)
        {
            d.DrawPoint(_c.GetPoint(0));
            int n = 10;
            for (int i = 0; i < n; ++i)
            {
                d.DrawLine(_c.GetPoint(i / (double)n), _c.GetPoint((i + 1) / (double)n), i == n - 1, _color);
            }
        }

        public IPoint GetPoint(double t)
        {
            return _c.GetPoint(t);
        }
    }
}
