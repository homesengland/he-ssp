using System.Data;
using System.Globalization;
using Microsoft.SharePoint.Client;

namespace HE.DocumentService.SharePoint.Extensions;

public static class ListItemCollectionExtension
{
    public static DataTable MapDataTable(this ListItemCollection? items)
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
                            var val = obj.Value.ToString();
                            if (val is { Length: > 0 } && DateTime.TryParse(val, CultureInfo.InvariantCulture, DateTimeStyles.None, out var r))
                            {
                                dr[obj.Key] = r;
                            }

                            dr[obj.Key] = obj.Value;
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
