using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//
// June, 2012, J. Loftus, for Luzerne County Community College. 
// Demonstration of implementing recursion using a stack explicitly.  
// The stack is used for the game element to recursively find his way out of a randomly generated maze. 
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
        int wait;
        public static bool toggle;
        public bool victory;
        public static int times;
        public static int DY;
        public static int DX;
        public Rectangle cr, cr1, dr1, oldrect, newrect;
        bool resetflag;
        Pen blackPen; Pen yellowPen;
        public int direction;
        public Pair<int, int> crumbloc;
        Queue<Pair<int, int>> trail;
        Stack<String> path;
        public String pdirection;
        public Pair<int, int> oldspot;
        public Rectangle spotrect;
        public int delay = 35; public int victally = 0;
        public int delaycount = 0;
        public bool startflag;
        Brush currentBrush;
        public bool Lflag, Rflag, Dflag, Uflag;
        SolidBrush redbrush; SolidBrush bluebrush;
        SolidBrush yellowbrush; SolidBrush pinkbrush;
        SolidBrush orangebrush; SolidBrush brownbrush;
        SolidBrush greyBrush;
        public Form1()
        {
            InitializeComponent();
            toggle = true; times = 0; victory = false;
            DY = 20; DX = 20; startflag = false;
            ran = new Random();
            rw = 0; cl = 0; x = 0; y = 0; wait = 500;
            resetflag = true; direction = 1; 
            trail = new Queue<Pair<int, int>>();
            path = new Stack<String>();
            redbrush = new SolidBrush(Color.Red);
            bluebrush = new SolidBrush(Color.Blue);
            yellowbrush = new SolidBrush(Color.Yellow);
            pinkbrush = new SolidBrush(Color.Pink);
            orangebrush = new SolidBrush(Color.Orange);
            brownbrush = new SolidBrush(Color.Brown);
            greyBrush = new SolidBrush(Color.DimGray);
        }

        // When the panel in which the Grid of squares is being drawn is hit with a Paint event,
        // then we call drawGrid to redraw the maze. 
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            paper = e.Graphics;
           
           
            blackPen = new Pen(Color.Black); yellowPen = new Pen(Color.Yellow);
            if (resetflag == true)
            {
               
                grid = new Grid();
                grid.drawGrid(paper);
                currentBrush = yellowbrush; 
                                       
            }
            if (resetflag == false)
            {
                
 //               if (startflag == true)
 //               {
 //                   drawTrail(oldspot.First, oldspot.Second, greyBrush);
 //                   grid.gridRec[rw, cl].setCrumbs(false);
 //               }
              
                if (victory)
                {
                    victally++;
                    int rnd1 = ran.Next(40);
                    if (rnd1 == 20)
                    {
                        int rnd2 = ran.Next(6);
                        switch (rnd2)
                        {
                            case 0:
                                currentBrush = redbrush;
                                break;
                            case 1:
                                currentBrush = bluebrush;
                                break;
                            case 2:
                                currentBrush = yellowbrush;
                                break;
                            case 3:
                                currentBrush = pinkbrush;
                                break;
                            case 4:
                                currentBrush = orangebrush;
                                break;
                            case 5:
                                currentBrush = brownbrush;
                                break;
                        }
                    }
                    oldrect = new Rectangle(x + 3, y + 3, 14, 14);                   
                    paper.FillEllipse(currentBrush, oldrect);
                   
                } // end of if (victory)              
                
                else {
                int locx = ran.Next(15);
                int locy = ran.Next(15);
                switch (direction)
                {
                    case 0:  // up
                       
                            oldrect = new Rectangle(x+3,y+3, 14, 14);
                            newrect = new Rectangle(x+3, y-17, 14, 14);
                            paper.FillEllipse(greyBrush, oldrect);                                                    
                         
                            paper.FillEllipse(currentBrush, newrect);
                           
        //                    crumbloc = new Pair<int,int>(x+locx,y+locy);
        //                    trail.Enqueue(crumbloc);
                            grid.gridRec[rw, cl].setCrumbs(true);                           
                            drawTrail(x + locx, y + locy, currentBrush);
                            drawWall(x, y-20, blackPen); drawWall(x, y, blackPen);
                            rw--; y = y - 20;
                            break;
                    case 1:  // down
                            oldrect = new Rectangle(x+3, y+3, 14, 14);
                            newrect = new Rectangle(x+3, y + 23, 14, 14);
                            paper.FillEllipse(greyBrush, oldrect);                      
                            
                            paper.FillEllipse(currentBrush, newrect);
                           
      //                      crumbloc = new Pair<int,int>(x+locx,y+locy);
      //                      trail.Enqueue(crumbloc);
                            grid.gridRec[rw, cl].setCrumbs(true);                           
                            drawTrail(x+locx, y+locy, currentBrush);
                            drawWall(x, y + 20, blackPen); drawWall(x, y, blackPen);
                            rw++; y = y + 20;
                            break;
                    case 2:  // left
                            oldrect = new Rectangle(x+3, y+3, 14, 14);
                            newrect = new Rectangle(x-17, y+3, 14, 14);
                            paper.FillEllipse(greyBrush, oldrect);                
                          
                            paper.FillEllipse(currentBrush, newrect);
                          
  //                          crumbloc = new Pair<int,int>(x+locx,y+locy);
  //                          trail.Enqueue(crumbloc);
                            grid.gridRec[rw, cl].setCrumbs(true);                         
                         
                            drawTrail(x + locx, y + locy, currentBrush);   
                            drawWall(x-20, y, blackPen); drawWall(x, y, blackPen);
                            cl--; x = x - 20;
                            break;
                    case 3:  // right
                            oldrect = new Rectangle(x+3, y+3, 14, 14);
                            newrect = new Rectangle(x+23, y+3, 14, 14);
                            paper.FillEllipse(greyBrush, oldrect);                          
                           
                            paper.FillEllipse(currentBrush, newrect);
                           
    //                        crumbloc = new Pair<int,int>(x+locx,y+locy);
    //                        trail.Enqueue(crumbloc);
                            grid.gridRec[rw, cl].setCrumbs(true);                
                            drawTrail(x + locx, y + locy, currentBrush ); 
                            drawWall(x+20, y, blackPen); drawWall(x, y, blackPen); 
                             cl++; x = x + 20;
                            break;
                }  // end of switch


                if ((rw == 24) & (cl == 24)) { victory = true; victally = 0; } 
                } // if (victory) 
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
            if (!victory)
            {
                delaycount++;
                if (delaycount > delay) { startflag = true; delaycount = 0; }

                bool goflag = false; resetflag = false;
                if (ran.Next(3) == 1) direction = ran.Next(4);
                Uflag = false; Dflag = false; Lflag = false; Rflag = false;
                int docount = 0; int swcount = 0;
                currentBrush = yellowbrush;
                do
                {

                    switch (direction)
                    {
                        case 0:  // up
                            if (!Uflag) swcount++;
                            Uflag = true;
                            if ((y > 0) & (grid.gridRec[rw, cl].getUp()))
                            {

                                if (!grid.gridRec[rw - 1, cl].getTrace())
                                {
                                    cr = new Rectangle(x, y - 20, 20, 40);
                                    goflag = true;
                                    grid.gridRec[rw - 1, cl].makeFree();
                                    grid.gridRec[rw - 1, cl].setTrace();
                                    path.Push("Up");
                                }

                            }
                            break;
                        case 1:  // down
                            if (!Dflag) swcount++;
                            Dflag = true;
                            if ((y < 480) & (grid.gridRec[rw, cl].getDown()))
                            {

                                if (!grid.gridRec[rw + 1, cl].getTrace())
                                {
                                    cr = new Rectangle(x, y, 20, 40);
                                    goflag = true;
                                    grid.gridRec[rw + 1, cl].makeFree();
                                    path.Push("Down");
                                    grid.gridRec[rw + 1, cl].setTrace();

                                }


                            }
                            break;
                        case 2:  // left
                            if (!Lflag) swcount++;
                            Lflag = true;
                            if ((x > 0) & (grid.gridRec[rw, cl].getLeft()))
                            {

                                if (!grid.gridRec[rw, cl - 1].getTrace())
                                {
                                    cr = new Rectangle(x - 20, y, 40, 20);
                                    goflag = true;
                                    grid.gridRec[rw, cl - 1].makeFree();
                                    path.Push("Left");
                                    grid.gridRec[rw, cl - 1].setTrace();

                                }


                            }
                            break;
                        case 3:  // right
                            if (!Rflag) swcount++;
                            Rflag = true;
                            if ((x < 480) & (grid.gridRec[rw, cl].getRight()))
                            {

                                if (!grid.gridRec[rw, cl + 1].getTrace())
                                {
                                    cr = new Rectangle(x, y, 40, 20);

                                    goflag = true;
                                    grid.gridRec[rw, cl + 1].makeFree();
                                    path.Push("Right");
                                    grid.gridRec[rw, cl + 1].setTrace();

                                }


                            }
                            break;

                    }  // end of switch statement 
                    if (goflag == false) direction = ran.Next(4);
                    if ((goflag == false) & (swcount > 3))
                    {
                        if (path.Count() > 0)
                        {
                            String d = path.Pop();
                            if (d == "Up") { direction = 1; cr = new Rectangle(x, y, 20, 40); }
                            if (d == "Down") { direction = 0; cr = new Rectangle(x, y - 20, 20, 40); }
                            if (d == "Left") { direction = 3; cr = new Rectangle(x, y, 40, 20); }
                            if (d == "Right") { direction = 2; cr = new Rectangle(x - 20, y, 40, 20); }
                            goflag = true;
                            currentBrush = pinkbrush;
                        }
                    }
                    docount++;
                } while (goflag == false);
            } // end if not victory 
                Graphics g = panel1.CreateGraphics();
//                if (startflag == true)
//                {
//                    oldspot = trail.Dequeue();
//                    spotrect = new Rectangle(oldspot.First, oldspot.Second, 2, 2);
//                    panel1.Invalidate(spotrect);
//                }

                if (victory) cr = new Rectangle(460, 460, 40, 40);
                panel1.Invalidate(cr);
   //             panel1.Invalidate(cr1);
               
                panel1.Update();
            
            
        }

        private void Faster_Click(object sender, EventArgs e)
        {
            wait = wait - 50; if (wait < 100) wait = 100;
            this.timer1.Interval = wait;

        }

        private void Slower_Click(object sender, EventArgs e)
        {
            wait = wait + 50; if (wait > 5000) wait = 50000;
            this.timer1.Interval = wait;

        } // end of timer definition
                
    }
}

 
      