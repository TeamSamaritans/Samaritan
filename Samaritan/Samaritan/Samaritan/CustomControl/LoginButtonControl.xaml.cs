using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Samaritan.CustomControl
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginButtonControl : ContentView
    {
		public LoginButtonControl ()
		{
			InitializeComponent ();
		}

        /// Bindable Property for TappedCommand
        /// </summary>
        public static readonly BindableProperty TappedCommandProperty = BindableProperty.Create(nameof(TappedCommand), typeof(Command), typeof(LoginButtonControl), null);

        /// Bindable Property for TappedCommand
        /// </summary>
        public static readonly BindableProperty SignUpTappedCommandProperty = BindableProperty.Create(nameof(SignUpTappedCommand), typeof(Command), typeof(LoginButtonControl), null);



        /// <summary>
        /// Gets or sets tapped command
        /// </summary>
        public Command TappedCommand
        {
            get { return (Command)GetValue(TappedCommandProperty); }
            set { SetValue(TappedCommandProperty, value); }
        }


        /// <summary>
        /// Gets or sets tapped command
        /// </summary>
        public Command SignUpTappedCommand
        {
            get { return (Command)GetValue(SignUpTappedCommandProperty); }
            set { SetValue(SignUpTappedCommandProperty, value); }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (this.TappedCommand != null)
            {
                this.TappedCommand.Execute(null);
            }
        }

        private void SignUp_Tapped(object sender, EventArgs e)
        {
            if (this.SignUpTappedCommand != null)
            {
                this.SignUpTappedCommand.Execute(null);
            }
        }
    }
}