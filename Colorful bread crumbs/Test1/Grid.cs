using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
//
// May, 2012, J. Loftus, for Luzerne County Community College. 
//Code to demonstrate how to use a Dequeue (FIFO queue) to draw a trace (the bread crumbs) 
// behind the game element that will disappear in the same order in which it was drawn.  
//  Part of a weekly workshop offered during the Summer 2012 entitled "Applied Data Structures
// and Algorithms for Interactive Simulation systems".  
//
// The class Grid draws and maintains a two dimensional array of GRectangles, that represents the game
// grid, on which all of the action presumably will take place. 
namespace Test1
{

    class Grid
    {
        public Random rand;
        public GRectangle[,] gridRec;
        Pair<GRectangle, int>[] S;
        private SolidBrush brushgrey;
        private SolidBrush brushwhite;
        private Pen blackPen;
        private Pen greyPen;
        private int x, y, width, height;
        public int callcount = 0;
        public const int dimx = 25; 
        public const int dimy = 25;
        //
        // The Grid constructor will populate the 10 by 10 array of GRectangles, which we call
        // gridRec, and then will go through, and in a second pass, for each rectangle, point
        // to it's neighbors via a List of GRectangles. 
        //
        public Grid()
        {
            gridRec = new GRectangle[25, 25]; 
            S = new Pair<GRectangle, int>[dimx*dimy];
            brushgrey = new SolidBrush(Color.DimGray);
            brushwhite = new SolidBrush(Color.White);
            blackPen = new Pen(Color.Black, 1);
            greyPen = new Pen(Color.DimGray);
            //
            // Here we initialize the grid, which consists of an array of GRectangles - just 
            // C# Rectangles with some extra properties and methods.  
            // Initially, wall is set to true (all squares are walls), then the Search backtracking
            // methed will produce the 'free' squares, by setting wall to false. 
            //
            x = 0; y = 0; width = 500/dimx; height = 500/dimy;
            for (int i = gridRec.GetLowerBound(0); i <= gridRec.GetUpperBound(0); i++)
            {
                x = 0;
                for (int j = gridRec.GetLowerBound(1); j <= gridRec.GetUpperBound(1); j++)
                {
                    gridRec[i, j] = new GRectangle(x, y, width, height, true, i, j);

                    x += width;
                }
                y += height;
            }  // end of for loop

            //
            // Take second pass through the array of GRectangles, to setup the neighbors
            //
            x = 0; y = 0; 
            for (int i = gridRec.GetLowerBound(0); i <= gridRec.GetUpperBound(0); i++)
            {
                x = 0;
                for (int j = gridRec.GetLowerBound(1); j <= gridRec.GetUpperBound(1); j++)
                {
                 
                    S[dimx * i + j] = new Pair<GRectangle, int>();
                    S[dimx * i + j].First = gridRec[i, j];
                    S[dimx * i + j].Second = -1;
                    x += width;
                }
                y += height;
            } //end of second for loop
            //
            
        } // end of constructor Grid()
    

        //   
        // drawGrid simply draws the Grid of GRectangles, by casting them back to Rectangles, and 
        // then calling the C# supplied FillRectangle method. 
        //
        public void drawGrid(Graphics paper)
        {
            foreach (GRectangle rec in gridRec)
            {
                Rectangle rec1 = rec.toRectangle();
               
                      paper.DrawRectangle(blackPen, rec1);  
            } // end of for each
  
            // Now we randomly go through and knock down walls until we get a minimally 
            // connected maze (Kruskal's algorithm for maze generation) 
            // First, get a random entry. 
            int nextrow=0, nextcol=0;
            int docounter = 0;
            //
            // We randomly select edges exactly 99 times.  100 vertices, and 99 edges make a tree!
            rand = new Random();   // Seed the random number generator. 
            do
            {
                int row = rand.Next(dimy);
                int col = rand.Next(dimx);
                int dir = rand.Next(2);   // dir = direction, whether vertical or horizontal

                if (dir == 0) // Find a neighbor vertically
                {
                    if (row == 0) { nextrow = row + 1; nextcol = col; }
                    else if (row == dimy-1) { nextrow = row - 1; nextcol = col; }
                    else { int ud = 2 * rand.Next(2) - 1; nextrow = row + ud; nextcol = col; }
                }
                if (dir == 1) // Find a neighbor horizontally
                {
                    if (col == 0) { nextcol = col + 1; nextrow = row; }
                    else if (col == dimx-1) { nextcol = col - 1; nextrow = row; }
                    else { int ud = 2 * rand.Next(2) - 1; nextcol = col + ud; nextrow = row; }
                }
                // 
                // Call find to see to what set each of the two rectangles belong to.  
                int first = find(dimx * row + col); int second = find(dimx * nextrow + nextcol);
                if (first != second)   // Only if the sets are not the same do we continue
                {
                    Union(first, second);  // Union the two disjoint sets.  
                    docounter++;
                    switch (dimx * (nextrow - row) + nextcol - col)     // Knock down the wall between the two rectangles. 
                    {

                        case 1:   // Same row, nextcol right of col
                            Point pt1 = new Point(nextcol*width, row * height);
                            Point pt2 = new Point(nextcol*width, row*height+height);
                            paper.DrawLine(greyPen, pt1, pt2);        // greyPen is the background color.
                            gridRec[row, col].setRight(gridRec[row, nextcol]);
                            gridRec[row, nextcol].setLeft(gridRec[row, col]); 
                            break;
                        case -1:   // Same row, col right of nextcol
                            pt1 = new Point(col*width, row*height);
                            pt2 = new Point(col*width, row*height+height);
                            paper.DrawLine(greyPen, pt1, pt2);
                            gridRec[row, nextcol].setRight(gridRec[row, col]);
                            gridRec[row, col].setLeft(gridRec[row, nextcol]); 
                            break;
                        case dimx:   // Same col, nextrow below row
                            pt1 = new Point(col*width, nextrow*height);
                            pt2 = new Point(col*width+width, nextrow*height);
                            paper.DrawLine(greyPen, pt1, pt2);
                            gridRec[row, col].setDown(gridRec[nextrow, col]);
                            gridRec[nextrow, col].setUp(gridRec[row, col]); 
                            break;
                        case -dimx:   // Same col, nextrow above row
                            pt1 = new Point(col*width, row*height);
                            pt2 = new Point(col*width+width, row*height);
                            paper.DrawLine(greyPen, pt1, pt2);
                            gridRec[nextrow, col].setDown(gridRec[row, col]);
                            gridRec[row, col].setUp(gridRec[nextrow, col]); 
                            break;
                    } // end of switch

                } // end of if first not equal to second
               
            } while (docounter < dimx*dimy-1);
        } // end of drawGrid

        //
        // Union and find methods are the standard ones for the Disjoint Union data structure.  
        // The elements of S corresponding to two squares are in the same set iff there is a path from one to
        // the other in the maze, i.e., they are 'connected'.
        // These algorithms were taken from Data Structures in C++ by Mark Allen Weiss.  
        // 
        public void Union( int root1, int root2 )
        {
            if (S[root2].Second < S[root1].Second) // root2 is deeper
            {
                S[root1].Second = root2;        // Make root2 new root
            }
            else
            {
                if (S[root1].Second == S[root2].Second) { S[root1].Second--; } // Update height if same
                S[root2].Second = root1;        // Make root1 new root
            }       
        } // end of union

        int find(int x)
        {
            if (S[x].Second < 0)
                return x;
            else
                return find(S[x].Second);
        }
    } // end of Grid class definition
    
}
