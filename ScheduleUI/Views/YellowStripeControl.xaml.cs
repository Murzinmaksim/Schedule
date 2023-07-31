using System.Windows;
using System.Windows.Controls;

namespace ScheduleUI
{
    public partial class YellowStripeControl : UserControl
    {
       
        public YellowStripeControl()
        {
            InitializeComponent();
            SizeChanged += UserControl_SizeChanged;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DataContext is ViewModels.YellowStripeViewModel viewModel)
            {
                viewModel.UpdateStripe(e.NewSize.Width, e.NewSize.Height);
            }
        }
    }
}
