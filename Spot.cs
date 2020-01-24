using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace A
{
    public class Spot : Form1
    {
        public int x, y;
        public double f, g, h;

        public List<Spot> neigh;

        public Spot prev;
        public bool wall;
        public Spot(int i, int j)
        {
            x = i;
            y = j;
            f = 0;
            g = 0;
            h = 0;

            neigh = new List<Spot>();
            prev = null;
            wall = false;
        }
        public void color(Color color,Graphics gr,Panel grid) // Change color of this.spot
        {
            SolidBrush brush = new SolidBrush(color);
            Rectangle rect = new Rectangle(x * grid.Width / rows, y * grid.Width / columns, (grid.Width / rows) - 1, (grid.Height / columns) - 1);
            gr.FillRectangle(brush, rect);
            if (wall == false)
            {
                TextRenderer.DrawText(gr, (x+1).ToString(), cFont, rect, Color.Gray, TextFormatFlags.Top | TextFormatFlags.Top); //Draw X co-ordinate for each spot
                TextRenderer.DrawText(gr, (y+1).ToString(), cFont, rect, Color.Gray, TextFormatFlags.Bottom | TextFormatFlags.Right); //Draw Y co-ordinate for each spot
            }
        }
        public void addneigh(Spot[,] matrix) //Add all the possible neighbours of this.spot
        {
            if (y < columns - 1)
            {
                if (x < rows - 1)
                {
                    neigh.Add(matrix[x + 1, y + 1]); //Down-Right
                }
                neigh.Add(matrix[x, y + 1]); //Down
            }

            if (x < rows - 1)
            {
                if (y > 0)
                {
                    neigh.Add(matrix[x + 1, y - 1]);//Up-Right
                }
                neigh.Add(matrix[x + 1, y]); //Right
            }

            if (y > 0)
            {
                if (x > rows - 1)
                {
                    neigh.Add(matrix[x - 1, y - 1]);//Up-Left
                }
                neigh.Add(matrix[x, y - 1]); //Up
            }

            

            if (x > 0)
            {
                if (y < columns-1)
                {
                    neigh.Add(matrix[x - 1, y + 1]);//Down-Left
                }
                neigh.Add(matrix[x - 1, y]); //Left
            }
        }
    }
}