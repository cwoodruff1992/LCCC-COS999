using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
// 
// May, 2012, J. Loftus, for Luzerne County Community College.  
// Code to test and work with recursive backtracking for random
// maze generated.  Part of a weekly workshop offered during the Summer 2012 entitled "Applied Data Structures
// and Algorithms for Interactive Simulation systems".  
//
// The class Grid draws and maintains a two dimensional array of GRectangles, that represents the game
// grid, on which all of the action presumably will take place. 
namespace Test1
{

    class Grid
    {
        public Random rand;
        public GRectangle[,] gridRec;   // gridRec is the two dimensional grid
        private SolidBrush brushgrey;
        private SolidBrush brushwhite;
        private Pen blackPen;
        private int x, y, width, height;
        public int callcount = 0;
        //
        // The Grid constructor will populate the 10 by 10 array of GRectangles, which we call
        // gridRec, and then will go through, and in a second pass, for each rectangle, point
        // to it's neighbors via a List of GRectangles. 
        //
        public Grid()
        {
            gridRec = new GRectangle[10, 10];   // Must create an instance of gridRec
            brushgrey = new SolidBrush(Color.DimGray);
            brushwhite = new SolidBrush(Color.White);
            blackPen = new Pen(Color.Black, 1);
            //
            // Here we initialize the grid, which consists of an array of GRectangles - just 
            // C# Rectangles with some extra properties and methods.  
            // Initially, wall is set to true (all squares are walls), then the Search backtracking
            // methed will produce the 'free' squares, by setting wall to false. 
            //
            x = 0; y = 0; width = 45; height = 35;
            for (int i = gridRec.GetLowerBound(0); i <= gridRec.GetUpperBound(0); i++)
            {
                x = 0;
                for (int j = gridRec.GetLowerBound(1); j <= gridRec.GetUpperBound(1); j++)
                {
                    gridRec[i, j] = new GRectangle(x, y, width, height, true, i, j);
                    x += 45;
                }
                y += 35;
            }  // end of for loop
            
            //
            // Take second pass through the array of GRectangles, to setup the neighbors
            // A neighbor is merely an adjacent square.
            x = 0; y = 0; width = 45; height = 35;
            for (int i = gridRec.GetLowerBound(0); i <= gridRec.GetUpperBound(0); i++)
            {
                x = 0;
                for (int j = gridRec.GetLowerBound(1); j <= gridRec.GetUpperBound(1); j++)
                {
                    // Up, Down, Left, Right in order for List
                    if (i > 0) { gridRec[i,j].setUp(gridRec[i-1, j]); }
                     
                    if (i < 9) { gridRec[i,j].setDown(gridRec[i+1,j]); }
                    if (j > 0) { gridRec[i,j].setLeft(gridRec[i, j - 1]); }
                    if (j < 9) { gridRec[i,j].setRight(gridRec[i, j + 1]); }
                    
                    x += 45;
                }
                y += 35;
            } //end of second for loop
            //
            //
            // Begin the recursive search for the maze, starting at upper left corner square, which is
            // position 0,0
            rand = new Random();  // Seed a random number generator.  
            int Counter = 0;
            Search(gridRec[0, 0], ref Counter);  // Still in the constructor, we fire off the recursion with this first
                                                 // call to Search (begin at upper left corner of the grid).
        } // end of grid constructor
        //
        //
        // The method Search will do depth-first search to create the maze. 
        //
        public void Search(GRectangle r, ref int callcounter)
        {
            //
            //
            // If the curreent square is not available, we return.  But if it's
            // available, we make it free (change wall to false), and then search
            // through it's neighbors, calling Search again for each of them.  
            if (r.isAvailable() == false) // Is it available?               
                return;
            r.makeFree();  // Make it free (no longer a wall).
            //
            // The following line is temporary: just for debugging.  callcounter will be printed out
            // in the drawGrid method: it tracks the order in which the squares were 'dug out'. 
            callcounter++; r.setCounter(callcounter);  // We house the callcounter in each square, so we can access it later.  
            //
            // We randomly choose which neighbor of the current square to look at, to get
            // a more robust maze. testrec is just a copy of the neighbors.  Each time we select a neighbor
            // we remove it from testrec, so we can then get a random integer within the new, smaller range on
            // the next iteration
            List<GRectangle> testrec = r.getNeighbors();  // Get a copy of the list of neighbors for r. 
            do
            {
                int testidx = rand.Next(testrec.Count);
                if (testrec[testidx].isWall() == true)  // Only walls need to considered to 'burrow through'. 
                {
                    Search(testrec[testidx], ref callcounter);  // Here we call Search for each of the remainding neighbors. 
                }
                testrec.RemoveAt(testidx); // As this one was considered, remove it (so now we have a smaller list).
            } while (testrec.Count > 0); // end of do while
 
            return;
        } //end of Search

        // drawGrid simply draws the Grid of GRectangles, by casting them back to Rectangles, and 
        // then calling the C# supplied FillRectangle method. 
        //
        public void drawGrid(Graphics paper)
        {
            foreach (GRectangle rec in gridRec)
            {
                Rectangle rec1 = rec.toRectangle();  // rec1 is the Rectangle corresponding to rec.  
                if (rec.isWall() == false)  // If it's free, paint it as white, and place the counter in the square.
                {
                   
                        paper.FillRectangle(brushwhite, rec1);
                        Font font1 = new Font("Arial", 8, FontStyle.Bold, GraphicsUnit.Point);

                        paper.DrawString(rec.getCounter().ToString(), font1, Brushes.Blue, rec1);  // getCounter returns
                        // the order in which this square was 'dug out' in the above Search routine.  
                }
                else
                {
                 
                      paper.FillRectangle(brushgrey, rec1); // If it's not free, paint it out as grey.  
                   
                }
            } // end of foreach
        }  // end of drawGrid 

    } // end of class grid definition

} 
