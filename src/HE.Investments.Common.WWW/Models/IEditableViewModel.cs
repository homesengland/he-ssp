namespace HE.Investments.Common.WWW.Models;

public interface IEditableViewModel
{
    public bool IsEditable { get; }

    public bool IsReadOnly { get; }
}
