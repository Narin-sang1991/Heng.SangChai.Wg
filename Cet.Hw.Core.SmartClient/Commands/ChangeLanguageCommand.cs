using Cet.Hw.Core.AppServiceContract;
using Cet.Hw.Core.SmartClient.ViewModels;
using Cet.SmartClient.Client;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLocalizeExtension.Engine;

namespace Cet.Hw.Core.SmartClient.Commands
{
    public class ChangeLanguageCommand : MenuCommand
    {
        private readonly IUnityContainer container;
        private string languageName;
        public string LanguageName
        {
            get { return languageName; }
            set { languageName = value; }
        }

        public ChangeLanguageCommand(IUnityContainer container, string languageName) : base(container, MenuResources.ChangeLanguageEN)
        {
            this.container = container;
            this.languageName = languageName;
        }

        public override void Execute(object parameter)
        {
            ChangeLanguage(parameter);
        }

        private void ChangeLanguage(object lang)
        {
            CultureInfo cultureInfo = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentUICulture.Clone();
            string cultureText = cultureInfo.ToString();
            if (cultureText == languageName) return;

            CultureInfo newCultureInfo = CultureInfo.GetCultureInfo(languageName);
            System.Threading.Thread.CurrentThread.CurrentUICulture = newCultureInfo;
            LocalizeDictionary.Instance.Culture = newCultureInfo;

            //this.container.Resolve<IConfigurationService>().UpdateCurrentLanuage(newCultureInfo);

            //Load Menu
            MenuGenerator menuGenerator = this.container.Resolve<MenuGenerator>();
            menuGenerator.LoadMenu();
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
