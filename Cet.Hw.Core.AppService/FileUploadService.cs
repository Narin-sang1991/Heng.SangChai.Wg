using Cet.Hw.Core.Domain;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cet.Hw.Core.AppServiceContract;
using Cet.Core.Data;
using Cet.EntityFramework.Adaptor;
using System.Data.Entity;
using System.ServiceModel;
using Cet.Core;
using System.Data.Entity.Infrastructure;
using Cet.Core.Utility;

namespace Cet.Hw.Core.AppService
{
    public class FileUploadService
    {
        IUnityContainer Container { get; set; }

        private FileInfoExtension fileInfoExtension { get; set; }

        public FileUploadService(IUnityContainer container)
        {
            this.Container = container;
            fileInfoExtension = container.Resolve<FileInfoExtension>();
        }

        [Dependency]
        public IFileInfoIntroductory FileInfoIntroductory { get; set; }

        public FileInfoUploadData GetFileInfoByRelation(Guid relationId, FileInfoType relationType)
        {
            var criteria = new FileInfoCriteria();
            criteria.RelationId = relationId;
            criteria.RelationType = relationType;
            var queryfileInfo = QueryFileInfo(criteria);
            var result = new FileInfoUploadData();
            result = queryfileInfo.Select(FileInfo2FileInfoUploadData).FirstOrDefault();
            return result;
        }

        public IList<FileInfoUploadData> FindFileInfo(FileInfoCriteria criteria, SortingCriteria sortingCriteria, PagingCriteria pagingCriteria)
        {
            var queryfileInfo = QueryFileInfo(criteria);

            if (sortingCriteria != null) queryfileInfo = queryfileInfo.OrderBy(sortingCriteria);
            if (pagingCriteria != null) queryfileInfo = queryfileInfo.Page(pagingCriteria);

            var datas = new List<FileInfoUploadData>();
            datas = queryfileInfo.Select(FileInfo2FileInfoUploadData).ToList();
            return datas;
        }

        Func<CoreFileInfo, FileInfoUploadData> FileInfo2FileInfoUploadData = fileInfo => new FileInfoUploadData()
        {
            Id = fileInfo.Id,
            FileName = fileInfo.FileName,
            Extension = fileInfo.CoreFileExtension.FileType,
            RelationId = fileInfo.RelationId,
            RelationType = (FileInfoType)fileInfo.RelationTypeValue
        };

        public int CountFileInfo(FileInfoCriteria criteria)
        {
            return QueryFileInfo(criteria).Count();
        }

        private IQueryable<CoreFileInfo> QueryFileInfo(FileInfoCriteria criteria)
        {
            var result = FileInfoIntroductory.GetAll().Where(FileInfoSpecification.GetSpecification01(criteria))
                .Include(t => t.CoreFileExtension);
            return result;
        }

        public Guid SaveFileInfo(FileInfoUploadData data, string filePath)
        {
            CoreFileInfo fileInfo = (data.Id.HasValue && data.Id != Guid.Empty ? FileInfoIntroductory.Get(data.Id.Value) : null);

            if (fileInfo == null)
            {
                fileInfo = new CoreFileInfo();
                UpdateFileInfoInternal(fileInfo, data);
                FileInfoIntroductory.Add(fileInfo);
            }
            else
            {
                UpdateFileInfoInternal(fileInfo, data);
            }
            var filePathFullName = System.IO.Path.Combine(filePath, fileInfo.Id.ToString());
            fileInfoExtension.WriteFile(data.FileByteData, filePath);

            return fileInfo.Id;
        }

        public void RemoveFileInfo(Guid relationId, string filePath)
        {
            CoreFileInfo fileInfo = FileInfoIntroductory.GetAll().Include(t => t.CoreFileExtension).Where(t => t.RelationId == relationId).FirstOrDefault();
            var fileName = string.Format(fileInfo.Id.ToString() + fileInfo.CoreFileExtension.FileType);
            FileInfoIntroductory.Remove(fileInfo);
            try
            {
                fileInfoExtension.Delete(fileName, filePath);
            }
            catch (DbUpdateException ex)
            {
                throw new FaultException<DataValidationException>(new DataValidationException() { Message = AppServiceContract.Resources.Messages.ErrorChangeData });
            }
        }

        private void UpdateFileInfoInternal(CoreFileInfo fileInfo, FileInfoUploadData data)
        {
            var fileExtension = FileInfoIntroductory.GetAll().Select(t => t.CoreFileExtension)
                .Where(t => t.FileType == data.Extension).FirstOrDefault();

            fileInfo.UpdateInternal(data.FileName, fileExtension);
            fileInfo.SetRelation(data.RelationId, (int)data.RelationType);
            fileInfo.SetDescription(string.IsNullOrWhiteSpace(data.Description) ? null : data.Description);
        }

    }
}
