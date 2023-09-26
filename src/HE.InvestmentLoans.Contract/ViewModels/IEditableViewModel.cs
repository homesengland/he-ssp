namespace HE.InvestmentLoans.Contract.ViewModels;

public interface IEditableViewModel
{
    public bool IsEditable();

    public bool IsReadOnly() => IsEditable() is false;
}
