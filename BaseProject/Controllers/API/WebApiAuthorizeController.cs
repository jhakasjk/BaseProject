using BaseProject.Attributes;
using CoreEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BaseProject.Controllers.API
{
    [ApiException, ApiAuthorizeUser, Validate]
    public class WepApiAuthorizeController : BaseApiController
    {
        //
        // GET: /WebApi/
        public UserDetails User { get; set; }
    }
}
