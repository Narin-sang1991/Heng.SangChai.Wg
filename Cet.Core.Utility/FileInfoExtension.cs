using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Core.Utility
{
    public class FileInfoExtension
    {
        IUnityContainer Container { get; set; }

        public FileInfoExtension(IUnityContainer container)
        {
            this.Container = container;
        }

        #region FileInfo Utility Methods
        public string GetValidatedFileName(string filePath, string fileName, string fileExtention)
        {
            string result = fileName;
            int lengthName = fileName.Length - fileExtention.Length;
            var nameOnly = fileName.Substring(0, lengthName);
            int i = 0;

            while (IsExistingFile(result, filePath))
            {
                result = string.Format(nameOnly + "({0})" + fileExtention, ++i);
            }
            return result;
        }


        public bool IsExistingFile(string fileName, string filePath)
        {
            var currentDirectory = System.Environment.CurrentDirectory;
            var pathFile = currentDirectory + filePath + fileName;
            var isExist = System.IO.File.Exists(pathFile);
            return isExist;
        }

        public byte[] ReadFile(string fileName, string filePath)
        {
            string filePathFullName = System.IO.Path.Combine(filePath, fileName);

            if (!System.IO.File.Exists(filePathFullName))
                throw new FaultException<Cet.Core.DataValidationException>(new DataValidationException() { Message = "File not found!" });

            System.IO.FileStream stream = new System.IO.FileStream(filePathFullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            byte[] byteData = new byte[stream.Length];
            stream.Read(byteData, 0, Convert.ToInt32(stream.Length));
            stream.Close();

            return byteData;
        }

        public string WriteFile(byte[] byteData, string filePathFullName)
        {
            string result = "";
            var directory = System.IO.Path.GetDirectoryName(filePathFullName);
            try
            {
                if (!System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);
                }

                System.IO.File.WriteAllBytes(filePathFullName, byteData);
                result = filePathFullName;
            }
            catch (System.IO.IOException iex)
            {
                throw new FaultException<Cet.Core.DataValidationException>(new DataValidationException() { Message = string.Format("{0} : upload file path = {1}", iex.Message, filePathFullName) });
            }
            catch (Exception ex)
            {
                throw new FaultException<Cet.Core.DataValidationException>(new DataValidationException() { Message = ex.Message });
            }
            return result;
        }

        public void Delete(string fileName, string filePath)
        {
            try
            {
                string fileFullPath = System.IO.Path.Combine(filePath, fileName);
                System.IO.File.Delete(fileFullPath);
            }
            catch (System.IO.IOException iex)
            {
                throw new FaultException<Cet.Core.DataValidationException>(new DataValidationException() { Message = string.Format("{0} : delete file path = {1}", iex.Message, fileName) });
            }
            catch (Exception ex)
            {
                throw new FaultException<Cet.Core.DataValidationException>(new DataValidationException() { Message = ex.Message });
            }
        }

        public String ReadTxtFile(string filePathFullName)
        {
            var directory = System.IO.Path.GetDirectoryName(filePathFullName);
            try
            {
                if (System.IO.Directory.Exists(directory))
                {
                    string text = System.IO.File.ReadAllText(filePathFullName);
                    return text;
                }
                else
                    return null;
            }
            catch (System.IO.IOException iex)
            {
                throw new FaultException<Cet.Core.DataValidationException>(new DataValidationException() { Message = string.Format("{0} : read file path = {1}", iex.Message, filePathFullName) });
            }
            catch (Exception ex)
            {
                throw new FaultException<Cet.Core.DataValidationException>(new DataValidationException() { Message = ex.Message });
            }
        }

        public String[] ReadTxtLineFile(string filePathFullName)
        {
            var directory = System.IO.Path.GetDirectoryName(filePathFullName);
            try
            {
                if (System.IO.Directory.Exists(directory))
                {
                    string[] textLines = System.IO.File.ReadAllLines(filePathFullName);
                    return textLines;
                }
                else
                    return null;
            }
            catch (System.IO.IOException iex)
            {
                throw new FaultException<Cet.Core.DataValidationException>(new DataValidationException() { Message = string.Format("{0} : read file path = {1}", iex.Message, filePathFullName) });
            }
            catch (Exception ex)
            {
                throw new FaultException<Cet.Core.DataValidationException>(new DataValidationException() { Message = ex.Message });
            }
        }

        #endregion

    }
}
