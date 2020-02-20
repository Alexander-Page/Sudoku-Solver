using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            loadBoxes();
        }

        
        TextBox[,] boxes = new TextBox[9, 9];
        string[,] example = new string[9, 9]{{"1", "", "", "4", "8", "9", "", "", "6" },
                                             {"7", "3", "", "", "", "", "", "4", ""},
                                             {"", "", "", "", "", "1", "2", "9", "5" },
                                             {"", "", "7", "1", "2", "", "6", "", "" },
                                             { "5", "", "", "7", "", "3", "", "", "8"},
                                             { "", "", "6", "", "9", "5", "7", "", ""},
                                             { "9", "1", "4", "6", "", "", "", "", ""},
                                             { "", "2", "", "", "", "", "", "3", "7"},
                                             { "8", "", "", "5", "1", "2", "", "", "4"}}; 

        private void buttonSolve_Click(object sender, EventArgs e)
        { 
            solve(); 
        }


        bool solve()
        {
            int sleepLength = (int)numericUpDownSleep.Value;
            int x = 0, y = 0;
            Tuple<int, int> empty = findEmpty();

            if (empty.Item1 == -1 && empty.Item2 == -1)
            {
                return true;
            }
            else
            {
                y = empty.Item1;
                x = empty.Item2;
            }

            for (int i = 1; i < 10; i++)
            {
                if (validate(i, y, x))
                {
                    boxes[y, x].Text = i.ToString();
                    boxes[y, x].Refresh();
                    System.Threading.Thread.Sleep(sleepLength);
                    if (solve())
                    {
                        return true;
                    }
                    boxes[y, x].Text = "0";
                }

                
            }
            return false;
        }

        private Tuple<int, int> findEmpty()
        {
            //find finds the first empty position and returns the position as a tuple
            // if no empty position is found, return (-1,-1)
            for (int i = 0; i < boxes.GetLength(0); i++)
            {
                for (int j = 0; j < boxes.GetLength(1); j++)
                {
                    if (boxes[i,j].Text.Length == 0 || boxes[i,j].Text.Equals("0"))
                    {
                        return new Tuple<int, int>(i, j);
                    }
                }
            }
            // if there is no empty position return (-1,-1)
            return new Tuple<int, int>(-1, -1);
        }

        // check if a value at a given position is valid
        private bool validate(int val, int y, int x)
        {
            //check row
            for (int i = 0; i < boxes.GetLength(0); i++)
            {
                if (boxes[y,i].Text.Equals(val.ToString()) && x != i)
                {
                    // if an element in the same row is equal to the given element
                    // and is not in the same position, return false
                    return false;
                }
            }

            //check column
            for (int i = 0; i < boxes.GetLength(1); i++)
            {
                if (boxes[i,x].Text.Equals(val.ToString()) && y != i)
                {
                    return false;
                }
            }

            //check box
            int box_x = x / 3;
            int box_y = y / 3;
            for (int i = box_y*3; i < box_y*3+3; i++)
            {                
                for (int j = box_x*3; j < box_x*3+3; j++)
                {
                    if(boxes[i,j].Text.Equals(val.ToString()) && i != y && j != x)
                    {
                        // if and element in the same box is equal to the given element
                        // and the postition is not the same as the given 
                        // postion, return false
                        return false;
                    }
                }
            }

            // if all 3 checks pass, the the position is valid. return true
            return true;
        }

        private void loadBoxes()
        {
            int x = 8;
            int y = 8;
            foreach (TextBox textbox in Controls.OfType<TextBox>())
            {
                //System.Diagnostics.Debug.WriteLine($"{textbox.Name}\ty = {y}\tx = {x}");
                boxes[y, x] = textbox;
                if (x == 0) { y--; x = 8; }
                else if (x > 0) { x--; }
            }
        }

        private void buttonLoadExample_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < boxes.GetLength(0); i++)
            {
                for (int j = 0; j < boxes.GetLength(1); j++)
                {
                    boxes[i, j].Text = example[i, j];
                }
            }
        }
    }
}
