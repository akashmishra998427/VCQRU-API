namespace Login.Models
{
    public class Company
    {
        #region CompanyRegion
        public static string? DownloadZipFIle { get; set; }
        public string? HindiFile { get; set; }
        public string? EnglishFle { get; set; }
        public string? Row_ID { get; set; }
        public Int64 M_code_Row_ID { get; set; }
        public string? OldRow_ID { get; set; }
        public Int32 NoofCodes { get; set; }

        public string? ProductionUnit { get; set; }
        public string? Channels { get; set; }
        public Int32 ExcNoofCodes { get; set; }
        public Int32 ActNoofCodes { get; set; }
        public string? Comp_ID { get; set; }
        public string? Comp_Name { get; set; }
        public Int32 Comp_Cat_Id { get; set; }
        public string? Comp_Email { get; set; }
        public string? PacketCode { get; set; }
        public string? WebSite { get; set; }
        public string? Address { get; set; }
        public Int32 City_ID { get; set; }
        public Int32 M_ConsumerID { get; set; }
        public Int64 intM_Consumer_MCOde { get; set; }
        public string? Contact_Person { get; set; }
        public string? Mobile_No { get; set; }
        public string? Phone_No { get; set; }
        public string? Fax { get; set; }
        public string? Reg_Date { get; set; }
        public DateTime Req_Date { get; set; }
        public DateTime Rec_Date { get; set; }
        public string? Password { get; set; }
        public string? oldPassword { get; set; }
        public Int32 IsRetailer { get; set; }
        public Int32 Status { get; set; }
        public Int32 Email_Vari_Flag { get; set; }
        public Int32 Update_Flag { get; set; }
        public string? TypeOfCompany { get; set; }
        public string? Comp_type { get; set; }
        public string? Condition { get; set; }
        public DateTime AmcDateFrom { get; set; }
        public DateTime AmcDateTo { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime DateFromChk { get; set; }
        public DateTime DateToChk { get; set; }
        public DateTime PromoDateFrom { get; set; }
        public DateTime PromoDateTo { get; set; }
        public string? logo_path { get; set; }
        #endregion
    }

}
