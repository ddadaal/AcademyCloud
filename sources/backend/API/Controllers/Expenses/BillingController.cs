using AcademyCloud.API.Models.Expenses.Billing;
using AcademyCloud.API.Utils;
using AcademyCloud.Expenses.Protos.Common;
using AcademyCloud.Identity.Protos.Interop;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Shared;

namespace AcademyCloud.API.Controllers.Expenses
{
    [Route("/expenses/billing")]
    [ApiController]
    public class BillingController : Controller
    {
        private readonly ServiceClientFactory factory;

        public BillingController(ServiceClientFactory factory)
        {
            this.factory = factory;
        }

        private CurrentAllocatedBilling FromGrpc(AcademyCloud.Expenses.Protos.Billing.CurrentAllocatedBilling billing, IDictionary<string, string> nameMap)
        {
            return new CurrentAllocatedBilling
            {
                SubjectId = billing.SubjectId,
                SubjectName = nameMap[billing.SubjectId],
                Amount = billing.Amount,
                NextDue = billing.NextDue.ToDateTime(),
                PayerId = billing.PayerId,
                PayerName = nameMap[billing.PayerId],
                Resources = billing.Quota,
            };

        }

        [HttpGet("allocated/current/{subjectType}")]
        public async Task<BillingsResponse<CurrentAllocatedBilling>> GetCurrentAllocatedBillings([FromRoute] SubjectType subjectType)
        {
            var resp = await (await factory.GetBillingClient())
                .GetCurrentAllocatedBillingsAsync(new AcademyCloud.Expenses.Protos.Billing.GetCurrentAllocatedBillingsRequest
                {
                    SubjectType = subjectType
                });

            // get names
            var subjects = new List<GetNamesRequest.Types.Subject>(resp.Billings.Count * 2);
            resp.Billings.ForEach(x =>
            {
                subjects.Add(new GetNamesRequest.Types.Subject() { Type = GetNamesRequest.Types.SubjectType.User, Id = x.PayerId });
                subjects.Add(new GetNamesRequest.Types.Subject() { Type = (GetNamesRequest.Types.SubjectType)subjectType, Id = x.SubjectId });
            });

            var namesResp = await (await factory.GetIdentityInteropClientAsync())
                .GetNamesAsync(new GetNamesRequest { Subjects = { subjects } });

            return new BillingsResponse<CurrentAllocatedBilling>
            {
                Billings = resp.Billings.Select(x => FromGrpc(x, namesResp.IdNameMap)),
            };
        }

        [HttpGet("allocated/current/{subjectType}/{id}")]
        public async Task<BillingResponse<CurrentAllocatedBilling>> GetCurrentAllocatedBilling([FromRoute] SubjectType subjectType, [FromRoute] string id)
        {
            var resp = await (await factory.GetBillingClient())
                .GetCurrentAllocatedBillingAsync(new AcademyCloud.Expenses.Protos.Billing.GetCurrentAllocatedBillingRequest
                {
                    Id = id,
                    SubjectType = subjectType
                });

            // get names
            var namesResp = await (await factory.GetIdentityInteropClientAsync())
                .GetNamesAsync(new GetNamesRequest
                {
                    Subjects =
                    {
                        new GetNamesRequest.Types.Subject() { Type = GetNamesRequest.Types.SubjectType.User, Id = resp.Billing.PayerId },
                        new GetNamesRequest.Types.Subject() { Type = (GetNamesRequest.Types.SubjectType)subjectType, Id = resp.Billing.SubjectId },
                    }
                });

            return new BillingResponse<CurrentAllocatedBilling>
            {
                Billing = FromGrpc(resp.Billing, namesResp.IdNameMap)
            };
        }
        private CurrentUsedBilling FromGrpc(AcademyCloud.Expenses.Protos.Billing.CurrentUsedBilling billing, IDictionary<string, string> nameMap)
        {
            return new CurrentUsedBilling
            {
                SubjectId = billing.SubjectId,
                SubjectName = nameMap[billing.SubjectId],
                Amount = billing.Amount,
                NextDue = billing.NextDue.ToDateTime(),
                Resources = billing.Resources,
            };

        }


        [HttpGet("used/current/{subjectType}")]
        public async Task<BillingsResponse<CurrentUsedBilling>> GetCurrentUsedBillings([FromRoute] SubjectType subjectType)
        {
            var resp = await (await factory.GetBillingClient())
                .GetCurrentUsedBillingsAsync(new AcademyCloud.Expenses.Protos.Billing.GetCurrentUsedBillingsRequest
                {
                    SubjectType = subjectType
                });

            // get names
            var namesResp = await (await factory.GetIdentityInteropClientAsync())
                .GetNamesAsync(new GetNamesRequest
                {
                    Subjects = {
                        resp.Billings.Select(x => new GetNamesRequest.Types.Subject { Type = (GetNamesRequest.Types.SubjectType) subjectType, Id = x.SubjectId })
                    }
                });

            return new BillingsResponse<CurrentUsedBilling>
            {
                Billings = resp.Billings.Select(x => FromGrpc(x, namesResp.IdNameMap)),
            };
        }

        [HttpGet("used/current/{subjectType}/{id}")]
        public async Task<BillingResponse<CurrentUsedBilling>> GetCurrentUsedBilling([FromRoute] SubjectType subjectType, [FromRoute] string id)
        {

            var resp = await (await factory.GetBillingClient())
                .GetCurrentUsedBillingAsync(new AcademyCloud.Expenses.Protos.Billing.GetCurrentUsedBillingRequest
                {
                    Id = id,
                    SubjectType = subjectType
                });

            // get names
            var namesResp = await (await factory.GetIdentityInteropClientAsync())
                .GetNamesAsync(new GetNamesRequest
                {
                    Subjects =
                    {
                        new GetNamesRequest.Types.Subject() { Type = (GetNamesRequest.Types.SubjectType)subjectType, Id = resp.Billing.SubjectId },
                    }
                });

            return new BillingResponse<CurrentUsedBilling>
            {
                Billing = FromGrpc(resp.Billing, namesResp.IdNameMap)
            };
        }


        [HttpGet("allocated/history/{subjectType}/{id}")]
        public async Task<BillingsResponse<HistoryAllocatedBilling>> GetHistoryAllocatedBillings([FromRoute] SubjectType subjectType, [FromRoute] string id)
        {
            var resp = await (await factory.GetBillingClient())
                .GetHistoryAllocatedBillingsAsync(new AcademyCloud.Expenses.Protos.Billing.GetHistoryAllocatedBillingsRequest
                {
                    Id = id,
                    SubjectType = subjectType,
                });

            // get names
            var namesResp = await (await factory.GetIdentityInteropClientAsync())
                .GetNamesAsync(new GetNamesRequest
                {
                    Subjects =
                    {
                        resp.Billings.Select(x => new GetNamesRequest.Types.Subject { Type = (GetNamesRequest.Types.SubjectType) subjectType, Id = id })
                    }
                });

            return new BillingsResponse<HistoryAllocatedBilling>
            {
                Billings = resp.Billings.Select(x => new HistoryAllocatedBilling
                {
                    Id = x.Id,
                    EndTime = x.EndTime.ToDateTime(),
                    StartTime = x.StartTime.ToDateTime(),
                    Amount = x.Amount,
                    PayerId = x.PayerId,
                    PayerName = namesResp.IdNameMap[x.PayerId],
                    Resources = x.Quota,
                })
            };
        }

        [HttpGet("used/history/{subjectType}/{id}")]
        public async Task<BillingsResponse<HistoryUsedBilling>> GetHistoryUsedBillings([FromRoute] SubjectType subjectType, [FromRoute] string id)
        {

            var resp = await (await factory.GetBillingClient())
                .GetHistoryUsedBillingsAsync(new AcademyCloud.Expenses.Protos.Billing.GetHistoryUsedBillingsRequest
                {
                    Id = id,
                    SubjectType = subjectType,
                });

            // get names
            var namesResp = await (await factory.GetIdentityInteropClientAsync())
                .GetNamesAsync(new GetNamesRequest
                {
                    Subjects =
                    {
                        resp.Billings.Select(x => new GetNamesRequest.Types.Subject { Type = (GetNamesRequest.Types.SubjectType) subjectType, Id = id })
                    }
                });

            return new BillingsResponse<HistoryUsedBilling>
            {
                Billings = resp.Billings.Select(x => new HistoryUsedBilling
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Resources = x.Resources,
                    StartTime = x.StartTime.ToDateTime(),
                    EndTime = x.EndTime.ToDateTime(),
                })
            };
        }
    }
}
