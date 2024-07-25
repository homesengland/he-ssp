using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.CRM.Plugins.Handlers.ISPs;
using HE.CRM.Plugins.Handlers.LoanApplications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using PwC.Base.Tests.Plugins.Handlers;

namespace HE.CRM.Plugins.Tests.Plugins.ISP
{
    [TestClass]
    public class CreateRecordsOnSentForApprovalChangeHandlerTests : CrmEntityHandlerTestBase<invln_ISP, CreateRecordsOnSentForApprovalChangeHandler, DataverseContext>
    {
        private readonly Guid _IspId = Guid.NewGuid();
        private readonly Guid _DESTeamId = Guid.NewGuid();
        private readonly Guid _HoFTeamId = Guid.NewGuid();
        private readonly Guid _CROTeamId = Guid.NewGuid();
        private readonly Guid _DesApprovalId = Guid.NewGuid();
        private readonly Guid _HoFApprovalId = Guid.NewGuid();

        private invln_ISP isp = new invln_ISP();
        private Team DesTeam = new Team();
        private Team HoFTeam = new Team();
        private Team CROTeam = new Team();
        private invln_reviewapproval desApproval = new invln_reviewapproval();
        private invln_reviewapproval hofApproval = new invln_reviewapproval();

        [TestInitialize]
        public void Initialize()
        {
            base.InitializeTest();

            isp.Id = _IspId;
            isp.invln_ApprovalLevelNew = new OptionSetValue((int)invln_ApprovalLevel.HoF);

            DesTeam.Id = _DESTeamId;
            DesTeam.Name = "DES Team";

            HoFTeam.Id = _HoFTeamId;
            HoFTeam.Name = "HoF Team";

            CROTeam.Id = _CROTeamId;
            CROTeam.Name = "CRO Team";

            desApproval.Id = _DesApprovalId;
            desApproval.invln_ispid = isp.ToEntityReference();
            desApproval.invln_reviewerapprover = new OptionSetValue((int)invln_reviewerapproverset.DESReview);
            desApproval.invln_name = "DES team review of ISP";
            hofApproval.Id = _HoFApprovalId;
            hofApproval.invln_ispid = isp.ToEntityReference();
            hofApproval.invln_reviewerapprover = new OptionSetValue((int)invln_reviewerapproverset.HoFApproval);
            hofApproval.invln_name = "HoF team approval of ISP";
        }

        [TestMethod]
        public void CanWork_Success()
        {
            Target = new invln_ISP
            {
                Id = Guid.NewGuid(),
                invln_SendforApproval = true
            };

            PreImage = new invln_ISP
            {
                Id = Guid.NewGuid(),
                invln_SendforApproval = false
            };

            PostImage = new invln_ISP
            {
                Id = Guid.NewGuid(),
                invln_SendforApproval = true
            };

            Asset("Update", (int)StageEnum.PostOperation);

            var result = handler.CanWork();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanWork_Fail()
        {
            Target = new invln_ISP
            {
                Id = Guid.NewGuid(),
                invln_SendforApproval = false
            };

            PreImage = new invln_ISP
            {
                Id = Guid.NewGuid(),
                invln_SendforApproval = true
            };

            PostImage = new invln_ISP
            {
                Id = Guid.NewGuid(),
                invln_SendforApproval = false
            };

            Asset("Update", (int)StageEnum.PostOperation);

            var result = handler.CanWork();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DoWork_FirstApproval_HOF()
        {
            Target = new invln_ISP
            {
                Id = Guid.NewGuid(),
                invln_SendforApproval = true
            };

            PreImage = new invln_ISP
            {
                Id = Guid.NewGuid(),
                invln_SendforApproval = false,
                invln_ApprovalLevelNew = new OptionSetValue((int)invln_ApprovalLevel.HoF)
            };

            PostImage = new invln_ISP
            {
                Id = Guid.NewGuid(),
                invln_SendforApproval = true,
                invln_ApprovalLevelNew = new OptionSetValue((int)invln_ApprovalLevel.HoF)
            };

            fakedContext.Initialize(
                new List<Entity>()
                {
                    isp,
                    DesTeam,
                    HoFTeam,
                    CROTeam
                }
                );

            Asset("Update", (int)StageEnum.PreOperation);
            handler.DoWork();

            var result = fakedContext.CreateQuery<invln_reviewapproval>().ToList();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("DES team review of ISP", result.Where(x => x.invln_reviewerapprover.Value == (int)invln_reviewerapproverset.DESReview).FirstOrDefault().invln_name);
            Assert.AreEqual("HoF team approval of ISP", result.Where(x => x.invln_reviewerapprover.Value == (int)invln_reviewerapproverset.HoFApproval).FirstOrDefault().invln_name);
        }

        [TestMethod]
        public void DoWork_FirstApproval_CRO()
        {
            Target = new invln_ISP
            {
                Id = Guid.NewGuid(),
                invln_SendforApproval = true
            };

            PreImage = new invln_ISP
            {
                Id = Guid.NewGuid(),
                invln_SendforApproval = false,
                invln_ApprovalLevelNew = new OptionSetValue((int)invln_ApprovalLevel.HoF)
            };

            PostImage = new invln_ISP
            {
                Id = Guid.NewGuid(),
                invln_SendforApproval = true,
                invln_ApprovalLevelNew = new OptionSetValue((int)invln_ApprovalLevel.CRO)
            };

            fakedContext.Initialize(
                new List<Entity>()
                {
                    isp,
                    DesTeam,
                    HoFTeam,
                    CROTeam
                }
                );

            Asset("Update", (int)StageEnum.PreOperation);
            handler.DoWork();

            var result = fakedContext.CreateQuery<invln_reviewapproval>().ToList();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("DES team review of ISP", result.Where(x => x.invln_reviewerapprover.Value == (int)invln_reviewerapproverset.DESReview).FirstOrDefault().invln_name);
            Assert.AreEqual("HoF team approval of ISP", result.Where(x => x.invln_reviewerapprover.Value == (int)invln_reviewerapproverset.HoFReview).FirstOrDefault().invln_name);
        }

        [TestMethod]
        public void DoWork_NextApproval_HOF()
        {
            Target = new invln_ISP
            {
                Id = _IspId,
                invln_SendforApproval = true,
                invln_ISPId = _IspId
            };

            PreImage = new invln_ISP
            {
                Id = _IspId,
                invln_SendforApproval = false,
                invln_ApprovalLevelNew = new OptionSetValue((int)invln_ApprovalLevel.HoF)
            };

            PostImage = new invln_ISP
            {
                Id = _IspId,
                invln_SendforApproval = true,
                invln_ApprovalLevelNew = new OptionSetValue((int)invln_ApprovalLevel.HoF)
            };

            fakedContext.Initialize(
                new List<Entity>()
                {
                    isp,
                    DesTeam,
                    HoFTeam,
                    CROTeam,
                    desApproval,
                    hofApproval
                }
                );

            Asset("Update", (int)StageEnum.PreOperation);
            handler.DoWork();

            var result = fakedContext.CreateQuery<invln_reviewapproval>().ToList();
            var oldApprovals = fakedContext.CreateQuery<invln_reviewapproval>().Where(x => x.invln_status.Value == (int)invln_StatusReviewApprovalSet.NotRequired).ToList();
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(2, oldApprovals.Count);
            Assert.AreEqual("DES team review of ISP", result.Where(x => x.invln_reviewerapprover.Value == (int)invln_reviewerapproverset.DESReview).FirstOrDefault().invln_name);
            Assert.AreEqual("HoF team approval of ISP", result.Where(x => x.invln_reviewerapprover.Value == (int)invln_reviewerapproverset.HoFApproval).FirstOrDefault().invln_name);
        }
    }
}
