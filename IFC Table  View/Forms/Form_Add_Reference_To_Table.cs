using GeometryGym.Ifc;
using IFC_Table_View.IFC.ModelItem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace IFC_Table_View.Data
{
    public partial class Form_Add_Reference_To_Table : Form
    {
        private List<BaseModelReferenceIFC> CollectionModelitemTable;
        private List<ModelItemIFCObject> CollectionModelItemObject;
        public List<BaseModelReferenceIFC> CollectionTableToAdd;

        public Form_Add_Reference_To_Table(List<ModelItemIFCObject> CollectionModelItemObject, List<BaseModelReferenceIFC> CollectionModelitemTable)
        {
            InitializeComponent();
            CollectionTableToAdd = new List<BaseModelReferenceIFC>();
            this.CollectionModelitemTable = CollectionModelitemTable;
            this.CollectionModelItemObject = CollectionModelItemObject;

            CollectionTableToAdd = new List<BaseModelReferenceIFC>();
        }
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private void Form1_Load(object sender, EventArgs e)
        {
            FillDataGrid();
        }


        //Заполнение датагрид
        private void FillDataGrid()
        {
            dataGridViewTable.AutoGenerateColumns = false;
            dataGridViewObjects.AutoGenerateColumns = false;

            dataGridViewTable.DataSource = new BindingList<BaseModelReferenceIFC>(CollectionModelitemTable);
            dataGridViewTable.Columns[0].DataPropertyName = "NameReference";

            dataGridViewObjects.DataSource = new BindingList<ModelItemIFCObject>(CollectionModelItemObject);
            dataGridViewObjects.Columns[0].DataPropertyName = "IFCObjectGUID";
            dataGridViewObjects.Columns[1].DataPropertyName = "IFCObjectName";
            dataGridViewObjects.Columns[2].DataPropertyName = "IFCObjectClass";

        }


        private void button_Add_Reference_Click(object sender, EventArgs e)
        {
            GetNameSelectTable();
            this.Close();
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
                    CollectionTableToAdd.Add(((BindingList<BaseModelReferenceIFC>)dataGridViewTable.DataSource)[row.Index]);
                }
            }
        }

    }
}