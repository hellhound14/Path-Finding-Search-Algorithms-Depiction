using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace A
{
    public partial class Form1 : Form
    {
        public bool LmouseDown = false;
        public bool RmouseDown = false;

        public static int rows = 25;
        public static int columns = 25;

        SolidBrush white = new SolidBrush(Color.White);

        public static Graphics gr = null;

        public static List<Spot> openset, closedset, path, blocked;
        public List<Spot> allblocks;

        public static Spot[,] matrix;

        public static Spot start, end;

        public static Font cFont;
        public Form1()
        {
            openset = new List<Spot>();
            closedset = new List<Spot>();
            path = new List<Spot>();
            blocked = new List<Spot>();
            allblocks = new List<Spot>();

            cFont = new Font(SystemFonts.DefaultFont.FontFamily, 135/rows, FontStyle.Regular);

            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            endx.Text = rows.ToString();
            endy.Text = columns.ToString();

            matrix = new Spot[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = new Spot(i, j);
                    allblocks.Add(matrix[i, j]);
                }
            }
        }


        public void resetwalls(bool allreset) // Reset_blocked
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j].f = 0;
                    matrix[i, j].g = 0;
                    matrix[i, j].h = 0;
                }
            }
            if (allreset)
            {
                foreach (var spot in blocked)
                {
                    spot.wall = false;
                    spot.color(Color.White, gr, grid);
                }
                blocked.Clear();
            }
        }

        private void grid_Paint(object sender, PaintEventArgs e) // Starting_blocks
        {
            gr = grid.CreateGraphics();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Rectangle rect = new Rectangle(i * grid.Width / rows, j * grid.Width / columns, (grid.Width / rows) - 1, (grid.Height / columns) - 1);
                    gr.FillRectangle(white, rect);                    
                    TextRenderer.DrawText(gr, (i+1).ToString(), cFont, rect, Color.Gray, TextFormatFlags.Top | TextFormatFlags.Top);
                    TextRenderer.DrawText(gr, (j+1).ToString(), cFont, rect, Color.Gray, TextFormatFlags.Bottom | TextFormatFlags.Right);
                }
            }
        }

        private void grid_MouseDown(object sender, MouseEventArgs e) 
        {
            if (e.Button == MouseButtons.Left) 
            {
                LmouseDown = true;
            }
            else if (e.Button == MouseButtons.Right) 
            {
                RmouseDown = true;
            }
        }

        private void grid_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                LmouseDown = false;
            }
            else if (e.Button == MouseButtons.Right)
            {
                RmouseDown = false;
            }
        }

        private void grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (LmouseDown || RmouseDown)
            {
                int x = grid.PointToClient(Cursor.Position).X * rows / grid.Width;
                int y = grid.PointToClient(Cursor.Position).Y * columns / grid.Height;
                try
                {
                    resetwalls(false);
                    var spot = matrix[x, y];
                    if (!blocked.Contains(matrix[x, y]) && LmouseDown)
                    {
                        blocked.Add(spot);
                        spot.wall = true;
                        spot.color(Color.Black, gr, grid);
                    }
                    else if(blocked.Contains(matrix[x,y]) && RmouseDown)
                    {
                        blocked.Remove(spot);
                        spot.wall = false;
                        spot.color(Color.White, gr, grid);
                    }

                }
                catch  { 
                }      
            }
        }

        private void clear_Click(object sender, EventArgs e) => resetwalls(true); 

        private void Search_Click(object sender, EventArgs e)
        {
            openset.Clear();
            closedset.Clear();
            path.Clear();

            foreach (var spot in allblocks)
            {
                spot.color(Color.White, gr, grid);
            }

            start = matrix[Convert.ToInt32(startx.Text) - 1, Convert.ToInt32(starty.Text) - 1];
            end = matrix[Convert.ToInt32(endx.Text) - 1, Convert.ToInt32(endy.Text) - 1];

            if (start.wall == true || end.wall == true)
            {
                resetwalls(true);
                MessageBox.Show("Start or End is Blocked");
                return;
            }

            A_Algorithm.Algorithm(grid);
        }
    }
}
