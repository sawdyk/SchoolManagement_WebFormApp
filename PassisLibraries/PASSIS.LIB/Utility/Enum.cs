using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PASSIS.LIB.Utility
{
    /// <summary>
    /// Tentative:  meant to be remove and made more generic. 
    /// </summary>
    public enum Campus
    {
        Anthony = 1,
        Lekki = 2

    }
    public enum LearningSupport
    {
        No = 0,
        Yes = 1

    }
    public enum ScoreSheetSubmissionStatus
    {
        Awaiting_Approval = 0,
        Disapproved = 1,
        Approved = 2,
        Processed = 3
    }
    public enum UserStatus
    {
        Active = 1,
        Dismissed = 2,
        Graduated = 3,
        Left = 4,
        Inactive = 5
    }
    public enum FeeYearStatus
    {
        Disabled = 0,
        Enabled = 1
    }
    public enum Gender
    {
        Male = 1,
        Female = 2

    }
    public enum MarkStatus
    {
        New = 0,
        Submitted = 1,
        Marked = 2


    }
    public enum roles
    {
        systemadmin = 1,
        schooladmin = 2,
        teacher = 3,
        parent = 5,
        student = 6,
        auditor = 7,
        AdmissionOfficer = 13
    }
    public enum FormMode
    {
        View = 1,
        Insert = 2,
        Edit = 3
    }
    public enum PaymentStatus
    {
        New = 1,
        Pending = 2,
        Successful = 3,
        Unsuccessful = 4
    }

    public enum PaymentChannel
    {
        Interswitch = 1,
        CashDeposit = 2,
        OnlineTransfer = 3,
    }


}
