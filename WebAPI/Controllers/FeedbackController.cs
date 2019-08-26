using NoorCare.Repository;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Entity;
using WebAPI.Repository;

namespace WebAPI.Controllers
{
    public class FeedbackController : ApiController
    {
        IFeedbackRepository _feedbackRepo = RepositoryFactory.Create<IFeedbackRepository>(ContextTypes.EntityFramework);

        [Route("api/feedback/getAllFeedback")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/Feedback
        public HttpResponseMessage GetAllFeedback()
        {            
            var result = _feedbackRepo.GetAll().ToList();

            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/feedback/getdetail/{feedbackId}")]
        [HttpGet]
        [AllowAnonymous]
        // GET: api/feedback/5
        public HttpResponseMessage GetDetail(string feedbackId)
        {
            var result = _feedbackRepo.Find(x => x.FeedbackID == feedbackId).FirstOrDefault();
            return Request.CreateResponse(HttpStatusCode.Accepted, result);            
        }

        [Route("api/feedback/register")]
        [HttpPost]
        [AllowAnonymous]
        // POST: api/feedback
        public HttpResponseMessage Register(Feedback obj)
        {
            if(ModelState.IsValid)
            {
                Random rd = new Random(987612345);
                var _feedbackId ="F_"+ rd.Next();
                obj.FeedbackID = _feedbackId;
                var _feedbackCreated = _feedbackRepo.Insert(obj);
                return Request.CreateResponse(HttpStatusCode.Accepted, obj.FeedbackID);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest,"");
        }


        [Route("api/feedback/update")]
        [HttpPut]
        [AllowAnonymous]
        // PUT: api/Feedback/5
        public HttpResponseMessage Update(Feedback obj)
        {
            int tbleId = getTableId(obj.FeedbackID);
            obj.Id = tbleId;
            var result = _feedbackRepo.Update(obj);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        [Route("api/feedback/delete/{feedbackid}")]
        [HttpDelete]
        [AllowAnonymous]
        // DELETE: api/Feedback/5
        public HttpResponseMessage Delete(string feedbackid)
        {
            int tbleId= getTableId(feedbackid);

            var result = _feedbackRepo.Delete(tbleId);
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }

        private int getTableId(string feedbackId)
        {
            IFeedbackRepository _doctorRepo = RepositoryFactory.Create<IFeedbackRepository>(ContextTypes.EntityFramework);
            var result = _doctorRepo.Find(x => x.FeedbackID == feedbackId).FirstOrDefault();

            return result.Id;
        }
    }
}
