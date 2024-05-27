using Geometry;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace Visual
{
    public interface IDrawer
    {
        Color Color { get; }
        void DrawPoint(IPoint p);
        void DrawLine(IPoint a, IPoint b, bool enableEndCap);
    }

    public class SVGExportingDecorator : IDrawer
    {
        private readonly IDrawer _innerDrawer;
        private readonly StreamWriter _writer;
        private readonly bool _isDashed;
        private readonly bool _enableEndCap;

        // Конструктор декоратора экспорта в SVG
        public SVGExportingDecorator(IDrawer innerDrawer, StreamWriter writer, bool isDashed, bool enableEndCap)
        {
            _innerDrawer = innerDrawer;
            _writer = writer;
            _isDashed = isDashed;
            _enableEndCap = enableEndCap;
        }

        // Возвращает цвет из внутреннего рисовальщика
        public Color Color => _innerDrawer.Color;

        public void DrawPoint(IPoint p)
        {
            ExportPointSVG(p, Color);
            //_innerDrawer.DrawPoint(p);
        }

        public void DrawLine(IPoint a, IPoint b, bool enableEndCap)
        {
            ExportLineSVG(a, b, enableEndCap, Color);
            //_innerDrawer.DrawLine(a, b, enableEndCap);
        }


        private void ExportPointSVG(IPoint p, Color color)
        {
            string shape = (color == Color.Green) ? "circle" : "rect";
            string fill = (color == Color.Green) ? "green" : "black";
            _writer.WriteLine($"<{shape} cx=\"{(int)p.getX()}\" cy=\"{(int)p.getY()}\" r=\"3\" width=\"5\" height=\"5\" fill=\"{fill}\" />");
        }

        private void ExportLineSVG(IPoint A, IPoint B, bool enableEndCap, Color color)
        {
            string strokeColor = color == Color.Green ? "green" : "black";
            _writer.Write($"<line x1=\"{(int)A.getX()}\" y1=\"{(int)A.getY()}\" x2=\"{(int)B.getX()}\" y2=\"{(int)B.getY()}\" style=\"stroke:{strokeColor};stroke-width:3\"");

            if (_isDashed)
            {
                _writer.Write(" stroke-dasharray=\"5,2\"");
            }

            _writer.WriteLine(" />");

            // Сохранение точек в начале и в конце линии в зависимости от цвета
            ExportPointSVG(A, color);
            if (enableEndCap)
            {
                ExportPointSVG(B, color);
            }
        }



    }

    public abstract class BaseDrawer : IDrawer
    {
        protected Pen _pen;
        protected Color _color;
        protected Graphics _g;
        protected bool _enableEndCap;
        public Color Color => _color;

        // Конструктор базового рисовальщика
        public BaseDrawer(Graphics g, Color color, bool enableEndCap)
        {
            _color = color;
            _pen = new Pen(_color, 3);
            _g = g;
            _enableEndCap = enableEndCap;
        }

        public void DrawPoint(IPoint p)
        {
            DoDrawPoint(p);
        }

        public void DrawLine(IPoint A, IPoint B, bool enableEndCap)
        {
            DoDrawLine(A, B, enableEndCap);
        }

        protected abstract void DoDrawPoint(IPoint p);
        protected abstract void DoDrawLine(IPoint A, IPoint B, bool enableEndCap);
    }

    public class GreenDrawer : BaseDrawer
    {
        public GreenDrawer(Graphics g, bool enableEndCap) : base(g, Color.Green, enableEndCap)
        {
            _pen.StartCap = LineCap.Round;
            _pen.CustomEndCap = new AdjustableArrowCap(4, 4);
        }

        protected override void DoDrawPoint(IPoint p)
        {
            _g.FillEllipse(new SolidBrush(_color), (int)p.getX() - 2, (int)p.getY() - 2, 4, 4);
        }

        protected override void DoDrawLine(IPoint A, IPoint B, bool enableEndCap)
        {
            Pen pen = new Pen(_color, 3)
            {
                StartCap = LineCap.Round
            };
            if (enableEndCap)
            {
                pen.CustomEndCap = new AdjustableArrowCap(4, 4);
            }
            _g.DrawLine(pen, (int)A.getX(), (int)A.getY(), (int)B.getX(), (int)B.getY());
        }
    }

    public class BlackDrawer : BaseDrawer
    {
        public BlackDrawer(Graphics g, bool enableEndCap) : base(g, Color.Black, enableEndCap)
        {
            _pen.DashStyle = DashStyle.Dot; // Установка стиля пунктира
        }
        protected override void DoDrawPoint(IPoint p)
        {
            _g.FillRectangle(new SolidBrush(_color), (int)p.getX() - 2, (int)p.getY() - 2, 5, 5);
        }

        protected override void DoDrawLine(IPoint A, IPoint B, bool enableEndCap)
        {
            _g.DrawLine(_pen, (int)A.getX(), (int)A.getY(), (int)B.getX(), (int)B.getY());
            if (enableEndCap)
            {
                _g.FillRectangle(new SolidBrush(_color), (int)B.getX() - 2, (int)B.getY() - 2, 5, 5);
            }
        }

    }

    public abstract class ReflectionDrawer : IDrawer
    {
        private readonly IDrawer _innerDrawer;
        protected int _size;

        // Конструктор абстрактного рисовальщика для отражений
        public ReflectionDrawer(IDrawer innerDrawer, int size)
        {
            _innerDrawer = innerDrawer;
            _size = size;
        }

        public Color Color => _innerDrawer.Color;

        public void DrawPoint(IPoint p)
        {
            ReflectPoint(p);
            _innerDrawer.DrawPoint(p);
        }

        public void DrawLine(IPoint A, IPoint B, bool enableEndCap)
        {
            ReflectPoint(A);
            ReflectPoint(B);
            _innerDrawer.DrawLine(A, B, enableEndCap);
        }

        protected abstract void ReflectPoint(IPoint p);
    }

    public class HorizontalReflection : ReflectionDrawer
    {
        public HorizontalReflection(IDrawer innerDrawer, int width) : base(innerDrawer, width) { }

        protected override void ReflectPoint(IPoint p)
        {
            p.setX(_size - p.getX());
        }
    }

    public class VerticalReflection : ReflectionDrawer
    {
        public VerticalReflection(IDrawer innerDrawer, int height) : base(innerDrawer, height) { }

        protected override void ReflectPoint(IPoint p)
        {
            p.setY(_size - p.getY());
        }
    }

    public interface IDrawable
    {
        void Draw(IDrawer d);
    }

    public class AVisualCurve : IDrawable
    {
        private readonly ICurve _curve;
        private readonly Color _color;

        public AVisualCurve(ICurve curve, Color color)
        {
            _curve = curve;
            _color = color;
        }
        public Color Color => _color;

        public void Draw(IDrawer drawer)
        {
            int segments = 10; // Количество сегментов для разбиения кривой
            for (int i = 0; i < segments; ++i)
            {
                IPoint startPoint = _curve.GetPoint(i / (double)segments);
                IPoint endPoint = _curve.GetPoint((i + 1) / (double)segments);
                drawer.DrawLine(startPoint, endPoint, i == segments - 1); // Сохраняем отрезок между начальной и конечной точками
            }
        }

    }
}