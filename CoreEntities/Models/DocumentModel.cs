using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntities.Models
{
    #region Document Related Model
    /// <summary>
    /// This model is used for
    /// primary document model where
    /// we are storing document details
    /// </summary>
    public class DocumentModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string SavedName { get; set; }
        public byte[] DownloadBinary { get; set; }
        public string Mime { get; set; }
    }

    #endregion
}
