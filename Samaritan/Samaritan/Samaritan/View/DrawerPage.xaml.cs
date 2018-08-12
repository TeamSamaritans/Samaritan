using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Samaritan.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawerPage : ContentPage
    {
        public DrawerPage()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizerOfflineSavedTapped(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new PostList("Offline saved", true));
        }
    }
}