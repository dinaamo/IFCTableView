using GeometryGym.Ifc;
using IFC_Table_View.IFC.ModelItem;
using IFC_Table_View.Infracrucrure.Commands.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace IFC_Table_View.Data
{
    public partial class Form_Delete_Reference_To_Table : Form
    {
        
        private List<ModelItemIFCObject> CollectionModelItemObject;
        private List<BaseModelReferenceIFC> CollectionModelitemTable;

        public List<BaseModelReferenceIFC> CollectionModelitemTableToDelete;


        public Form_Delete_Reference_To_Table(List<ModelItemIFCObject> CollectionModelItemObject, List<BaseModelReferenceIFC> CollectionModelitemTable)
        {
            InitializeComponent();
            
            this.CollectionModelItemObject = CollectionModelItemObject;   
            this.CollectionModelitemTable = CollectionModelitemTable;

            CollectionModelitemTableToDelete = new List<BaseModelReferenceIFC>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            FilterTable();

            FillDataGrid();
        }

        //Фильтрация таблиц
        private void FilterTable()
        {
            List<IfcObjectReferenceSelect> ifcObjectReferenceSelect = new List<IfcObjectReferenceSelect>();
            foreach (ModelItemIFCObject modelItemObject in CollectionModelItemObject)
            {
                GeometryGym.STEP.SET<IfcRelDefinesByProperties> RelDefCollection = null;

                //Получаем объект
                if (modelItemObject.GetIFCObject() is IfcObject ifcObject)
                {
                    RelDefCollection = ifcObject.IsDefinedBy;
                }
                else if (modelItemObject.GetIFCObject() is IfcContext ifcContext)
                {
                    RelDefCollection = ifcContext.IsDefinedBy;
                }

                //Получаем все ссылочные элементы на которые есть ссылки у элемента
                ifcObjectReferenceSelect.AddRange(RelDefCollection?.SelectMany(it => it.RelatingPropertyDefinition)
                                                                        .OfType<IfcPropertySet>()
                                                                        .SelectMany(PropSet => PropSet.HasProperties)
                                                                        .Select(dict => dict.Value)
                                                                        .OfType<IfcPropertyReferenceValue>()
                                                                        .Select(it => it.PropertyReference).ToList());
            }

            //Оставляем в коллекции только ссылочные элементы ссылки на которые есть в элементе
            CollectionModelitemTable = CollectionModelitemTable.Where(it => ifcObjectReferenceSelect.Any(oRef => it.GetReferense() == oRef)).ToList();
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

        private void button_Delete_Reference_Click(object sender, EventArgs e)
        {
            GetNameSelectTable();
            this.Hide();
        }

        //Получаем имена ссылок которые выбрал пользователь
        private void GetNameSelectTable()
        {
            DataGridViewRowCollection selRows = dataGridViewTable.Rows;

            foreach (DataGridViewRow row in selRows)
            {
                object stateCell = row.Cells[1].Value;
                if (stateCell != null && (bool)stateCell)
                {
                    CollectionModelitemTableToDelete.Add(((BindingList<BaseModelReferenceIFC>)dataGridViewTable.DataSource)[row.Index]);
                }
            }
        }
    }
}