using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// 
// J. Loftus June 2012, for Luzerne County Community College.
// Demonstration code created as part of a weekly workshop offered at LCCC during the Summer
// of 2012 by J. Loftus, entitled "Applied Data Structures and Algorithms for Interactive Simulation Systems"
// Code demonstrates how to randomly set direction in response to a System.timer event, and how to redraw
// the walls of the maze as the game element moves along.  (This is done in yellow so one can see what's happening.)
// 
namespace Test1
{
    public partial class Form1 : Form
    {
        Graphics paper;
        int rw, cl, x, y;
        Grid grid;
        public Random ran;
        public static bool toggle;
        public static int times;
        public static int DY;
        public static int DX;
        public Rectangle cr, oldrect, newrect;
        bool stop, resetflag;
        Pen blackPen; Pen yellowPen;
        public int direction;
        public Form1()
        {
            InitializeComponent();
            toggle = true; times = 0;
            DY = 20; DX = 20; stop = false;
            ran = new Random();
            rw = 0; cl = 0; x = 0; y = 0; direction = 1;
            resetflag = true;
        }

        // When the panel in which the Grid of squares is being drawn is hit with a Paint event,
        // then we call drawGrid to redraw the maze. 
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            paper = e.Graphics;
            SolidBrush bluebrush = new SolidBrush(Color.Blue);
            SolidBrush yellowbrush = new SolidBrush(Color.Yellow);
            SolidBrush greyBrush = new SolidBrush(Color.DimGray);
            blackPen = new Pen(Color.Black); yellowPen = new Pen(Color.Yellow);
            if (resetflag == true)
            {
               
                grid = new Grid();
                grid.drawGrid(paper);
                                       
            }
            if (resetflag == false)
            {
                switch (direction)
                {
                    case 0:  // up
                            oldrect = new Rectangle(x+3,y+3, 14, 14);
                            newrect = new Rectangle(x+3, y-17, 14, 14);
                            paper.FillEllipse(greyBrush, oldrect);
                            paper.FillEllipse(bluebrush, newrect);
                            drawWall(x, y-20, yellowPen); drawWall(x, y, yellowPen);
                            rw--; y = y - 20;
                            break;
                    case 1:  // down
                            oldrect = new Rectangle(x+3, y+3, 14, 14);
                            newrect = new Rectangle(x+3, y + 23, 14, 14);
                            paper.FillEllipse(greyBrush, oldrect);
                            paper.FillEllipse(bluebrush, newrect);
                            drawWall(x, y + 20, yellowPen); drawWall(x, y, yellowPen);
                            rw++; y = y + 20;
                            break;
                    case 2:  // left
                            oldrect = new Rectangle(x+3, y+3, 14, 14);
                            newrect = new Rectangle(x-17, y+3, 14, 14);
                            paper.FillEllipse(greyBrush, oldrect);
                            paper.FillEllipse(bluebrush, newrect);
                            drawWall(x-20, y, yellowPen); drawWall(x, y, yellowPen);
                            cl--; x = x - 20;
                            break;
                    case 3:  // right
                            oldrect = new Rectangle(x+3, y+3, 14, 14);
                            newrect = new Rectangle(x+23, y+3, 14, 14);
                            paper.FillEllipse(greyBrush, oldrect);
                            paper.FillEllipse(bluebrush, newrect);
                            drawWall(x+20, y, yellowPen); drawWall(x, y, yellowPen);
                            cl++; x = x + 20;
                            break;
                }  // end of switch
                    
                                                
              
            }  // if resetflag == false

            resetflag = false;
        }  // end of paint

        private void drawWall(int X, int Y, Pen p)
        {
            int i = Y/20; int j = X/20;
            // up
            if (  (Y > 0) & (!grid.gridRec[i,j].getUp())  )
            {
                Point pt1 = new Point(X, Y);
                Point pt2 = new Point(X+20, Y);
                paper.DrawLine(p, pt1, pt2);
            }
            // down
            if (  (Y < 480) & (!grid.gridRec[i,j].getDown())  )
            {
                Point pt1 = new Point(X, Y+20);
                Point pt2 = new Point(X + 20, Y+20);
                paper.DrawLine(p, pt1, pt2);
            }
            // left
            if (  (X > 0) & (!grid.gridRec[i,j].getLeft())   ) 
            {
                Point pt1 = new Point(X, Y);
                Point pt2 = new Point(X, Y+20);
                paper.DrawLine(p, pt1, pt2);
            }
            // right
            if (  (X < 480) & (!grid.gridRec[i,j].getRight())  ) 
            {
                Point pt1 = new Point(X+20, Y);
                Point pt2 = new Point(X+20, Y+20);
                paper.DrawLine(p, pt1, pt2);
            }
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            ran = new Random();
            rw = 0; cl = 0; x = 0; y = 0; direction = 1;
            resetflag = true;
            panel1.Refresh();
        }
        
        private void label2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Goodbye!");
            Close();
        }
       
         
        private void timer1_Tick(object sender, EventArgs e)
        {

            bool goflag = false; resetflag = false;
            if (ran.Next(3) == 1) direction = ran.Next(4);
            do
            {
                
                switch (direction)
                {
                    case 0:  // up
                        if ((y > 0) & (grid.gridRec[rw, cl].getUp()) )
                        {
                            cr = new Rectangle(x, y - 20, 20, 40);                   
                            goflag = true;
                        }
                        break;
                    case 1:  // down
                        if ((y < 480) & (grid.gridRec[rw, cl].getDown()))
                        {
                            cr = new Rectangle(x, y, 20, 40);                           
                            goflag = true;
                        }
                        break;
                    case 2:  // left
                        if ((x > 0) & (grid.gridRec[rw, cl].getLeft()))
                        {
                            cr = new Rectangle(x - 20, y, 40, 20);                           
                            goflag = true;
                        }
                        break;
                    case 3:  // right
                        if ((x < 480) & (grid.gridRec[rw, cl].getRight()))
                        {
                            cr = new Rectangle(x, y, 40, 20);                           
                            goflag = true;
                        }
                        break;
                        
                }
                if (goflag == false) direction = ran.Next(4);
            } while (goflag == false);
           
                Graphics g = panel1.CreateGraphics();
                        
    //            if (toggle == true) toggle = false;
    //            else toggle = true;
                panel1.Invalidate(cr);
                panel1.Update();
            
            
        } // end of timer definition
                
    }
}

 
      