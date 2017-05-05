using CoreEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Interfaces
{
    public interface IDocumentManager
    {
        /// <summary>
        /// Save file byte array into database document table
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        ActionOutput<DocumentModel> SaveDocument(DocumentModel document);
        /// <summary>
        /// THIS METHOD IS USED TO DOWNLOAD
        /// DOCUMENT AVAILABLE
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        ActionOutput<DocumentModel> DownloadDocument(int documentId);
    }
}
