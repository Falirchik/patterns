using Geometry;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Visual;

namespace LinesAndCurves
{
    public partial class Main_Form : Form
    {
        List<VisualCurve> curves;
        private bool generate_new = false;
        public Main_Form()
        {
            InitializeComponent();
            generate_new = false;
            curves = new List<VisualCurve> ();
            //curves = new IDrawable[2];
        }

        private void Main_Form_Generate_Button_Clicked(object sender, EventArgs e)
        {
            generate_new = true;
            panel1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (generate_new == true)
            {
                curves.Clear();
                Random rnd = new Random();
                IPoint a = new Geometry.Point((float)rnd.Next(200, 500), (float)rnd.Next(100, 300));
                IPoint b = new Geometry.Point((float)rnd.Next(200, 500), (float)rnd.Next(100, 300));
                IPoint c = new Geometry.Point((float)rnd.Next(200, 500), (float)rnd.Next(100, 300));
                IPoint d = new Geometry.Point((float)rnd.Next(200, 500), (float)rnd.Next(100, 300));

                curves.Add(new VisualCurve(new Line(a, b)));
                curves.Add(new VisualCurve(new Bezier(a, b, c, d)));
                generate_new = false;
            }

            IDrawer black = new BlackDrawer(e.Graphics);
            IDrawer green = new GreenDrawer(e.Graphics);

            if (curves.Count > 0)
            {
                curves[0].Draw(green);
                curves[1].Draw(black);
            }

            if (checkBox1.Checked && curves.Count > 0)
            {
                IDrawer chiralgreen = new ChiralDrawer(green);
                IDrawer chiralblack = new ChiralDrawer(black);
                curves[0].Draw(chiralgreen);
                curves[1].Draw(chiralblack);
            }
        }

        private void panel1_draw_mirror(object sender, PaintEventArgs e)
        {
            if (checkBox1.Checked)
            {
                curves[0].Draw(new ChiralDrawer(new GreenDrawer(e.Graphics)));
                curves[1].Draw(new ChiralDrawer(new BlackDrawer(e.Graphics)));
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            generate_new = false;
            panel1.Refresh();
        }

        private void Save_Button_Clicked(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "SVG files (*.svg)|*.svg";
            saveFileDialog.Title = "Save SVG File";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                SaveToSVG(filePath, panel1.Width, panel1.Height);
            }
        }

        private void SaveToSVG(string filePath, int width, int height)
        {
            // ������� SVG-���� � ��������� ��� ��� ������
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // ���������� ��������� SVG
                writer.WriteLine("<?xml version=\"1.0\" standalone=\"no\"?>");
                writer.WriteLine("<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" ");
                writer.WriteLine("\"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">");
                writer.WriteLine($"<svg width=\"{width}\" height=\"{height}\" version=\"1.1\" ");
                writer.WriteLine("xmlns=\"http://www.w3.org/2000/svg\">");

                // �������� �� ������ ����� � ���������� ������ � SVG
                foreach (var curve in curves)
                {
                    //writer.WriteLine(curve.ToSvgPath());
                }

                // ��������� ��� SVG
                writer.WriteLine("</svg>");
            }

            MessageBox.Show("������������ ����� ������� ��������� � ������� SVG �� ����: " + filePath);
        }

        
    }
}
