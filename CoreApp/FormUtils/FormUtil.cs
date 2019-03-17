using CoreApp.Dicts;
using CoreApp.InfaObjects;
using CoreApp.Keys;
using CoreApp.Parsers;
using CoreApp.ReleaseObjects;
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
            foreach(FileInfo file in parser.OraObjectDict.notFoundFiles)
            {
                dictDGV.Rows.Add(file.FullName);
            }

            foreach(FileInfo file in parser.InfaObjectDict.notFoundFiles)
            {
                dictDGV.Rows.Add(file.FullName);
            }
            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public static void AddObjectsInDGV(DataGridView dictDGV, ETLParser parser)
        {
            foreach(KeyValuePair<ETLObject, ZPatch> item in parser.OraObjectDict.baseDict.EnumerateObjPatchPairs())
            {
                ETLObject oraObj = item.Key;
                ZPatch patch = item.Value;
                dictDGV.Rows.Add(oraObj.ObjName, oraObj.ObjType, patch.ZPatchName);
            }

            foreach (KeyValuePair<ETLObject, ZPatch> item in parser.InfaObjectDict.baseDict.EnumerateObjPatchPairs())
            {
                ETLObject infaObj = item.Key;
                ZPatch patch = item.Value;
                dictDGV.Rows.Add(infaObj.ObjName, infaObj.ObjType, patch.ZPatchName);
            }

            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public static void AddIntersectionsInDGV(DataGridView dictDGV, ETLParser parser)
        {
            bool colorDeterminator = false;
            foreach (var pairs in  parser.OraObjectDict.intersections.EnumerateByDistinctKeys())
            {
                foreach (var pair in pairs)
                {
                    ETLObject oraObj = pair.Key;
                    ZPatch patch = pair.Value;
                    DataGridViewRow row = new DataGridViewRow();
                    row.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                    row.Cells[0].Value = oraObj.ObjName;
                    row.Cells[1].Value = oraObj.ObjType;
                    row.Cells[2].Value = patch.ZPatchName;
                    row.DefaultCellStyle.BackColor = colorDeterminator ? Color.LightCyan : Color.LightYellow;
                    dictDGV.Rows.Add(row);                    
                }
                colorDeterminator = !colorDeterminator;
            }

            foreach (var pairs in parser.InfaObjectDict.intersections.EnumerateByDistinctKeys())
            {
                foreach (var pair in pairs)
                {
                    ETLObject infaObj = pair.Key;
                    ZPatch patch = pair.Value;
                    DataGridViewRow row = new DataGridViewRow();
                    row.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                    row.Cells[0].Value = infaObj.ObjName;
                    row.Cells[1].Value = infaObj.GetType().Name;
                    row.Cells[2].Value = patch.ZPatchName;
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
            foreach (KeyValuePair<ETLObject, ETLObject> item in parser.InfaObjectDict.infaDependencies.EnumeratePairs())
            {
                ETLObject infaObj1 = item.Key;
                DataGridViewRow row1 = new DataGridViewRow();
                row1.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                row1.Cells[0].Value = infaObj1.ObjName;
                row1.Cells[1].Value = infaObj1.GetType().Name;
                row1.Cells[2].Value = infaObj1.Patch.ZPatchName;
                row1.DefaultCellStyle.BackColor = colorDeterminator ? Color.LightCyan : Color.LightYellow;
                dictDGV.Rows.Add(row1);

                ETLObject infaObj2 = item.Value;
                DataGridViewRow row2 = new DataGridViewRow();
                row2.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                row2.Cells[0].Value = infaObj2.ObjName;
                row2.Cells[1].Value = infaObj2.GetType().Name;
                row2.Cells[2].Value = infaObj2.Patch.ZPatchName;
                row2.DefaultCellStyle.BackColor = colorDeterminator ? Color.LightCyan : Color.LightYellow;
                dictDGV.Rows.Add(row2);

                colorDeterminator = !colorDeterminator;
            }
            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public static void AddLostDependenciesInDGV(DataGridView dictDGV, ETLParser parser)
        {
            bool colorDeterminator = false;
            foreach (KeyValuePair<ETLObject, ETLObject> item in parser.InfaObjectDict.infaLostDependencies.EnumeratePairs())
            {
                ETLObject infaObj1 = item.Key;
                DataGridViewRow row1 = new DataGridViewRow();
                row1.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                row1.Cells[0].Value = infaObj1.ObjName;
                row1.Cells[1].Value = infaObj1.GetType().Name;
                row1.Cells[2].Value = infaObj1.Patch.ZPatchName;
                row1.DefaultCellStyle.BackColor = colorDeterminator ? Color.LightCyan : Color.LightYellow;
                dictDGV.Rows.Add(row1);

                ETLObject infaObj2 = item.Value;
                DataGridViewRow row2 = new DataGridViewRow();
                row2.Cells.AddRange(new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell(), new DataGridViewTextBoxCell());
                row2.Cells[0].Value = infaObj2.ObjName;
                row2.Cells[1].Value = infaObj2.GetType().Name;
                row2.Cells[2].Value = infaObj2.Patch.ZPatchName;
                row2.DefaultCellStyle.BackColor = colorDeterminator ? Color.LightCyan : Color.LightYellow;
                dictDGV.Rows.Add(row2);

                colorDeterminator = !colorDeterminator;
            }
            dictDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
    }
}
