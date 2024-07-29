using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.CRM.Plugins.Handlers.ReviewsApprovals;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using PwC.Base.Tests.Plugins.Handlers;

namespace HE.CRM.Plugins.Tests.Handlers.ReviewsApprovals
{
    [TestClass]
    public class IspStatusOnReviewApprovalStatusChangeHandlerTests : CrmEntityHandlerTestBase<invln_reviewapproval, IspStatusOnReviewApprovalStatusChangeHandler, DataverseContext>
    {
        private readonly Guid _raDesId = Guid.NewGuid();
        private readonly Guid _raHoFId = Guid.NewGuid();
        private readonly Guid _ispId = Guid.NewGuid();
        private readonly Guid _dESTeamId = Guid.NewGuid();
        private readonly Guid _hoFTeamId = Guid.NewGuid();
        private readonly Guid _cROTeamId = Guid.NewGuid();

        private invln_reviewapproval _raDes = new invln_reviewapproval();
        private invln_reviewapproval _raHoF = new invln_reviewapproval();
        private invln_ISP _isp = new invln_ISP();
        private Team _desTeam = new Team();
        private Team _hoFTeam = new Team();
        private Team _cROTeam = new Team();

        [TestInitialize]
        public void Initialize()
        {
            base.InitializeTest();

            _desTeam.Id = _dESTeamId;
            _desTeam.Name = "DES Team";

            _hoFTeam.Id = _hoFTeamId;
            _hoFTeam.Name = "HoF Team";

            _cROTeam.Id = _cROTeamId;
            _cROTeam.Name = "CRO Team";

            _isp.Id = _ispId;
            _isp.invln_ApprovalLevelNew = new OptionSetValue((int)invln_ApprovalLevel.HoF);

            _raDes.Id = _raDesId;
            _raDes.invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending);
            _raDes.invln_ispid = _isp.ToEntityReference();
            _raDes.invln_reviewerapprover = new OptionSetValue((int)invln_reviewerapproverset.DESReview);
            _raDes["CreatedOn"] = DateTime.Now.AddMinutes(-1);
            _raDes.OwnerId = _desTeam.ToEntityReference();

            _raHoF.Id = _raHoFId;
            _raHoF.invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending);
            _raHoF.invln_ispid = _isp.ToEntityReference();
            _raHoF.invln_reviewerapprover = new OptionSetValue((int)invln_reviewerapproverset.HoFReview);
            _raHoF["CreatedOn"] = DateTime.Now.AddMinutes(-1);
            _raHoF.OwnerId = _hoFTeam.ToEntityReference();

        }

        [TestMethod]
        public void CanWork()
        {
            Target = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending)
            };

            Asset("Update", (int)StageEnum.PostOperation);
            Assert.IsTrue(handler.CanWork());
        }

        [TestMethod]
        public void DoWork_2PendingTo1Rejected()
        {
            Target = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Rejected)
            };

            PreImage = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending),
                invln_ispid = _isp.ToEntityReference()
            };

            PostImage = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Rejected),
                invln_ispid = _isp.ToEntityReference()
            };

            fakedContext.Initialize(new List<Entity>(){
                _isp,
                _raDes,
                _raHoF
            });

            Asset("Update", (int)StageEnum.PostOperation);
            handler.DoWork();
            var isp = fakedContext.CreateQuery<invln_ISP>().FirstOrDefault();
            Assert.AreEqual((int)invln_ApprovalStatus.Rejected, isp.invln_ApprovalStatus.Value);
        }

        [TestMethod]
        public void DoWork_2PendingTo1Reviewed()
        {
            Target = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Reviewed)
            };

            PreImage = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending),
                invln_ispid = _isp.ToEntityReference()
            };

            PostImage = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Reviewed),
                invln_ispid = _isp.ToEntityReference()
            };

            fakedContext.Initialize(new List<Entity>(){
                _isp,
                _raDes,
                _raHoF
            });

            Asset("Update", (int)StageEnum.PostOperation);
            handler.DoWork();
            var isp = fakedContext.CreateQuery<invln_ISP>().FirstOrDefault();
            Assert.AreEqual((int)invln_ApprovalStatus.Pending, isp.invln_ApprovalStatus.Value);
        }

        [TestMethod]
        public void DoWork_1PendingTo2Reviewed()
        {
            _raDes.invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending);
            _raHoF.invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Reviewed);

            Target = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Reviewed)
            };

            PreImage = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending),
                invln_ispid = _isp.ToEntityReference()
            };

            PostImage = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Reviewed),
                invln_ispid = _isp.ToEntityReference()
            };

            fakedContext.Initialize(new List<Entity>(){
                _isp,
                _raDes,
                _raHoF
            });

            Asset("Update", (int)StageEnum.PostOperation);
            handler.DoWork();
            var isp = fakedContext.CreateQuery<invln_ISP>().FirstOrDefault();
            Assert.AreEqual((int)invln_ApprovalStatus.Pending, isp.invln_ApprovalStatus.Value);
        }

        [TestMethod]
        public void DoWork_2PendingTo1Approved()
        {
            _raDes.invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending);
            _raHoF.invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending);

            Target = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Approved)
            };

            PreImage = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending),
                invln_ispid = _isp.ToEntityReference()
            };

            PostImage = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Approved),
                invln_ispid = _isp.ToEntityReference()
            };

            fakedContext.Initialize(new List<Entity>(){
                _isp,
                _raDes,
                _raHoF
            });

            Asset("Update", (int)StageEnum.PostOperation);
            handler.DoWork();
            var isp = fakedContext.CreateQuery<invln_ISP>().FirstOrDefault();
            Assert.AreEqual((int)invln_ApprovalStatus.Pending, isp.invln_ApprovalStatus.Value);
        }

        [TestMethod]
        public void DoWork_1PendingTo2Approved()
        {
            _raDes.invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Approved);
            _raHoF.invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Approved);

            Target = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Approved)
            };

            PreImage = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending),
                invln_ispid = _isp.ToEntityReference()
            };

            PostImage = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Approved),
                invln_ispid = _isp.ToEntityReference()
            };

            fakedContext.Initialize(new List<Entity>(){
                _isp,
                _raDes,
                _raHoF,
                _desTeam,
                _hoFTeam,
                _cROTeam,
            });

            Asset("Update", (int)StageEnum.PostOperation);
            handler.DoWork();
            var isp = fakedContext.CreateQuery<invln_ISP>().FirstOrDefault();
            Assert.AreEqual((int)invln_ApprovalStatus.Approved, isp.invln_ApprovalStatus.Value);
        }

        [TestMethod]
        public void DoWork_PendingToRevied()
        {
            _raDes.invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Reviewed);
            _raHoF.invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Approved);

            Target = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Reviewed)
            };

            PreImage = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending),
                invln_ispid = _isp.ToEntityReference()
            };

            PostImage = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Reviewed),
                invln_ispid = _isp.ToEntityReference()
            };

            fakedContext.Initialize(new List<Entity>(){
                _isp,
                _raDes,
                _raHoF,
                _desTeam,
                _hoFTeam,
                _cROTeam,
            });

            Asset("Update", (int)StageEnum.PostOperation);
            handler.DoWork();
            var isp = fakedContext.CreateQuery<invln_ISP>().FirstOrDefault();
            Assert.AreEqual((int)invln_ApprovalStatus.Approved, isp.invln_ApprovalStatus.Value);
        }

        [TestMethod]
        public void DoWork_InReview()
        {
            _raDes.invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Reviewed);
            _raHoF.invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Reviewed);

            Target = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Reviewed)
            };

            PreImage = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Pending),
                invln_ispid = _isp.ToEntityReference()
            };

            PostImage = new invln_reviewapproval()
            {
                Id = _raDesId,
                invln_status = new OptionSetValue((int)invln_StatusReviewApprovalSet.Reviewed),
                invln_ispid = _isp.ToEntityReference()
            };

            fakedContext.Initialize(new List<Entity>(){
                _isp,
                _raDes,
                _raHoF,
                _desTeam,
                _hoFTeam,
                _cROTeam,
            });

            Asset("Update", (int)StageEnum.PostOperation);
            handler.DoWork();
            var isp = fakedContext.CreateQuery<invln_ISP>().FirstOrDefault();
            Assert.AreEqual((int)invln_ApprovalStatus.InReview, isp.invln_ApprovalStatus.Value);
        }
    }
}
