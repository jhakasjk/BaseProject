using AutoMapper;
using CoreEntities.Enums;
using CoreEntities.Interfaces;
using CoreEntities.Models;
using DataAccessLayer.Model.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Managers
{
    public class DocumentManager : BaseManager, IDocumentManager
    {
        IMapper documentMapper;
        IMapper documentDbToModel;
        public DocumentManager()
        {
            var configd = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DocumentModel, Document>();
            });
            documentMapper = configd.CreateMapper();
            var configToModel = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Document, DocumentModel>();
            });
            documentDbToModel = configToModel.CreateMapper();
        }

        /// <summary>
        /// THIS METHOD IS USED TO
        /// SAVE DOCUMENT UPLOADED BY USER
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        ActionOutput<DocumentModel> IDocumentManager.SaveDocument(DocumentModel document)
        {
            var documentToSave = documentMapper.Map<DocumentModel, Document>(document);
            documentToSave = Context.Documents.Add(documentToSave);
            SaveChanges();
            document.Id = documentToSave.Id;
            document.DownloadBinary = null;
            return new ActionOutput<DocumentModel>
            {
                Object = document,
                Status = ActionStatus.Successfull
            };
        }
        ActionOutput<DocumentModel> IDocumentManager.DownloadDocument(int documentId)
        {
            var document = Context.Documents.Where(d => d.Id == documentId).FirstOrDefault();
            if (document != null)
            {
                DocumentModel savedDoc = documentDbToModel.Map<Document, DocumentModel>(document);

                return new ActionOutput<DocumentModel>
                {
                    Object = savedDoc,
                    Status = ActionStatus.Successfull
                };
            }
            return new ActionOutput<DocumentModel>
            {
                Message = "Document NotFound",
                Status = ActionStatus.Error
            };
        }
    }
}
