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
	public partial class ExtendedEntryControl : ContentView
    {
		public ExtendedEntryControl ()
		{
			InitializeComponent ();
            this.BindingContext = this;
        }

        /// <summary>
        /// Bindable property to get text
        /// </summary>
        public static readonly BindableProperty TextValueProperty = BindableProperty.Create("TextValue", typeof(string), typeof(ExtendedEntryControl), string.Empty);

        /// <summary>
        /// Gets or sets property of TextValue
        /// </summary>
        public string TextValue
        {
            get { return (string)GetValue(TextValueProperty); }
            set { SetValue(TextValueProperty, value); }
        }

        /// <summary>
        /// BindableProperty to get image source
        /// </summary>
        public static readonly BindableProperty SourceValueProperty = BindableProperty.Create("SourceValue", typeof(ImageSource), typeof(ExtendedEntryControl), null);

        /// <summary>
        /// Gets or sets property of image source
        /// </summary>
        public ImageSource SourceValue
        {
            get { return (ImageSource)GetValue(SourceValueProperty); }
            set { SetValue(SourceValueProperty, value); }
        }


        /// <summary>
        /// Bindable property to get placeholder 
        /// </summary>
        public static readonly BindableProperty PlaceHolderValueProperty = BindableProperty.Create("PlaceHolderValue", typeof(string), typeof(ExtendedEntryControl), string.Empty);

        /// <summary>
        /// Gets or sets property of placeholder
        /// </summary>
        public string PlaceHolderValue
        {
            get { return (string)GetValue(PlaceHolderValueProperty); }
            set { SetValue(PlaceHolderValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets property of IsPassword
        /// </summary>
        public bool IsPassword
        {
            get { return (Boolean)GetValue(IsPasswordProperty); }
            set { base.SetValue(IsPasswordProperty, value); }
        }

        /// <summary>
        /// Bindable property to get IsPassword 
        /// </summary>
        public static BindableProperty IsPasswordProperty =
              BindableProperty.Create("IsPassword", typeof(Boolean), typeof(ExtendedEntryControl), false, BindingMode.TwoWay, propertyChanged: IsPasswordPropertyChanged);

        public static void IsPasswordPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue?.ToString() != newValue?.ToString())
            {
                var control = bindable as ExtendedEntryControl;
                if (control == null) return;
                control.entry.IsPassword = (Boolean)newValue;
            }
        }
    }
}