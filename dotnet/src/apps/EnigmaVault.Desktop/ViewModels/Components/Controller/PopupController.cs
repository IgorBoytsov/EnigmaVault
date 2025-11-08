using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.ViewModels.Base;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace EnigmaVault.Desktop.ViewModels.Components.Controller
{
    internal sealed partial class PopupController : BaseViewModel
    {
        private readonly Func<bool>? _condition;

        public PopupController(PopupPlacementMode placementMode = PopupPlacementMode.Default, Func<bool>? condition = null)
        {
            if (placementMode == PopupPlacementMode.CustomRightUp)
                CustomPlacementCallback = PlacePopupRightUp;

            if (condition != null)
                _condition = condition;
        }

        /*--Свойства--------------------------------------------------------------------------------------*/

        [ObservableProperty]
        private bool _isOpen;

        [ObservableProperty]
        private bool _staysOpen = true;

        [ObservableProperty]
        private UIElement? _placementTarget;

        [ObservableProperty]
        private PlacementMode _currentPlacement = PlacementMode.Left;

        /*--Команды---------------------------------------------------------------------------------------*/

        [RelayCommand(CanExecute = nameof(CanShow))]
        private void Show(UIElement target)
        {
            CurrentPlacement = PlacementMode.Left;
            PlacementTarget = target;
            IsOpen = true;
        }

        [RelayCommand]
        public void ShowAtMouse()
        {
            CurrentPlacement = PlacementMode.MousePoint;
            PlacementTarget = null;
            IsOpen = true;
        }

        private bool CanShow(UIElement? target = null)
        {
            if (_condition is null) return true;
            return _condition();
        }

        [RelayCommand]
        private void Hide()
        {
            IsOpen = false;
            PlacementTarget = null;
        }

        /*--Дополнительная логика-------------------------------------------------------------------------*/

        [ObservableProperty]
        private double _height = double.NaN;

        [ObservableProperty]
        private double _maxHeight = double.PositiveInfinity;

        public CustomPopupPlacementCallback? CustomPlacementCallback { get; private set; }

        public void UpdatePopupSize(double containerActualHeight, double contentActualHeight = 0)
        {
            if (containerActualHeight > 100)
                MaxHeight = containerActualHeight - 100;

            if (contentActualHeight > 0)
                Height = contentActualHeight + 100;
        }

        private CustomPopupPlacement[] PlacePopupRightUp(Size popupSize, Size targetSize, Point offset)
        {
            if (PlacementTarget is FrameworkElement target)
            {
                double xOffset = target.ActualWidth;
                double yOffset = 0;
                return [new CustomPopupPlacement(new Point(xOffset + 10, (yOffset - popupSize.Height) + 25), PopupPrimaryAxis.Vertical)];
            }
            return [new CustomPopupPlacement(new Point(0, 0), PopupPrimaryAxis.Vertical)];
        }
    }
}