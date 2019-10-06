using System;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Phoneword
{
  public class MainPage : ContentPage
  {
    private Label inputLabel;
    private Entry userEntry;
    private Button translateButton;
    private Button callButton;
    private String translatedNumber;

    public MainPage()
    {
      this.Padding = 20;
      StackLayout stackLayout = new StackLayout();
      stackLayout.Spacing = 15;

      // A StackLayout doesn't have color. Organizational structure only.
      //stackLayout.BackgroundColor = Color.Red;

      // Label
      inputLabel = new Label();
      inputLabel.Text = "Enter a Phoneword:";

      // Entry
      userEntry = new Entry();
      userEntry.Text = "1-855-XAMARIN";

      // Button
      translateButton = new Button();
      translateButton.Text = "Translate";
      translateButton.Clicked += OnTranslate;

      // 2nd Button
      callButton = new Button();
      callButton.Text = "Call";
      callButton.IsEnabled = false;
      callButton.Clicked += OnCall;

      stackLayout.Children.Add(inputLabel);
      stackLayout.Children.Add(userEntry);
      stackLayout.Children.Add(translateButton);
      stackLayout.Children.Add(callButton);

      this.Content = stackLayout;
    }

    private void OnTranslate(Object sender, EventArgs e)
    {
      string enteredNumber = userEntry.Text;
      translatedNumber = Phoneword.PhonewordTranslator.ToNumber(enteredNumber);

      if (!string.IsNullOrEmpty(translatedNumber))
      {
        callButton.IsEnabled = true;
        callButton.Text = "Call " + translatedNumber;
      }
      else
      {
        callButton.IsEnabled = false;
        callButton.Text = "Call";
      }
    }

    async void OnCall(Object sender, EventArgs e)
    {
      bool callDesired = await this.DisplayAlert("Dial a Number", "Would you like to call " + translatedNumber + "?", "Yes", "No");
      if (callDesired)
      {
        try
        {
          PhoneDialer.Open(translatedNumber);
        }
        catch (ArgumentNullException)
        {
          await DisplayAlert("Unable to dial", "Phone number was not valid.", "OK");
        }
        catch (FeatureNotSupportedException)
        {
          await DisplayAlert("Unable to dial","Phone dialing not supported.","OK");
        }
        catch (Exception)
        {
          // Other error has occurred.
          await DisplayAlert("Unable to dial","Phone dialing failed.","OK");
        }
      }
    }
  }
}
