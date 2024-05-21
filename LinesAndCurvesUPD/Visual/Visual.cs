using Geometry;
using System.Drawing;

namespace Visual
{
    public interface IDrawer
    {
        void DrawPoint(IPoint p);
        void DrawLine(IPoint a, IPoint b, bool EnableEndCap, Color color);

    }

    public interface ISVGExporter
    {
        void ExportPointSVG(IPoint p, StreamWriter writer);
        void ExportLineSVG(IPoint A, IPoint B, bool EnableEndCap, StreamWriter writer);
    }

    public class HorizontalReflection : IDrawer
    {
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
            _innerDrawer.DrawLine(A, B, EnableEndCap, color); 
        }
    }

    public class VerticalReflection : IDrawer
    {
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
            _innerDrawer.DrawLine(A, B, EnableEndCap, color);
        }
    }

    public class GreenDrawer : IDrawer
    {
        private Pen _pen;
        private Color _color;
        private Graphics _g;

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
            Pen pen = new Pen(color, 3); 
            _g.DrawLine(pen, (int)A.getX(), (int)A.getY(), (int)B.getX(), (int)B.getY());
            if (EnableEndCap) DrawPoint(B);
        }

    }

    public class SVGExporter : ISVGExporter
    {
        public void ExportPointSVG(IPoint p, StreamWriter writer)
        {
            writer.WriteLine($"<ellipse cx=\"{(int)p.getX()}\" cy=\"{(int)p.getY()}\" rx=\"3\" ry=\"3\" />");
        }

        public void ExportLineSVG(IPoint A, IPoint B, bool EnableEndCap, StreamWriter writer)
        {
            writer.WriteLine($"<line x1=\"{(int)A.getX()}\" y1=\"{(int)A.getY()}\" x2=\"{(int)B.getX()}\" y2=\"{(int)B.getY()}\"");
            if (EnableEndCap)
            {
                writer.WriteLine(" marker-end=\"url(#arrow)\"");
            }
            writer.WriteLine(" />");
        }

        public void ExportLineSVG(IPoint A, IPoint B, bool EnableEndCap, Color color, StreamWriter writer)
        {
            string colorHex = ColorTranslator.ToHtml(color);
            writer.WriteLine($"<line x1=\"{(int)A.getX()}\" y1=\"{(int)A.getY()}\" x2=\"{(int)B.getX()}\" y2=\"{(int)B.getY()}\"");
            if (EnableEndCap)
            {
                writer.WriteLine(" marker-end=\"url(#arrow)\"");
            }
            writer.WriteLine($" stroke=\"{colorHex}\" />");
        }
    }


    // ABSTRACTION
    public interface IDrawable
    {
        public void Draw(IDrawer d);
    }

    public class AVisualCurve : IDrawable, ICurve
    {
        private ISVGExporter _svgExporter;
        private ICurve _c;
        private Color _color; 

        public AVisualCurve(ICurve c, ISVGExporter svgExporter, Color color)
        {
            _c = c;
            _svgExporter = svgExporter;
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

        public void ExportToSVG(StreamWriter writer)
        {
            _svgExporter.ExportPointSVG(_c.GetPoint(0), writer);
            int n = 10;
            for (int i = 0; i < n; ++i)
            {
                _svgExporter.ExportLineSVG(_c.GetPoint(i / (double)n), _c.GetPoint((i + 1) / (double)n), i == n - 1, writer);
            }
        }

        public IPoint GetPoint(double t)
        {
            return _c.GetPoint(t);
        }
    }

    //public class VisualLine : AVisualCurve
    //{
    //    public VisualLine(Line l) : base(l) { }
    //    public override void Draw(IDrawer d)
    //    {
    //        d.DrawPoint(_c.GetPoint(0));
    //        int n = 10;
    //        for (int i = 0; i < n; ++i)
    //        {
    //            d.DrawLine(_c.GetPoint(i / (double)n), _c.GetPoint((i + 1) / (double)n), i == n - 1);
    //        }
    //    }
    //}

    //public class VisualBezier : AVisualCurve
    //{
    //    public VisualBezier(Bezier b) : base(b) { }
    //    public override void Draw(IDrawer d)
    //    {
    //        d.DrawPoint(_c.GetPoint(0));
    //        int n = 10;
    //        for (int i = 0; i < n; ++i)
    //        {
    //            d.DrawLine(_c.GetPoint(i / (double)n), _c.GetPoint((i + 1) / (double)n), i == n - 1);
    //        }
    //    }
    //}


}
