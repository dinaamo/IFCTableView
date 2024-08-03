using GeometryGym.Ifc;
using IFC_Table_View.IFC.ModelItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IFC_Table_View.Data
{
    public partial class Form_Add_Reference_To_Table : Form
    {
        private List<ModelItemIFCTable> CollectionModelitemTable;
        private ModelItemIFCObject modelItemObject;
        public List<ModelItemIFCTable> TableNameCollection;

        public Form_Add_Reference_To_Table(ModelItemIFCObject modelItemObject, List<ModelItemIFCTable> CollectionModelitemTable)
        {
            InitializeComponent();
            TableNameCollection = new List<ModelItemIFCTable>();
            this.CollectionModelitemTable = CollectionModelitemTable;
            this.modelItemObject = modelItemObject;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            labelName.Text = ((IfcObjectDefinition)modelItemObject.ItemTreeView).Name;
            labelGUID.Text = ((IfcObjectDefinition)modelItemObject.ItemTreeView).Guid.ToString();

            foreach (ModelItemIFCTable modelItem in CollectionModelitemTable)
            {
                dataGridViewTable.Rows.Add(new object[] { modelItem.IFCTable.Name });
            }
        }

        private void button_Add_Reference_Click(object sender, EventArgs e)
        {
            GetNameSelectTable();
            this.Hide();
        }

        //Получаем имена таблиц которые выбрал пользователь
        private void GetNameSelectTable()
        {
            DataGridViewRowCollection selRows = dataGridViewTable.Rows;

            foreach (DataGridViewRow row in selRows)
            {
                object stateCell = row.Cells[1].Value;
                if (stateCell != null && (bool)stateCell)
                {
                    TableNameCollection.Add(CollectionModelitemTable.First(it => it.IFCTable.Name == row.Cells[0].Value.ToString()));
                }
            }
        }
    }
}