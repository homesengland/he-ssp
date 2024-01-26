using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace HE.DocumentService.SharePoint.Extensions;

public static class ListItemCollectionExtension
{
    public static DataTable MapDataTable(this ListItemCollection items)
    {
        var dtGetReqForm = new DataTable();
        if (items != null && items.Count > 0)
        {
            foreach (var field in items[0].FieldValues.Keys)
            {
                dtGetReqForm.Columns.Add(field);
            }

            foreach (var item in items)
            {
                var dr = dtGetReqForm.NewRow();

                foreach (var obj in item.FieldValues)
                {
                    if (obj.Value != null)
                    {
                        var type = obj.Value.GetType().FullName;

                        if (type == "Microsoft.SharePoint.Client.FieldLookupValue")
                        {
                            dr[obj.Key] = ((FieldLookupValue)obj.Value).LookupValue;
                        }
                        else if (type == "Microsoft.SharePoint.Client.FieldUserValue")
                        {
                            dr[obj.Key] = ((FieldUserValue)obj.Value).LookupValue;
                        }
                        else if (type == "Microsoft.SharePoint.Client.FieldUserValue[]")
                        {
                            var multValue = (FieldUserValue[])obj.Value;
                            foreach (var fieldUserValue in multValue)
                            {
                                dr[obj.Key] += fieldUserValue.LookupValue;
                            }
                        }
                        else if (type == "System.DateTime")
                        {
#pragma warning disable CS8602
#pragma warning disable S6580 // Dereference of a possibly null reference.
                            var val = obj.Value.ToString();
                            if (val.Length > 0 && DateTime.TryParse(val, out var r))
                            {
                                dr[obj.Key] = r;
                            }
                            dr[obj.Key] = obj.Value;
#pragma warning restore CS8602
#pragma warning restore S6580 // Dereference of a possibly null reference.
                        }
                        else
                        {
                            dr[obj.Key] = obj.Value;
                        }
                    }
                    else
                    {
                        dr[obj.Key] = null;
                    }
                }
                dtGetReqForm.Rows.Add(dr);
            }
        }

        return dtGetReqForm;
    }
}
