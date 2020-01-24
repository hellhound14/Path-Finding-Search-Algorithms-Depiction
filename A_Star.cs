using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace A
{
    public class A_Algorithm : Form1
    {
        public static int lowestFscore() // Index_of_spot_with_lowest_F_score
        {
            int bestind = 0;
            for (int i = 0; i < openset.Count; i++)
            {
                if (openset[i].f < openset[bestind].f)
                {
                    bestind = i;
                }
            }
            return bestind;
        }

        public static double heuristic(Spot a, Spot b) // Distance_between_end_and_spot
        {
            var dist = Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));
            return dist;
        }

        public static void Algorithm(Panel grid)
        {
            openset.Add(start);

            while (true)
            {
                foreach (var spot in blocked)
                {
                    spot.color(Color.Black, gr, grid);
                }
                foreach (var spot in openset)
                {
                    spot.color(Color.LawnGreen, gr, grid);
                }
                foreach (var spot in closedset)
                {
                    spot.color(Color.Red, gr, grid);
                }

                if (openset.Count == 0)
                {
                    MessageBox.Show("No Possible Solution");
                    break;
                }
                else
                {
                    int best = lowestFscore();
                    Spot current = openset[best];
                    if (current == end)
                    {
                        var temp = current;
                        path.Add(current);
                        while (temp.prev != null)
                        {
                            try
                            {
                                path.Add(temp.prev);
                                temp = temp.prev;

                            }
                            catch 
                            {

                            }
                        }
                        foreach (var spot in path)
                        {
                            spot.color(Color.RoyalBlue, gr, grid);
                        }

                        MessageBox.Show("Reached");
                        break;
                    }
                    closedset.Add(current);
                    openset.Remove(current);

                    current.addneigh(matrix);
                    var neighbours = current.neigh;
                    foreach (var neighbour in neighbours)
                    {
                        if (!closedset.Contains(neighbour) && !neighbour.wall)
                        {
                            var go = false;
                            var tempg = current.g + heuristic(current, neighbour);
                            if (openset.Contains(neighbour))
                            {
                                if (tempg < neighbour.g)
                                {
                                    neighbour.g = tempg;
                                    go = true;
                                }
                            }
                            else
                            {
                                neighbour.g = tempg;
                                openset.Add(neighbour);
                                go = true;
                            }
                            if (go)
                            {
                                neighbour.h = heuristic(neighbour, end);
                                neighbour.f = neighbour.g + neighbour.h;
                                neighbour.prev = current;
                            }
                        }
                    }
                }
            }
        }
    }
}
