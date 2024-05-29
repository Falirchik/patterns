using Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Visual;


namespace LinesAndCurves
{
    public partial class Form1 : Form
    {
        private List<AVisualCurve> ListOfCurves;

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

            AVisualCurve L = new AVisualCurve(new Line(a, b), Color.Black);
            AVisualCurve B = new AVisualCurve(new Bezier(a, b, c, d), Color.Green);
            ICurve bezierCurve = new Bezier(a, b, c, d);
            AVisualCurve mfB = new AVisualCurve(new MoveTo(new Fragment(bezierCurve, 0.66, 0.33), new Geometry.Point(300, 300)), Color.Black);

            ListOfCurves.Add(L);
            ListOfCurves.Add(B);
            ListOfCurves.Add(mfB);
            panel1.Refresh();
        }

        private IDrawer ApplyReflection(IDrawer drawer)
        {
            if (VReflection_checkbox.Checked && HReflection_checkbox.Checked)
            {
                return new VerticalReflection(new HorizontalReflection(drawer, panel1.Width), panel1.Height);
            }
            else if (VReflection_checkbox.Checked)
            {
                return new VerticalReflection(drawer, panel1.Height);
            }
            else if (HReflection_checkbox.Checked)
            {
                return new HorizontalReflection(drawer, panel1.Width);
            }

            return drawer; // ¬озвращаем исходный drawer, если отражение не примен€етс€
        }

        private void DrawCurves(Graphics graphics, StreamWriter writer = null)
        {
            foreach (var curve in ListOfCurves)
            {
                IDrawer blackDrawer = new BlackDrawer(graphics, true);
                IDrawer greenDrawer = new GreenDrawer(graphics, false);

                IDrawer BDrawer = ApplyReflection(blackDrawer);
                IDrawer GDrawer = ApplyReflection(greenDrawer);

                if (writer != null)
                {
                    if (curve.Color == Color.Black)
                    {
                        curve.Draw(new SVGExportingDecorator(BDrawer, writer, true));
                    }
                    else if (curve.Color == Color.Green)
                    {
                        curve.Draw(new SVGExportingDecorator(GDrawer, writer, false));
                    }
                }
                else
                {
                    if (curve.Color == Color.Black)
                    {
                        curve.Draw(BDrawer);
                    }
                    else if (curve.Color == Color.Green)
                    {
                        curve.Draw(GDrawer);
                    }
                }
            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (ListOfCurves.Count == 0)
                return;

            DrawCurves(e.Graphics);
        }
        private void Save_button_Click(object sender, EventArgs e)
        {
            if (ListOfCurves.Count == 0)
                return;

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

                    using (Bitmap bitmap = new Bitmap(panel1.Width, panel1.Height))
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        DrawCurves(graphics, writer);
                    }

                    writer.WriteLine("</svg>");
                    MessageBox.Show("Curves have been saved successfully!");
                }
            }
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

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            // Add mouse click event handling logic here
        }
    }
}
