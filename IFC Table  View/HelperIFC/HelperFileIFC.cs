using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFC_Table_View.HelperIFC
{
    internal class HelperFileIFC
    {
        static string path;
        public static string OpenIFC_File()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "IFC Files|*.ifc*;.ifcxml"; ;
                DialogResult dialog = openFileDialog.ShowDialog();
                if (dialog == DialogResult.OK)
                { 
                    path = openFileDialog.FileName;
                    
                }
                else
                {
                    path = null;
                }
            }
            return path;
        }

        public static string SaveAsIFC_File(string format)
        {
            if (format != "ifc" && format != "ifcxml")
            { return null; }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = $"IFC Files|*.{format}";
                DialogResult dialog = saveFileDialog.ShowDialog();
                if (dialog == DialogResult.OK)
                {
                    path = saveFileDialog.FileName;

                }
                else
                {
                    path = null;
                }
            }
            return path;
        }


    }
}
