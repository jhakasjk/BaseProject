using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreEntities.Models;
using System.IO;
using CoreEntities.Interfaces;
using BaseProject.Attributes;

namespace BaseProject.Controllers
{
    public class DocumentController : BaseController
    {
        #region Variable Declaration
        private readonly IDocumentManager _documentManager;
        #endregion

        public DocumentController(IErrorLogManager errorLogManager, IDocumentManager documentManager) : base(errorLogManager)
        {
            _documentManager = documentManager;
        }

        [Public]
        public ActionResult _FileUploader(int id)
        {
            return View(id);
        }

        [Public]
        public ActionResult Upload(HttpPostedFileBase Document)
        {
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(Document.InputStream))
            {
                fileData = binaryReader.ReadBytes(Document.ContentLength);
                DocumentModel model = new DocumentModel();
                model.DisplayName = Document.FileName.Substring(Document.FileName.LastIndexOf(@"\") + 1);
                model.SavedName = Guid.NewGuid().ToString() + Document.FileName.Substring(Document.FileName.LastIndexOf('.'));
                model.DownloadBinary = fileData;
                model.Mime = Document.ContentType;
                return Json(_documentManager.SaveDocument(model), JsonRequestBehavior.AllowGet);
            }
        }

        [Public]
        public FileResult Download(int documentId)
        {
            var doc = _documentManager.DownloadDocument(documentId);
            return File(doc.Object.DownloadBinary, doc.Object.Mime, doc.Object.DisplayName);
        }

        [Public]
        public ActionResult Index()
        {
            return View();
        }
    }
}