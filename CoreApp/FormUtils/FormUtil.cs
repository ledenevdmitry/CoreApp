using CoreApp.Dicts;
using CoreApp.FixpackObjects;
using CoreApp.InfaObjects;
using CoreApp.Keys;
using CoreApp.Parsers;
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
            dictDGV.Columns.Add("PatchName", "Патч");

        }

        public static void InitNotFoundFilesDGV(DataGridView dictDGV)
        {
            dictDGV.Rows.Clear();
            dictDGV.Columns.Clear();
            dictDGV.Columns.Add("NotFoundFiles", "Ненайденные файлы");
        }

        public static void AddNotFoundFiles(DataGridView dictDGV, ETLParser parser)
        {
            foreach(FileInfo file in parser.oraObjectDict.notFoundFiles)
            {
                dictDGV.Rows.Add(file.FullName);
            }

            foreach(FileInfo file in parser.infaObjectDict.notFoundFiles)
            {
                dictDGV.Rows.Add(file.FullName);
            }
            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public static void AddObjectsInDGV(DataGridView dictDGV, ETLParser parser)
        {
            foreach(KeyValuePair<ETLObject, ZPatch> item in parser.oraObjectDict.baseDict.EnumerateObjPatchPairs())
            {
                ETLObject oraObj = item.Key;
                ZPatch patch = item.Value;
                dictDGV.Rows.Add(oraObj.objName, oraObj.objType, patch.name);
            }

            foreach (KeyValuePair<ETLObject, ZPatch> item in parser.infaObjectDict.baseDict.EnumerateObjPatchPairs())
            {
                ETLObject infaObj = item.Key;
                ZPatch patch = item.Value;
                dictDGV.Rows.Add(infaObj.objName, infaObj.objType, patch.name);
            }

            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public static void AddIntersectionsInDGV(DataGridView dictDGV, ETLParser parser)
        {
            bool colorDeterminator = false;
            foreach (var pairs in  parser.oraObjectDict.intersections.EnumerateByDistinctKeys())
            {
                foreach (var pair in pairs)
                {
                    ETLObject oraObj = pair.Key;
                    ZPatch patch = pair.Value;
                    DataGridViewRow row = new DataGridViewRow();
                    row.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                    row.Cells[0].Value = oraObj.objName;
                    row.Cells[1].Value = oraObj.objType;
                    row.Cells[2].Value = patch.name;
                    row.DefaultCellStyle.BackColor = colorDeterminator ? Color.LightCyan : Color.LightYellow;
                    dictDGV.Rows.Add(row);                    
                }
                colorDeterminator = !colorDeterminator;
            }

            foreach (var pairs in parser.infaObjectDict.intersections.EnumerateByDistinctKeys())
            {
                foreach (var pair in pairs)
                {
                    ETLObject infaObj = pair.Key;
                    ZPatch patch = pair.Value;
                    DataGridViewRow row = new DataGridViewRow();
                    row.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                    row.Cells[0].Value = infaObj.objName;
                    row.Cells[1].Value = infaObj.GetType().Name;
                    row.Cells[2].Value = patch.name;
                    row.DefaultCellStyle.BackColor = colorDeterminator ? Color.LightCyan : Color.LightYellow;
                    dictDGV.Rows.Add(row);
                }
                colorDeterminator = !colorDeterminator;
            }
            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
        
        public static void AddAllDependenciesInDGV(DataGridView dictDGV, ETLParser parser)
        {
            bool colorDeterminator = false;
            foreach (KeyValuePair<ETLObject, ETLObject> item in parser.infaObjectDict.infaDependencies.EnumeratePairs())
            {
                ETLObject infaObj1 = item.Key;
                DataGridViewRow row1 = new DataGridViewRow();
                row1.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                row1.Cells[0].Value = infaObj1.objName;
                row1.Cells[1].Value = infaObj1.GetType().Name;
                row1.Cells[2].Value = infaObj1.patch.name;
                row1.DefaultCellStyle.BackColor = colorDeterminator ? Color.LightCyan : Color.LightYellow;
                dictDGV.Rows.Add(row1);

                ETLObject infaObj2 = item.Value;
                DataGridViewRow row2 = new DataGridViewRow();
                row2.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                row2.Cells[0].Value = infaObj2.objName;
                row2.Cells[1].Value = infaObj2.GetType().Name;
                row2.Cells[2].Value = infaObj2.patch.name;
                row2.DefaultCellStyle.BackColor = colorDeterminator ? Color.LightCyan : Color.LightYellow;
                dictDGV.Rows.Add(row2);

                colorDeterminator = !colorDeterminator;
            }
            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public static void AddLostDependenciesInDGV(DataGridView dictDGV, ETLParser parser)
        {
            bool colorDeterminator = false;
            foreach (KeyValuePair<ETLObject, ETLObject> item in parser.infaObjectDict.infaLostDependencies.EnumeratePairs())
            {
                ETLObject infaObj1 = item.Key;
                DataGridViewRow row1 = new DataGridViewRow();
                row1.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                row1.Cells[0].Value = infaObj1.objName;
                row1.Cells[1].Value = infaObj1.GetType().Name;
                row1.Cells[2].Value = infaObj1.patch.name;
                row1.DefaultCellStyle.BackColor = colorDeterminator ? Color.LightCyan : Color.LightYellow;
                dictDGV.Rows.Add(row1);

                ETLObject infaObj2 = item.Value;
                DataGridViewRow row2 = new DataGridViewRow();
                row2.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                row2.Cells[0].Value = infaObj2.objName;
                row2.Cells[1].Value = infaObj2.GetType().Name;
                row2.Cells[2].Value = infaObj2.patch.name;
                row2.DefaultCellStyle.BackColor = colorDeterminator ? Color.LightCyan : Color.LightYellow;
                dictDGV.Rows.Add(row2);

                colorDeterminator = !colorDeterminator;
            }
            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
    }
}
