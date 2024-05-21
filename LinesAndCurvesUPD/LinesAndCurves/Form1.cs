using Geometry;
using System.Diagnostics.Eventing.Reader;
using Visual;

namespace LinesAndCurves
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ListOfCurves = new List<AVisualCurve>();
        }

        private void HReflection_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Refresh();
        }

        private void VReflection_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Refresh();
        }

        private void Generate_button_Click(object sender, EventArgs e)
        {
            ListOfCurves.Clear();
            Random rnd = new Random();
            IPoint a = new Geometry.Point((float)rnd.Next(50, 600), (float)rnd.Next(50, 400));
            IPoint b = new Geometry.Point((float)rnd.Next(50, 600), (float)rnd.Next(50, 400));
            IPoint c = new Geometry.Point((float)rnd.Next(50, 600), (float)rnd.Next(50, 400));
            IPoint d = new Geometry.Point((float)rnd.Next(50, 600), (float)rnd.Next(50, 400));

            ISVGExporter svgExporter = new SVGExporter(); 
            AVisualCurve L = new AVisualCurve(new Line(a, b), svgExporter, Color.Black);
            AVisualCurve B = new AVisualCurve(new Bezier(a, b, c, d), svgExporter, Color.Green);
            AVisualCurve mfB = new AVisualCurve(new MoveTo(new Fragment(B, 0.66, 0.33), new Geometry.Point(300, 300)), svgExporter, Color.Black);


            ListOfCurves.Add(L);
            ListOfCurves.Add(B);
            ListOfCurves.Add(mfB);
            panel1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (ListOfCurves.Count == 0)
            {
                return;
            }

            Color greenColor = Color.Green; 
            Color blackColor = Color.Black;
            IDrawer black, green;

            if (VReflection_checkbox.Checked & HReflection_checkbox.Checked)
            {
                black = new VerticalReflection(new HorizontalReflection(new BlackDrawer(e.Graphics, blackColor), panel1.Width), panel1.Height);
                green = new VerticalReflection(new HorizontalReflection(new GreenDrawer(e.Graphics, greenColor), panel1.Width), panel1.Height);
            }
            else if (VReflection_checkbox.Checked)
            {
                black = new VerticalReflection(new BlackDrawer(e.Graphics, blackColor), panel1.Height);
                green = new VerticalReflection(new GreenDrawer(e.Graphics, greenColor), panel1.Height);
            }
            else if (HReflection_checkbox.Checked)
            {
                black = new HorizontalReflection(new BlackDrawer(e.Graphics, blackColor), panel1.Width);
                green = new HorizontalReflection(new GreenDrawer(e.Graphics, greenColor), panel1.Width);
            }
            else
            {
                black = new BlackDrawer(e.Graphics, blackColor);
                green = new GreenDrawer(e.Graphics, greenColor);
            }

            ListOfCurves[0].Draw(black);
            ListOfCurves[1].Draw(green);
            ListOfCurves[2].Draw(black);

        }

        private void WriteSvgHeader(StreamWriter writer, int width, int height)
        {
            writer.WriteLine("<?xml version=\"1.0\" standalone=\"no\"?>");
            writer.WriteLine("<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" ");
            writer.WriteLine("\"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">");
            writer.WriteLine($"<svg width=\"{width}\" height=\"{height}\" version=\"1.1\" ");
            writer.WriteLine("xmlns=\"http://www.w3.org/2000/svg\">");
        }

        private void WriteMarkerDefinition(StreamWriter writer)
        {
            writer.WriteLine("<defs>");
            writer.WriteLine("  <marker id=\"arrow\" viewBox=\"0 0 5 5\" refX=\"3\" refY=\"3\" markerWidth=\"5\" markerHeight=\"5\" orient=\"auto\">");
            writer.WriteLine("    <path d=\"M 0 0 L 5 3 L 0 5 Z\" fill=\"green\"/>");
            writer.WriteLine("  </marker>");
            writer.WriteLine("</defs>");
        }
        
        private void Save_button_Click(object sender, EventArgs e)
        {
            if (ListOfCurves.Count == 0)
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "SVG files (*.svg)|*.svg";
            saveFileDialog.Title = "Save SVG File";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    WriteSvgHeader(writer, panel1.Width, panel1.Height);
                    WriteMarkerDefinition(writer);

                    ISVGExporter svgExporter = new SVGExporter();

                    ListOfCurves[0].ExportToSVG(writer);
                    ListOfCurves[1].ExportToSVG(writer);
                    ListOfCurves[2].ExportToSVG(writer);

                    writer.WriteLine("</svg>");
                    MessageBox.Show("Curves have been saved successfully!");
                }
            }
        }

        IDrawer black, green;
        private List<AVisualCurve> ListOfCurves;
    }
}
