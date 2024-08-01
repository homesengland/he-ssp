using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.CRM.Plugins.Handlers.ISPs;
using HE.CRM.Plugins.Handlers.LoanApplications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using PwC.Base.Tests.Plugins.Handlers;

namespace HE.CRM.Plugins.Tests.Handlers.ISP
{
    [TestClass]
    public class CreateRecordsOnSentForApprovalChangeHandlerTests : CrmEntityHandlerTestBase<invln_ISP, CreateRecordsOnSentForApprovalChangeHandler, DataverseContext>
    {
        private readonly Guid _ispId = Guid.NewGuid();
        private readonly Guid _dESTeamId = Guid.NewGuid();
        private readonly Guid _hoFTeamId = Guid.NewGuid();
        private readonly Guid _cROTeamId = Guid.NewGuid();
        private readonly Guid _desApprovalId = Guid.NewGuid();
        private readonly Guid _hoFApprovalId = Guid.NewGuid();

        private invln_ISP _isp = new invln_ISP();
        private Team _desTeam = new Team();
        private Team _hoFTeam = new Team();
        private Team _cROTeam = new Team();
        private invln_reviewapproval _desApproval = new invln_reviewapproval();
        private invln_reviewapproval _hofApproval = new invln_reviewapproval();

        [TestInitialize]
        public void Initialize()
        {
            base.InitializeTest();

            _isp.Id = _ispId;
            _isp.invln_ApprovalLevelNew = new OptionSetValue((int)invln_ApprovalLevel.HoF);

            _desTeam.Id = _dESTeamId;
            _desTeam.Name = "DES Team";

            _hoFTeam.Id = _hoFTeamId;
            _hoFTeam.Name = "HoF Team";

            _cROTeam.Id = _cROTeamId;
            _cROTeam.Name = "CRO Team";

            _desApproval.Id = _desApprovalId;
            _desApproval.invln_ispid = _isp.ToEntityReference();
            _desApproval.invln_reviewerapprover = new OptionSetValue((int)invln_reviewerapproverset.DESReview);
            _desApproval.invln_name = "DES team review of ISP";
            _hofApproval.Id = _hoFApprovalId;
            _hofApproval.invln_ispid = _isp.ToEntityReference();
            _hofApproval.invln_reviewerapprover = new OptionSetValue((int)invln_reviewerapproverset.HoFApproval);
            _hofApproval.invln_name = "HoF team approval of ISP";
        }

        [TestMethod]
        public void CanWork_Success()
        {
            Target = new invln_ISP
            {
                Id = _ispId,
                invln_SendforApproval = true
            };

            PreImage = new invln_ISP
            {
                Id = _ispId,
                invln_SendforApproval = false
            };

            PostImage = new invln_ISP
            {
                Id = _ispId,
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
                Id = _ispId,
                invln_SendforApproval = false
            };

            PreImage = new invln_ISP
            {
                Id = _ispId,
                invln_SendforApproval = true
            };

            PostImage = new invln_ISP
            {
                Id = _ispId,
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
                Id = _ispId,
                invln_SendforApproval = true
            };

            PreImage = new invln_ISP
            {
                Id = _ispId,
                invln_SendforApproval = false,
                invln_ApprovalLevelNew = new OptionSetValue((int)invln_ApprovalLevel.HoF)
            };

            PostImage = new invln_ISP
            {
                Id = _ispId,
                invln_SendforApproval = true,
                invln_ApprovalLevelNew = new OptionSetValue((int)invln_ApprovalLevel.HoF)
            };

            fakedContext.Initialize(
                new List<Entity>()
                {
                    _isp,
                    _desTeam,
                    _hoFTeam,
                    _cROTeam
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
                Id = _ispId,
                invln_SendforApproval = true
            };

            PreImage = new invln_ISP
            {
                Id = _ispId,
                invln_SendforApproval = false,
                invln_ApprovalLevelNew = new OptionSetValue((int)invln_ApprovalLevel.HoF)
            };

            PostImage = new invln_ISP
            {
                Id = _ispId,
                invln_SendforApproval = true,
                invln_ApprovalLevelNew = new OptionSetValue((int)invln_ApprovalLevel.CRO)
            };

            fakedContext.Initialize(
                new List<Entity>()
                {
                    _isp,
                    _desTeam,
                    _hoFTeam,
                    _cROTeam
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
                Id = _ispId,
                invln_SendforApproval = true,
                invln_ISPId = _ispId
            };

            PreImage = new invln_ISP
            {
                Id = _ispId,
                invln_SendforApproval = false,
                invln_ApprovalLevelNew = new OptionSetValue((int)invln_ApprovalLevel.HoF)
            };

            PostImage = new invln_ISP
            {
                Id = _ispId,
                invln_SendforApproval = true,
                invln_ApprovalLevelNew = new OptionSetValue((int)invln_ApprovalLevel.HoF)
            };

            fakedContext.Initialize(
                new List<Entity>()
                {
                    _isp,
                    _desTeam,
                    _hoFTeam,
                    _cROTeam,
                    _desApproval,
                    _hofApproval,
                    _desTeam,
                    _hoFTeam,
                    _cROTeam,
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
