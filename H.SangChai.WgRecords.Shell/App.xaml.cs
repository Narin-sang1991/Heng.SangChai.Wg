using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using WPFLocalizeExtension.Engine;

namespace H.SangChai.WgRecords.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        BootstrapperClient bootstrapper = new BootstrapperClient();

        private void OnStartup(object sender, StartupEventArgs e)
        {
            string defaultCulture = string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultCulture"]) ? "th-TH" : ConfigurationManager.AppSettings["DefaultCulture"];
            CultureInfo cultureInfo = new CultureInfo(defaultCulture); //(CultureInfo)CultureInfo.CurrentCulture.Clone();
            cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            cultureInfo.DateTimeFormat.FullDateTimePattern = "dd/MM/yyyy h:mm:ss tt";

            cultureInfo.NumberFormat.CurrencyDecimalDigits = 6;
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            cultureInfo.NumberFormat.CurrencyGroupSeparator = ",";
            cultureInfo.NumberFormat.CurrencyGroupSizes = new int[] { 3 };
            cultureInfo.NumberFormat.CurrencySymbol = string.Empty;

            cultureInfo.NumberFormat.NumberDecimalDigits = 2;
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            cultureInfo.NumberFormat.NumberGroupSeparator = ",";
            cultureInfo.NumberFormat.NumberGroupSizes = new int[] { 3 };

            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            LocalizeDictionary.Instance.Culture = cultureInfo;

            string defaultCalendarCulture = string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultCalendarCulture"]) ? "th-TH" : ConfigurationManager.AppSettings["DefaultCalendarCulture"];
            CultureInfo calendarCultureInfo = new CultureInfo(defaultCalendarCulture);

            calendarCultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            calendarCultureInfo.DateTimeFormat.FullDateTimePattern = "dd/MM/yyyy h:mm:ss tt";

            calendarCultureInfo.NumberFormat.CurrencyDecimalDigits = 6;
            calendarCultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            calendarCultureInfo.NumberFormat.CurrencyGroupSeparator = ",";
            calendarCultureInfo.NumberFormat.CurrencyGroupSizes = new int[] { 3 };
            calendarCultureInfo.NumberFormat.CurrencySymbol = string.Empty;

            calendarCultureInfo.NumberFormat.NumberDecimalDigits = 2;
            calendarCultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            calendarCultureInfo.NumberFormat.NumberGroupSeparator = ",";
            calendarCultureInfo.NumberFormat.NumberGroupSizes = new int[] { 3 };

            Thread.CurrentThread.CurrentCulture = calendarCultureInfo;

            EventManager.RegisterClassHandler(typeof(TextBox),
                TextBox.PreviewKeyDownEvent,
                new System.Windows.Input.KeyEventHandler(TextBox_PreviewKeyDown));

            bootstrapper.Run();
        }

        private void TextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (((TextBox)sender).AcceptsReturn && e.Key == System.Windows.Input.Key.Return)
            {
                int cursorPosition = ((TextBox)sender).SelectionStart;
                ((TextBox)sender).SelectedText = string.Empty;
                ((TextBox)sender).Text = ((TextBox)sender).Text.Insert(cursorPosition, "\r\n");
                ((TextBox)sender).SelectionStart = cursorPosition + 1;

                e.Handled = true;
            }
        }
    }
}
