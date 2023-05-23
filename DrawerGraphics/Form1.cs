using System;
using System.Windows.Forms;
using DrawerGraphics.Charts;

namespace DrawerGraphics
{
    public partial class Form1 : Form
    {
        private AlgorithmPAM _pam;
        
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
            _pam = new AlgorithmPAM();
            _pam.UpdatePoints();
            
            pane.XAxis.Scale.Min = _pam.XMin;
            pane.XAxis.Scale.Max = _pam.XMax;
            
            pane.YAxis.Scale.Min = _pam.YMin;
            pane.YAxis.Scale.Max = _pam.YMax;

            _pam.CreateCurves(pane);
            
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var pane = zedGraphControl1.GraphPane;
            
            _pam.UpdatePoints();
            _pam.CreateCurves(pane);
            
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }
    }
}