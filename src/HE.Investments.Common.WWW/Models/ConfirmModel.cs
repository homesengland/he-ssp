namespace HE.Investments.Common.WWW.Models;

public class ConfirmModel<T>
{
    public ConfirmModel()
    {
    }

    public ConfirmModel(T viewModel)
    {
        ViewModel = viewModel;
    }

    public T ViewModel { get; set; }

    public string Response { get; set; }
}
