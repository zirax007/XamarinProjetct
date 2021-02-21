using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace MyClass.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfosPopupView
    {

        public InfosPopupView(string PopUpTitle, string PopUpAbsence, string PopUpPresence)
        {
            InitializeComponent();
            PopUpStudentName.Text = PopUpTitle;
            if(PopUpAbsence != null)
            {
                PopUpStudentAbsenceSL.IsVisible = true;
                PopUpStudentAbsence.Text = PopUpAbsence;
            } else
            {
                PopUpStudentAbsenceSL.IsVisible = false;
            }

            if(PopUpPresence != null)
            {
                PopUpStudentPresentSL.IsVisible = true;
                PopUpStudentPresent.Text = PopUpPresence;
            } else
            {
                PopUpStudentPresentSL.IsVisible = false;
            }
        }

        private void ClosePopUpDtails(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}