using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
// This is a virtual class, holding a pair of objects of a specific type.  
// It is used in the Grid class to hold a GRectangle, integer pair.  
namespace Test1
{
    public class Pair<T, U> 
    { 
    public Pair() { } 
    public Pair(T first, U second) 
    {   
        this.First = first; 
        this.Second = second; 
    } 
    public T First { get; set; } 
    public U Second { get; set; } }; 
    
}
