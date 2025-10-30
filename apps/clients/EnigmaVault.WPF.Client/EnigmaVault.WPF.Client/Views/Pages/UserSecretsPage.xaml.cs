using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace EnigmaVault.WPF.Client.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserSecretsPage.xaml
    /// </summary>
    public partial class UserSecretsPage : Page
    {
        public UserSecretsPage()
        {
            InitializeComponent();

            Loaded += ControlArchiveWindow_Loaded;
            Loaded += ControlToDeletedWindow_Loaded;
        }

        //TODO: Перенести код в Behavior
        #region Управление Popop с отображением архивированных записей 

        private void ControlArchiveWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (OpenArchiveManagementButton != null)
            {
                OpenArchiveManagementButton.Checked += OpenArchiveManagementButton_Checked;
                OpenArchiveManagementButton.Unchecked += OpenArchiveManagementButton_Unchecked;
                UpdateArchivePopupPosition();
            }
        }

        private void OpenArchiveManagementButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateArchivePopupPosition();
            ArchiveManagementPopup.IsOpen = true;
        }

        private void OpenArchiveManagementButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ArchiveManagementPopup.IsOpen = false;
        }

        private void UpdateArchivePopupPosition()
        {
            if (ArchiveManagementPopup != null && OpenArchiveManagementButton != null)
            {
                //double screenHeight = SystemParameters.PrimaryScreenHeight;
                //double buttonBottom = OpenArchiveManagementButton.PointToScreen(new Point(0, OpenArchiveManagementButton.ActualHeight)).Y;
                //ArchiveManagementPopup.MaxHeight = Math.Min(400, screenHeight - buttonBottom - 10);

                ArchiveManagementPopup.Height = ArchivedSecretsListView.Height + 100;
                ArchiveManagementPopup.MaxHeight = this.ActualHeight - 150;
            }
        }

        private CustomPopupPlacement[] PlaceArchivePopup(Size popupSize, Size targetSize, Point offset)
        {
            if (OpenArchiveManagementButton != null)
            {
                var target = OpenArchiveManagementButton;
                var buttonPosition = target.PointToScreen(new Point(0, 0));
                target.PointFromScreen(buttonPosition);

                // Для открытия справа от правого нижнего угла кнопки и направления вверх

                double xOffset = target.ActualWidth; // Смещение вправо на ширину кнопки
                double yOffset = 0;                  // Начинать с нижней границы кнопки
                return new[] 
                { 
                    //new CustomPopupPlacement(new Point(xOffset, yOffset - popupSize.Height), PopupPrimaryAxis.Vertical)
                    new CustomPopupPlacement(new Point(xOffset + 10, (yOffset - popupSize.Height) + 25), PopupPrimaryAxis.Vertical) 
                };
            }
            return new[] 
            {
                new CustomPopupPlacement(new Point(0, 0), PopupPrimaryAxis.Vertical) 
            };
        }

        #endregion

        #region Управление Popup с отображение записей для удаления

        private void ControlToDeletedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (OpenArchiveManagementButton != null)
            {
                OpenSecretsToDeleteManagementButton.Checked += OpenSecretsToDeleteManagementButton_Checked;
                OpenSecretsToDeleteManagementButton.Unchecked += OpenSecretsToDeleteManagementButton_Unchecked;
                UpdateSecretsToDeletePopupPosition();
            }
        }

        private void OpenSecretsToDeleteManagementButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateSecretsToDeletePopupPosition();
            TrashManagementPopup.IsOpen = true;
        }

        private void OpenSecretsToDeleteManagementButton_Unchecked(object sender, RoutedEventArgs e)
        {
            TrashManagementPopup.IsOpen = false;
        }

        private void UpdateSecretsToDeletePopupPosition()
        {
            if (TrashManagementPopup != null && OpenSecretsToDeleteManagementButton != null)
            {
                TrashManagementPopup.Height = TrashManagementPopup.Height + 100;
                TrashManagementPopup.MaxHeight = this.ActualHeight - 150;
            }
        }

        private CustomPopupPlacement[] PlaceSecretsToDeletePopup(Size popupSize, Size targetSize, Point offset)
        {
            if (OpenSecretsToDeleteManagementButton != null)
            {
                var target = OpenSecretsToDeleteManagementButton;
                var buttonPosition = target.PointToScreen(new Point(0, 0));
                target.PointFromScreen(buttonPosition);

                double xOffset = target.ActualWidth;
                double yOffset = 0;
                return new[]
                { 
                    new CustomPopupPlacement(new Point(xOffset + 10, (yOffset - popupSize.Height) + 50), PopupPrimaryAxis.Vertical)
                };
            }
            return new[]
            {
                new CustomPopupPlacement(new Point(0, 0), PopupPrimaryAxis.Vertical)
            };
        }

        #endregion
    }
}