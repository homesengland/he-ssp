namespace HE.Investments.Loans.Contract.ViewModels;

public interface IEditableViewModel
{
    public bool IsEditable();

    public bool IsReadOnly() => !IsEditable();
}
