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
// The GRectangle class wraps around the C#- provided Rectangle class, and includes some extra fields
// and methods needed for a potential game.  It represents one 'square' on a 10 by 10 game grid.  

namespace Test1
{
    class GRectangle 
    {
        private bool wall;  // wall is a boolean that flags whether the current square is a wall or has been 'dug out'
                            // recursively.  Initially, every square is a wall, then as we proceed we set some of them 
                            // to wall = false. 
        private Rectangle thisRec;
        private List<GRectangle> Neighbors;  // This is a List of up to 4 neighbors for each square in the grid
        private int ivalue;
        private int jvalue;
        private int counter;
        //
        // This is the constructor for a GRectangle object.  Must be called with the "new" keyword.
        public GRectangle(int X, int Y, int Width, int Length, bool Wall, int I, int J)
        {
            counter = 0;
            // The boolean variable wall indicates whether this current GRectangle is a wall (can't pass through) 
            // or not. 
            wall = Wall;
            
            // thisRec is just the C# Rectangle corresponding to this GRectangle. 
            thisRec = new Rectangle(X, Y, Width, Length);

            // Neighbors are the (up to 4) squares that are adjacent to the current one. 
            Neighbors = new List<GRectangle>();
            ivalue = I;
            jvalue = J;
        }
        public GRectangle() { wall = true; }
        //
        // Return the list of neighbors for a GRectangle object
        public List<GRectangle> getNeighbors()
        {
            return Neighbors;
        }
        //
        // The next four methods merely add neighbors to the Neighbors list, as needed.
        public void setUp(GRectangle u) 
        {
        
            Neighbors.Add(u);
        
        }
        public void setDown(GRectangle u)
        {
          
             Neighbors.Add(u);
        }
        public void setLeft(GRectangle u)
        {
            Neighbors.Add(u);
        }
        public void setRight(GRectangle u)
        {
            Neighbors.Add(u);
        }
        //
        // At times we have to cast is back to Rectangle, to use various C# supplied drawing routines, etc.
        public Rectangle toRectangle()
        {
            return thisRec;
        }
        //
        // isWall tells us if this GRectangle is a wall or not. 
        public bool isWall()
        {
            return wall;
        }
        public void makeFree()
        {
            wall = false;
        }
        //
        // getCount will return the number of neighbors this square has (a number from 2 to 4 inclusively)
        public int getCount()
        {
            return Neighbors.Count;
        }
        //
        // getCounter is will just house the order in which the squares are drawn to the screen.
        // 
        public int getCounter()
        {
            return counter;
        }
        public void setCounter(int callcounter)
        {
            counter = callcounter;
        }
        //
        // is Available is used by the recursive search routine to see if the square currently being
        // considered can be made 'free', i.e., not a wall.  We simply loop through its neighbors, 
        // and count how many are already free.  If this number is two or more, it can't be used.
        //
        public bool isAvailable()
        {
            int count = 0;
            foreach (GRectangle n in Neighbors)
            {
                if (!n.isWall()) count++; 
            }
            if (count < 2) return true;
            return false;
        }
    }

}

