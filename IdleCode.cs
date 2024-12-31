using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Scheduling
{
    class IdleCode : Code
    {
        public IdleCode() : base()
        {
            try
                {
                    Console.WriteLine("Entering IdleCode constructor.");
                    Lines.Add("yield");
                    Lines.Add("goto 0"); // Add infinite loop
                    Console.WriteLine("IdleCode constructor completed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in IdleCode constructor: " + ex.Message);
                    throw; // חשוב לזרוק את החריגה כדי לא לאבד מידע
                }

                    
        }
    }
}
