using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Structure;
using System.Diagnostics;
using System.Collections;
using System.Reflection.Emit;
using System.Xml.Linq;


namespace INGIN.ScheduleSpecification
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class ScheduleSpecificationCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;
            ViewSchedule schedSpec;

            //Получение ведомости спецификаций
            try
            {
                schedSpec = new FilteredElementCollector(doc)
                    .OfClass(typeof(View))
                    .Where(f => f.Name.Contains("Ведомость спецификаций"))
                    .Cast<ViewSchedule>()
                    .First();
            }
            catch
            {
                TaskDialog.Show("ИНЖИН", "Добавьте в проект спецификацию: \n- ОД_Ведомость спецификаций");
                return Autodesk.Revit.UI.Result.Failed;
            }


            //Получение всех спецификаций, вынесенных на листы
            var schedOnSheets = new FilteredElementCollector(doc)
                .OfClass(typeof(ScheduleSheetInstance))
                .Where(sched => sched.Name.Contains("Спецификация"))
                .OrderBy(sched => sched.Name)
                .ToList();

            //Количество спецификаций, вынесенный на лист
            var numbOfSchedOnSheet = schedOnSheets.Count;

            //Получение данных ведомости спецификаций
            var secData = schedSpec.GetTableData().GetSectionData(SectionType.Header);

            //Ведомость спецификаций
            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("INGIN:Ведомость спецификаций");

                //Удаление всех строк, кроме заголовка
                while (secData.NumberOfRows != 2)
                {
                    secData.RemoveRow(secData.NumberOfRows - 1);
                }

                //Добавление пустых строк в спецификацию
                while (secData.NumberOfRows != numbOfSchedOnSheet + 2)
                {
                    secData.InsertRow(secData.NumberOfRows);
                }

                //Получение данных для ведомости спецификаций
                var schedDataList = new List<(string, string, string)>();
                foreach (var schedOnSheet in schedOnSheets)
                {
                    var sheetNumber = Convert.ToInt32((doc.GetElement(schedOnSheet.OwnerViewId) as ViewSheet).SheetNumber).ToString();
                    var schedName = schedOnSheet.Name;
                    if (schedName.Take(3).Contains('_'))
                    {
                        schedName = schedOnSheet.Name.Substring(3);
                    }

                    string schedNote;
                    try
                    {
                        schedNote = schedOnSheet.LookupParameter("О_Примечание").AsValueString();
                    }
                    catch
                    {
                        schedNote = "";
                    }

                    var schedData = (sheetNumber, schedName, schedNote);
                    schedDataList.Add(schedData);
                }

                //Сортировка данных по номеру листа
                var schedDataListSort = schedDataList.OrderBy(s => Convert.ToInt32(s.Item1)).ToList();

                //Заполнение строк ведомости спецификаций
                var i = 2; //Первые 2 строки это заголовок спецификации
                foreach (var rowData in schedDataListSort)
                {
                    secData.SetCellText(i, 0, rowData.Item1);
                    secData.SetCellText(i, 1, rowData.Item2);
                    secData.SetCellText(i, 2, rowData.Item3);
                    i++;
                }

                transaction.Commit();
            }


            TaskDialog.Show("ИНЖИН", $"Ведомость спецификаций заполнена");

            return Result.Succeeded;
        }
    }
}

