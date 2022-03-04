using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Double_Mps_Thickness
{
    static class MPS_OCR_SMProcess
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MPS_OCR_SM());
        }
    }
}
