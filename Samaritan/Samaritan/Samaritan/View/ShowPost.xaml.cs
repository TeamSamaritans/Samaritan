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
	public partial class ShowPost : ContentPage
	{
        private string postId;
		public ShowPost(string id)
		{
			InitializeComponent ();
            postId = id;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ShowPostView();
        }

        private void ShowPostView()
        {
            webview.Source = "http://taritas.in/left-behind/details-view/" + postId;
        }
    }
}