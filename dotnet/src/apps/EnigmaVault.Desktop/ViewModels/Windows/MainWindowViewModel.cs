using EnigmaVault.Desktop.Services.PageNavigation;
using EnigmaVault.Desktop.ViewModels.Base;
using EnigmaVault.Desktop.ViewModels.Components.Controller;
using Shared.WPF.Navigations.Windows;

namespace EnigmaVault.Desktop.ViewModels.Windows
{
    internal sealed partial class MainWindowViewModel(IWindowNavigation windowNavigation, IPageNavigation pageNavigationService) : BaseWindowViewModel(windowNavigation, pageNavigationService)
    {
        public ToolTipController RightToolTipController { get; } = new(Enums.ToolTipPlacement.CenterRight);
        public ToolTipController BottomToolTipController { get; } = new(Enums.ToolTipPlacement.CenterBottom);
    }
}