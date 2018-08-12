
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Samaritan.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDrawerPage : MasterDetailPage
    {
        public MasterDrawerPage()
        {
            InitializeComponent();
            this.Detail = new NavigationPage(new PostList(string.Empty));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.IsPresented = false;
        }
    }
}