//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FiMA.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class INVESTOR_TRANSFERS
    {
        public int TRF_ID { get; set; }
        public Nullable<System.DateTime> TRF_DATE { get; set; }
        public string TRF_TIME { get; set; }
        public string TRF_FUND { get; set; }
        public string TRF_FUND_NAME { get; set; }
        public string TRF_CLIENT { get; set; }
        public string TRF_CLIENT_NAME { get; set; }
        public string TRF_CP { get; set; }
        public string TRF_CP_NAME { get; set; }
        public Nullable<double> TRF_SHARES { get; set; }
        public string TRF_STSTUS { get; set; }
        public string TRF_CD_STATUS { get; set; }
        public string CD_GID { get; set; }
        public Nullable<int> MSG_NUMBER { get; set; }
    }
}
