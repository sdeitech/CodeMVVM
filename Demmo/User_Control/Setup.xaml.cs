using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel.ViewModel;

namespace Demmo.User_Control
{
    /// <summary>
    /// User control class for handling setup-related interactions.
    /// </summary>
    public partial class Setup : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Setup"/> class.
        /// </summary>
        public Setup()
        {
            // Initializes the user control and sets up the data context with a new instance of SetupViewModel.

            InitializeComponent();
            this.DataContext = new SetupViewModel();

        }
    }
}
