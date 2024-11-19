using FabSandbox.Components.Layout;
using MudBlazor;

namespace FabSandbox.Data.Interfaces
{
    public interface IUserInterfaceToolsServices
    {
        CoreDialogMessageComponent CoreDialogMessageComponent { get; set; }
        CoreIndicatorProcessingComponent CoreIndicatorProcessingComponent { get; set; }
        CoreIndicatorProgressBarComponent CoreIndicatorProgressBarComponent { get; set; }
        CoreSnackbarMessageComponent SnackbarMessagesComponent { get; set; }
        UserInterfaceToolsServicesProvider UserInterfaceToolsServicesProvider { get; set; }

        Task HideLoadingIndicator();
        Task HideProgressBarIndicator();
        Task NavigateBack();
        Task OnProgressBarSetCurrentValue(int currentValue);
        Task OnProgressBarSetMaxValue(int maxValue);
        Task<bool> ShowDialogMessage(string title, string contentText, string buttonText, MudBlazor.Color buttonColor);
        Task ShowLoadingIndicator(string title = "");
        Task ShowProgressBarIndicator(string title = "", int maxValue = 0);
        void ShowSnackbarMessage(string title, string text, Severity severity);

    }
}