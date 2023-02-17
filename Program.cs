// License info and recommendations
//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Corporate Initiatives Group Asia Pty Ltd">
//     http://www.thecigroup.com.au
//     All source code excluding third party packages remains the sole property of Corporate Initiatives Group Asia Pty Ltd.
//     Source code may not be implemented, extended, modified, copied, re-distributed or deployed
//     without the express written consent of an authorised employee of Corporate Initiatives Group Asia Pty Ltd.
//     For more details please refer to the LICENSE file located in the root folder of the project source code.
//     20230215 Rod Driscoll
//     e: rodney.driscoll@thecigroup.com.au
//     m: +61 2 9223 3955
//     {c} Licensed to orporate Initiatives Group Asia Pty Ltd 2023.
// </copyright>
//-----------------------------------------------------------------------
namespace CI.CSharp.CrestronLogParser
{
    using System;
    using System.Windows.Forms;
    
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
