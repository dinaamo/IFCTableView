using GeometryGym.Ifc;
using IFC_Table_View.IFC.ModelItem;
using IFC_Table_View.Infracrucrure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IFC_Table_View.Data
{
    public partial class Form_Delete_Reference_To_Table : Form
    {
        
        private ModelItemIFCObject modelItemObject;
        public Dictionary<string, IfcPropertyReferenceValue> ifcPropertyReferenceValueDictionaryToDelete;
        Dictionary<string ,IfcPropertyReferenceValue> ifcPropertyReferenceValueDictionary;

        public Form_Delete_Reference_To_Table(ModelItemIFCObject modelItemObject)
        {
            InitializeComponent();
            ifcPropertyReferenceValueDictionaryToDelete = new Dictionary<string, IfcPropertyReferenceValue>();
            
            this.modelItemObject = modelItemObject;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            labelName.Text = ((IfcObjectDefinition)modelItemObject.ItemTreeView).Name;
            labelGUID.Text = ((IfcObjectDefinition)modelItemObject.ItemTreeView).Guid.ToString();

            
            //Получаем объект
            IfcObject ifcObject = (IfcObject)modelItemObject.ItemTreeView;

            //Получаем все характеристики типа IfcPropertyReferenceValue
            ifcPropertyReferenceValueDictionary = ifcObject.IsDefinedBy.SelectMany(it => it.RelatingPropertyDefinition)
                                                                                    .OfType<IfcPropertySet>()
                                                                                    .SelectMany(PropSet => PropSet.HasProperties)
                                                                                    .Where(dict => dict.Value.GetType() == typeof(IfcPropertyReferenceValue))
                                                                                    .ToDictionary(x => x.Key, y => (IfcPropertyReferenceValue)y.Value);
                                                                                    
            foreach (KeyValuePair<string, IfcPropertyReferenceValue> ifcPropertyReferenceValue in ifcPropertyReferenceValueDictionary)
            {
                dataGridViewTable.Rows.Add([ifcPropertyReferenceValue.Key]);
            }
            
            
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
                    KeyValuePair<string, IfcPropertyReferenceValue> findPair = ifcPropertyReferenceValueDictionary.First(it => it.Key == row.Cells[0].Value.ToString());

                    ifcPropertyReferenceValueDictionaryToDelete.Add(findPair.Key, findPair.Value);
                }
            }
        }
    }
}