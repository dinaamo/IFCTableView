using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edit_PropertyIFC4
{
    static class FileIFC
    {
        static string path;
        public static string OpenIFC_File()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "IFC Files|*.ifc"; ;
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

        public static string SaveAsIFC_File()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "IFC Files|*.ifc";
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
        public static string SaveAsIFCXML_File()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "IFCXML Files|*.ifcxml";
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
