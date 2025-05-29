using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Linq;

namespace INGIN.Helper
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class HelperCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Получение текущего документа
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            HelperView HelperView = new HelperView();
            
            HelperView.ShowDialog();


            return Result.Succeeded;
        }
    }
}
