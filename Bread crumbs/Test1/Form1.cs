using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//
// May, 2012, J. Loftus.  Code to demonstrate how to use a Dequeue (FIFO queue) to draw a trace (the bread crumbs) 
// behind the game element that will disappear in the same order in which it was drawn.  
//  Part of a weekly workshop offered during the Summer 2012 entitled "Applied Data Structures
// and Algorithms for Interactive Simulation systems".  
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
        public Rectangle cr, cr1, dr1, oldrect, newrect;
        bool resetflag;
        Pen blackPen; Pen yellowPen;
        public int direction;
        public Pair<int, int> crumbloc;
        Queue<Pair<int, int>> trail;  // The is the FIFO queue, it houses a Pair of integers, representing where a 
                                      // single bread crumb was drawn. 
        public Pair<int, int> oldspot;
        public Rectangle spotrect;
        public int delay = 35;
        public int delaycount = 0;
        public bool startflag;
        public Form1()
        {
            InitializeComponent();
            toggle = true; times = 0;
            DY = 20; DX = 20; startflag = false;
            ran = new Random();
            rw = 0; cl = 0; x = 0; y = 0;
            resetflag = true; direction = 1;
            trail = new Queue<Pair<int, int>>();
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
                if (startflag == true)
                {
                    drawTrail(oldspot.First, oldspot.Second, greyBrush);
                    grid.gridRec[rw, cl].setCrumbs(false);
                }
                int locx = ran.Next(15);
                int locy = ran.Next(15);
                int yelflag = ran.Next(14);
                switch (direction)
                {
                    case 0:  // up
                       
                            oldrect = new Rectangle(x+3,y+3, 14, 14);
                            newrect = new Rectangle(x+3, y-17, 14, 14);
                            paper.FillEllipse(greyBrush, oldrect);
                           
                            if (yelflag == 3) paper.FillEllipse(yellowbrush, newrect);
                            else paper.FillEllipse(bluebrush, newrect);
                            drawWall(x, y-20, blackPen); drawWall(x, y, blackPen);
                            crumbloc = new Pair<int,int>(x+locx,y+locy);
                            trail.Enqueue(crumbloc);
                            grid.gridRec[rw, cl].setCrumbs(true);
                            if (yelflag == 3) drawTrail(x + locx, y + locy, yellowbrush);
                            else drawTrail(x + locx, y + locy, bluebrush);
                            
                            rw--; y = y - 20;
                            break;
                    case 1:  // down
                            oldrect = new Rectangle(x+3, y+3, 14, 14);
                            newrect = new Rectangle(x+3, y + 23, 14, 14);
                            paper.FillEllipse(greyBrush, oldrect);
                        
                            if (yelflag == 3) paper.FillEllipse(yellowbrush, newrect);
                            else paper.FillEllipse(bluebrush, newrect);
                            drawWall(x, y + 20, blackPen); drawWall(x, y, blackPen);
                            crumbloc = new Pair<int,int>(x+locx,y+locy);
                            trail.Enqueue(crumbloc);
                            grid.gridRec[rw, cl].setCrumbs(true);
                            if (yelflag == 3) drawTrail(x + locx, y + locy, yellowbrush);
                            else drawTrail(x+locx, y+locy, bluebrush);
                            rw++; y = y + 20;
                            break;
                    case 2:  // left
                            oldrect = new Rectangle(x+3, y+3, 14, 14);
                            newrect = new Rectangle(x-17, y+3, 14, 14);
                            paper.FillEllipse(greyBrush, oldrect);
                          
                            if (yelflag == 3) paper.FillEllipse(yellowbrush, newrect);
                            else paper.FillEllipse(bluebrush, newrect);
                            drawWall(x-20, y, blackPen); drawWall(x, y, blackPen);
                            crumbloc = new Pair<int,int>(x+locx,y+locy);
                            trail.Enqueue(crumbloc);
                            grid.gridRec[rw, cl].setCrumbs(true);
                           
                            if (yelflag == 3) drawTrail(x + locx, y + locy, yellowbrush);
                            else drawTrail(x + locx, y + locy, bluebrush);
                            cl--; x = x - 20;
                            break;
                    case 3:  // right
                            oldrect = new Rectangle(x+3, y+3, 14, 14);
                            newrect = new Rectangle(x+23, y+3, 14, 14);
                            paper.FillEllipse(greyBrush, oldrect);
                           
                            if (yelflag == 3) paper.FillEllipse(yellowbrush, newrect);
                            else paper.FillEllipse(bluebrush, newrect);
                            drawWall(x+20, y, blackPen); drawWall(x, y, blackPen);
                            crumbloc = new Pair<int,int>(x+locx,y+locy);
                            trail.Enqueue(crumbloc);
                            grid.gridRec[rw, cl].setCrumbs(true);
                           
                            if (yelflag == 3) drawTrail(x + locx, y + locy, yellowbrush);
                            else drawTrail(x + locx, y + locy, bluebrush);
                             cl++; x = x + 20;
                            break;
                }  // end of switch
                    
                                                
              
            }  // if resetflag == false

            resetflag = false;
        }  // end of paint

        private void drawTrail(int X, int Y, Brush b)
        {
            
            Rectangle drop = new Rectangle(X, Y, 2, 2);
            paper.FillRectangle(b, drop);

        }

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
            delaycount++;
            if (delaycount > delay) {startflag = true; delaycount = 0;} 

            bool goflag = false; resetflag = false;
            if (ran.Next(3) == 1) direction = ran.Next(4);
            int docount = 0;
            do
            {
                
                switch (direction)
                {
                    case 0:  // up
                        if ((y > 0) & (grid.gridRec[rw, cl].getUp())  )
                        {
                            if (grid.gridRec[rw - 1, cl].isWall() || (docount > 3)  )
                            {
                                cr = new Rectangle(x, y - 20, 20, 40);
                                goflag = true;
                                grid.gridRec[rw - 1, cl].makeFree();
                               
                            }
                        }
                        break;
                    case 1:  // down
                        if ((y < 480) & (grid.gridRec[rw, cl].getDown()) )
                        {
                            if (grid.gridRec[rw + 1, cl].isWall()|| (docount > 3) )
                            {
                                cr = new Rectangle(x, y, 20, 40);                           
                                goflag = true;
                                grid.gridRec[rw + 1, cl].makeFree();
                               
                            }
                        }
                        break;
                    case 2:  // left
                        if ((x > 0) & (grid.gridRec[rw, cl].getLeft()) )
                        {
                            if (grid.gridRec[rw, cl - 1].isWall() || (docount > 3) )
                            {
                                cr = new Rectangle(x - 20, y, 40, 20);
                                goflag = true;
                                grid.gridRec[rw, cl - 1].makeFree();
                               
                            }
                            
                        }
                        break;
                    case 3:  // right
                        if ((x < 480) & (grid.gridRec[rw, cl].getRight()) )
                        {
                            if (grid.gridRec[rw, cl + 1].isWall() || (docount > 3) )
                            {
                                cr = new Rectangle(x, y, 40, 20);
                              
                                goflag = true;
                                grid.gridRec[rw, cl + 1].makeFree();
                              
                            }
                        }
                        break;
                        
                }
                if (goflag == false) direction = ran.Next(4);
                docount++;
            } while (goflag == false);
            cr1 = new Rectangle(x + 5, y + 5, 2, 2);
                Graphics g = panel1.CreateGraphics();
                if (startflag == true)
                {
                    oldspot = trail.Dequeue();
                    spotrect = new Rectangle(oldspot.First, oldspot.Second, 2, 2);
                    panel1.Invalidate(spotrect);
                }
                        
    
                panel1.Invalidate(cr);
                panel1.Invalidate(cr1);
               
                panel1.Update();
            
            
        } // end of timer definition
                
    }
}

 
      