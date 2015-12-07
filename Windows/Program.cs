#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace ToastBuddyLibExample.Windows
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
			using (var game = new Game1())
			{
				game.Run();
			}
        }
    }
}
