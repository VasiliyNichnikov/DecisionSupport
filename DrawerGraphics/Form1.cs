using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawerGraphics.Charts;
using ZedGraph;

namespace DrawerGraphics
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DrawChartPAM();
        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {
            
        }

        private void DrawChartPAM()
        {
            var pane = zedGraphControl1.GraphPane;
            
            // Очищаем список кривых на тот случай, если до этого сигналы были нарисованы
            pane.CurveList.Clear();
            
            // Алгоритм PAM
            var pam = new AlgorithmPAM();
            pam.UpdatePoints();
            
            pane.XAxis.Scale.Min = pam.XMin;
            pane.XAxis.Scale.Max = pam.XMax;
            
            pane.YAxis.Scale.Min = pam.YMin;
            pane.YAxis.Scale.Max = pam.YMax;

            var curve = pam.CreateCurves(pane);
            
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private void DrawGraphTest()
        {
            var pane = zedGraphControl1.GraphPane;
            
            // Очищаем список кривых на тот случай, если до этого сигналы были нарисованы
            pane.CurveList.Clear();

            var points = new PointPairList();

            var xMin = -100;
            var xMax = 100;

            var yMin = -100;
            var yMax = 100;

            var countPoints = 50;

            var random = new Random();

            for (var i = 0; i < countPoints; i++)
            {
                var x = random.Next(xMin, xMax);
                var y = random.Next(yMin, yMax);

                points.Add(x, y);
            }

            var myCurve = pane.AddCurve("Scatter", points, Color.Blue, SymbolType.Diamond);

            myCurve.Line.IsVisible = false;
            myCurve.Symbol.Fill.Color = Color.Blue;
            myCurve.Symbol.Fill.Type = FillType.Solid;
            myCurve.Symbol.Size = 7;

            pane.XAxis.Scale.Min = xMin;
            pane.XAxis.Scale.Max = xMax;
            
            pane.YAxis.Scale.Min = yMin;
            pane.YAxis.Scale.Max = yMax;

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }
    }
}