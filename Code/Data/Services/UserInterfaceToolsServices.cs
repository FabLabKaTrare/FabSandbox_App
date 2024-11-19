using FabSandbox.Components.Layout;
using FabSandbox.Data.Interfaces;
using MudBlazor;

namespace FabSandbox.Data.Services
{
    public class UserInterfaceToolsServices : IUserInterfaceToolsServices
    {
        public CoreDialogMessageComponent CoreDialogMessageComponent { get; set; }

        public CoreIndicatorProcessingComponent CoreIndicatorProcessingComponent { get; set; }

        public CoreIndicatorProgressBarComponent CoreIndicatorProgressBarComponent { get; set; }

        public CoreSnackbarMessageComponent SnackbarMessagesComponent { get; set; }

        public UserInterfaceToolsServicesProvider UserInterfaceToolsServicesProvider { get; set; }

        public async Task<bool> ShowDialogMessage(string title, string contentText, string buttonText, MudBlazor.Color buttonColor)
        {
            if (CoreDialogMessageComponent == null)
                return false;

            var result = await CoreDialogMessageComponent.ShowDialogMessage(title, contentText, buttonText, buttonColor);

            return !result.Canceled;
        }

        public void ShowSnackbarMessage(string title, string text, Severity severity)
        {
            if (SnackbarMessagesComponent == null)
                return;

            SnackbarMessagesComponent.ShowMessage(title, text, severity);
        }

        public async Task ShowLoadingIndicator(string title = "")
        {
            if (CoreIndicatorProcessingComponent == null)
                return;

            if (LoadingState == false)
            {
                LoadingState = true;

                await CoreIndicatorProcessingComponent?.OnShowLoading(title);
            }
        }

        public async Task ShowProgressBarIndicator(string title = "", int maxValue = 0)
        {
            if (CoreIndicatorProgressBarComponent == null)
                return;

            await CoreIndicatorProgressBarComponent?.OnShow(title, maxValue);

            await CoreIndicatorProgressBarComponent?.OnSetCurrentValue(0);
        }

        public async Task OnProgressBarSetCurrentValue(int currentValue)
        {
            if (CoreIndicatorProgressBarComponent == null)
                return;

            await CoreIndicatorProgressBarComponent?.OnSetCurrentValue(currentValue);
        }
        public async Task OnProgressBarSetMaxValue(int maxValue)
        {
            if (CoreIndicatorProgressBarComponent == null)
                return;

            await CoreIndicatorProgressBarComponent?.OnSetMaxValue(maxValue);
        }

        public async Task HideLoadingIndicator()
        {
            if (CoreIndicatorProcessingComponent == null)
                return;

            if (LoadingState)
            {
                LoadingState = false;

                await CoreIndicatorProcessingComponent?.OnHideLoading();
            }
        }

        public async Task HideProgressBarIndicator()
        {
            if (CoreIndicatorProgressBarComponent == null)
                return;

            await CoreIndicatorProgressBarComponent?.OnHide();
        }

        public async Task NavigateBack()
        {
            await UserInterfaceToolsServicesProvider?.NavigateBack();
        }

        
        bool LoadingState = false;
        
    }
}
