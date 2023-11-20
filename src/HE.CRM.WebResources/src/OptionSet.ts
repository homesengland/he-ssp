export enum RetrieveRequestType {
  Fetch,
  XHR,
  WebAPI
}

export enum CreditRatingAgency {
  standardAndPoor = 858110000,
  moodyInvestorServices = 858110001,
  fitchIbca = 858110002,
  other = 858110003,
}

export enum ExternalStatus {
  sentForApproval = 858110010,
}

export enum Securities {
  debenture = 858110000,
  firstLegalCharge = 858110001,
  subsequentCharge = 858110002,
  personalGuarantee = 858110003,
  parentCompanyGuarantee = 858110004,
  subordinatedDeed = 858110005,
  costOverrunGuarantee = 858110006,
  completionGuarantee = 858110007,
  interestShortfall = 858110008,
  other = 858110009,
}


export enum StatusReviewApproval {
  Pending = 858110000,
  Reviewed = 858110001,
  Sent_Back = 858110002,
  Approved = 858110003,
  Rejected = 858110004,
}
