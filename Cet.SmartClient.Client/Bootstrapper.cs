using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.UnityExtensions;
using System.Windows;
using Cet.Core.Logging;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Globalization;
using System.Threading;
using Microsoft.Practices.Prism.Modularity;
using System.Configuration;
using System.ServiceModel.Configuration;
using System.ServiceModel;
using Cet.Core;
using System.Reflection;
using Cet.SmartClient.Client.Resources;
using System.Windows.Controls;
using Microsoft.Practices.EnterpriseLibrary.Validation;


namespace Cet.SmartClient.Client
{
    public abstract class Bootstrapper : UnityBootstrapper
    {
        protected override void InitializeShell()
        {
            base.InitializeShell();
            Application.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(app_DispatcherUnhandledException);
            Application.Current.Exit += new ExitEventHandler(app_Exit);
            Application.Current.MainWindow.Show();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            ModuleCatalog catalog = new ConfigurationModuleCatalog();
            return catalog;
        }

        protected virtual void app_Exit(object sender, ExitEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        const string UnityConfigurationSectionName = "unity";

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            UnityConfigurationSection section = ConfigurationManager.GetSection(UnityConfigurationSectionName) as UnityConfigurationSection;
            if (section == null) return;
            foreach (ContainerElement containerElement in section.Containers)
            {
                section.Configure(Container, containerElement.Name);
            }
        }

        protected virtual void app_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception is ResourceReferenceKeyNotFoundException)
            {
                e.Handled = false;
            }
            else if (e.Exception is FaultException<DataValidationException>)
            {
                FaultException<DataValidationException> fault = e.Exception as FaultException<DataValidationException>;
                MessageBox.Show(fault.Detail.Message, Messages.MSG_WARNING_CAPTION);
                e.Handled = true;
            }
            else if (e.Exception is FaultException<ApplicationSecurityException>)
            {
                MessageBox.Show(Messages.MSG_PERMISSION_DENIED, Messages.MSG_ERROR_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            }
            else if (e.Exception is FaultException<DomainException>)
            {
                FaultException<DomainException> fault = e.Exception as FaultException<DomainException>;
                MessageBox.Show(fault.Detail.Message, Messages.MSG_WARNING_CAPTION);
                e.Handled = true;
            }
            else if (e.Exception is FaultException<ValidationResults>)
            {
                FaultException<ValidationResults> fault = e.Exception as FaultException<ValidationResults>;
                string message = string.Empty;
                foreach (var error in fault.Detail)
                {
                    if (fault.Detail.FirstOrDefault() != error)
                        message += "\n";
                    message += error.Message;
                }
                MessageBox.Show(message, Messages.MSG_ERROR_CAPTION);
                e.Handled = true;
            }
            else
            {
                if (e.Exception is FaultException<ExceptionDetail>)
                {
                    FaultException<ExceptionDetail> ex = e.Exception as FaultException<ExceptionDetail>;
                    if (ex.Detail != null)
                    {
                        StringBuilder errorMsg = new StringBuilder();
                        errorMsg.AppendLine(ex.Detail.ToString());

                        var exception = ex.Detail.InnerException;

                        while (exception != null)
                        {
                            errorMsg.AppendLine(exception.ToString());
                            exception = exception.InnerException;
                        }

                        Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(errorMsg.ToString(), ApplicationLogCategory.General, (int)Priority.High, (int)GeneralApplicationLogEvent.DispatcherUnhandledException, System.Diagnostics.TraceEventType.Error);
                    }
                }
                else
                {
                    Exception ex = e.Exception;
                    Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(ex.ToString(), ApplicationLogCategory.General, (int)Priority.High, (int)GeneralApplicationLogEvent.DispatcherUnhandledException, System.Diagnostics.TraceEventType.Error);
                }

                MessageBoxResult result = MessageBox.Show("An error has occurred. Would you like to continue ?", Messages.MSG_ERROR_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.No)
                {
                    Application.Current.Shutdown(-1);
                }
                e.Handled = true;
            }
        }
    }
}
