using CoreApp.Dicts;
using CoreApp.FixpackObjects;
using CoreApp.InfaObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreApp.FormUtils
{
    class FormUtil
    {
        public static void InitObjDGV(DataGridView dictDGV)
        {
            dictDGV.Rows.Clear();
            dictDGV.Columns.Clear();
            dictDGV.Columns.Add("Object", "Объект");
            dictDGV.Columns.Add("Type", "Тип/команда");
            dictDGV.Columns.Add("FileName", "Файл");

        }

        public static void InitNotFoundFilesDGV(DataGridView dictDGV)
        {
            dictDGV.Rows.Clear();
            dictDGV.Columns.Clear();
            dictDGV.Columns.Add("NotFoundFiles", "Ненайденные файлы");
        }

        public static void AddNotFoundFiles(DataGridView dictDGV, OraObjectDict dict)
        {
            foreach(FileInfo file in dict.notFoundFiles)
            {
                dictDGV.Rows.Add(file.FullName);
            }
            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public static void AddNotFoundFiles(DataGridView dictDGV, InfaObjectDict dict)
        {
            foreach(FileInfo file in dict.notFoundFiles)
            {
                dictDGV.Rows.Add(file.FullName);
            }
            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public static void AddOraObjectsInDGV(DataGridView dictDGV, OraObjectDict dict)
        {
            foreach(KeyValuePair<OraObject, HashSet<Patch>> item in dict.baseDict.objFilesPairs)
            {
                OraObject oraObj = item.Key;
                foreach(Patch patch in item.Value)
                {
                    dictDGV.Rows.Add(oraObj.objName, oraObj.type, patch.pathToPatch);
                }
            }
            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public static void AddOraIntersectionsInDGV(DataGridView dictDGV, OraObjectDict dict)
        {
            bool colorDeterminator = false;
            foreach (KeyValuePair<OraObject, HashSet<Patch>> item in dict.intersections.objFilesPairs)
            {

                OraObject oraObj = item.Key;
                foreach (Patch patch in item.Value)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                    row.Cells[0].Value = oraObj.objName;
                    row.Cells[1].Value = oraObj.type;
                    row.Cells[2].Value = patch.pathToPatch;
                    row.DefaultCellStyle.BackColor = colorDeterminator ? Color.LightCyan : Color.LightYellow;
                    dictDGV.Rows.Add(row);                    
                }
                colorDeterminator = !colorDeterminator;
            }
            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public static void AddInfaIntersectionsInDGV(DataGridView dictDGV, InfaObjectDict dict)
        {
            bool colorDeterminator = false;
            foreach (KeyValuePair<InfaBaseObject, HashSet<Patch>> item in dict.intersections.objFilesPairs)
            {
                InfaBaseObject infaObj = item.Key;
                foreach (Patch patch in item.Value)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                    row.Cells[0].Value = infaObj.objName;
                    row.Cells[1].Value = infaObj.GetType().Name;
                    row.Cells[2].Value = patch.pathToPatch;
                    row.DefaultCellStyle.BackColor = colorDeterminator ? Color.LightCyan : Color.LightYellow;
                    dictDGV.Rows.Add(row);
                }
                colorDeterminator = !colorDeterminator;
            }
            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public static void AddInfaNotFoundObjectsInDGV(DataGridView dictDGV, InfaObjectDict dict)
        {
            foreach (KeyValuePair<InfaBaseObject, HashSet<InfaBaseObject>> item in dict.notFoundObject)
            {
                InfaBaseObject infaObj = item.Key;
                DataGridViewRow row1 = new DataGridViewRow();
                row1.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                row1.Cells[0].Value = infaObj.objName;
                row1.Cells[1].Value = infaObj.GetType().Name;
                row1.Cells[2].Value = infaObj.file.FullName;
                row1.DefaultCellStyle.BackColor = Color.Cyan;
                dictDGV.Rows.Add(row1);

                foreach (InfaBaseObject notFoundObj in item.Value)
                {
                    DataGridViewRow row2 = new DataGridViewRow();
                    row2.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                    row2.Cells[0].Value = notFoundObj.objName;
                    row2.Cells[1].Value = notFoundObj.GetType().Name;
                    row2.DefaultCellStyle.BackColor = Color.LightYellow;
                    dictDGV.Rows.Add(row2);
                }

            }
            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public static void AddInfaWrongOrderInDGV(DataGridView dictDGV, InfaObjectDict dict)
        {
            bool colorDeterminator = false;
            foreach (KeyValuePair<InfaBaseObject, InfaBaseObject> item in dict.infaDependencies)
            {
                InfaBaseObject infaObj1 = item.Key;
                DataGridViewRow row1 = new DataGridViewRow();
                row1.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                row1.Cells[0].Value = infaObj1.objName;
                row1.Cells[1].Value = infaObj1.GetType().Name;
                row1.Cells[2].Value = infaObj1.file.FullName;
                row1.DefaultCellStyle.BackColor = colorDeterminator ? Color.LightCyan : Color.LightYellow;
                dictDGV.Rows.Add(row1);

                InfaBaseObject infaObj2 = item.Value;
                DataGridViewRow row2 = new DataGridViewRow();
                row2.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                row2.Cells[0].Value = infaObj2.objName;
                row2.Cells[1].Value = infaObj2.GetType().Name;
                row2.Cells[2].Value = infaObj2.file.FullName;
                row2.DefaultCellStyle.BackColor = colorDeterminator ? Color.LightCyan : Color.LightYellow;
                dictDGV.Rows.Add(row2);

                colorDeterminator = !colorDeterminator;
            }
            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public static void AddInfaObjectsInDGV(DataGridView dictDGV, InfaObjectDict dict)
        {
            foreach (KeyValuePair<InfaBaseObject, HashSet<Patch>> item in dict.baseDict.objFilesPairs)
            {
                InfaBaseObject infaObj = item.Key;
                foreach (Patch patch in item.Value)
                {
                    dictDGV.Rows.Add(infaObj.objName, infaObj.GetType().Name, patch.pathToPatch);
                }
            }
            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }


    }
}
