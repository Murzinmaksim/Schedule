using System.Windows;
using System.Windows.Controls;


namespace ScheduleUI
{
   public partial class RulerControl : UserControl
    {
        public RulerControl()
        {
            InitializeComponent();
            SizeChanged += RulerControl_SizeChanged;
          
        }

        private void RulerControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DataContext is ViewModels.RulerViewModels viewModel)
            {
                viewModel.UpdateLine(e.NewSize.Width, e.NewSize.Height);
            }

        }
   }
}
