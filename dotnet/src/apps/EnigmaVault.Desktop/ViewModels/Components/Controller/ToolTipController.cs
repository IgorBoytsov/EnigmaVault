using CommunityToolkit.Mvvm.ComponentModel;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.ViewModels.Base;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace EnigmaVault.Desktop.ViewModels.Components.Controller
{
    public sealed partial class ToolTipController : BaseViewModel
    {
        private const double PlacementOffset = 8.0;

        public ToolTipController(ToolTipPlacement placement)
        {
            CustomPlacementCallback = placement switch
            {
                ToolTipPlacement.CenterLeft => PlaceCenterLeft,
                ToolTipPlacement.CenterRight => PlaceCenterRight,
                ToolTipPlacement.CenterTop => PlaceCenterTop,
                ToolTipPlacement.CenterBottom => PlaceCenterBottom,
                _ => throw new ArgumentOutOfRangeException(nameof(placement), "Неизвестный режим размещения ToolTip.")
            };
        }

        [ObservableProperty]
        private bool _isOpen;

        [ObservableProperty]
        private object? _content;

        public CustomPopupPlacementCallback CustomPlacementCallback { get; }

        private CustomPopupPlacement[] PlaceCenterRight(Size toolTipSize, Size targetSize, Point offset)
        {
            var position = new Point(targetSize.Width + PlacementOffset, (targetSize.Height - toolTipSize.Height) / 2);
            return [new CustomPopupPlacement(position, PopupPrimaryAxis.Horizontal)];
        }

        private CustomPopupPlacement[] PlaceCenterLeft(Size toolTipSize, Size targetSize, Point offset)
        {
            var position = new Point(-toolTipSize.Width - PlacementOffset, (targetSize.Height - toolTipSize.Height) / 2);
            return [new CustomPopupPlacement(position, PopupPrimaryAxis.Horizontal)];
        }

        private CustomPopupPlacement[] PlaceCenterTop(Size toolTipSize, Size targetSize, Point offset)
        {
            var position = new Point((targetSize.Width - toolTipSize.Width) / 2, -toolTipSize.Height - PlacementOffset);
            return [new CustomPopupPlacement(position, PopupPrimaryAxis.Vertical)];
        }

        private CustomPopupPlacement[] PlaceCenterBottom(Size toolTipSize, Size targetSize, Point offset)
        {
            var position = new Point((targetSize.Width - toolTipSize.Width) / 2, targetSize.Height + PlacementOffset);
            return [new CustomPopupPlacement(position, PopupPrimaryAxis.Vertical)];
        }
    }
}