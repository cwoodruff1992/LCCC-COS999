using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//
// May, 2012, J. Loftus, for Luzerne County Community College.
// Code to test and work with recursive backtracking for random
// maze generated.  Part of a weekly workshop offered during the Summer 2012 entitled "Applied Data Structures
// and Algorithms for Interactive Simulation systems".  
//
namespace Test1
{
    public partial class Form1 : Form
    {
        Graphics paper;
     
        public Random rand;
        public Form1()
        {
            InitializeComponent();
        }

        // When the panel in which the Grid of squares is being drawn is hit with a Point event,
        // then we call drawGrid to redraw the maze. 
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            paper = e.Graphics;
            Grid grid = new Grid();
            grid.drawGrid(paper);
        }
         

        private void label1_Click_1(object sender, EventArgs e)
        {
            
            panel1.Refresh();
        }
        
        private void label2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Goodbye!");
            Close();
        }
                
    }
}

 
      