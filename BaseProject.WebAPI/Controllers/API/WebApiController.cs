using BaseProject.WebAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BaseProject.WebAPI.Controllers.API
{
    [ApiException, ApiAuthorize, Validate]
    public class WebApiController : BaseApiController
    {
    }
}
