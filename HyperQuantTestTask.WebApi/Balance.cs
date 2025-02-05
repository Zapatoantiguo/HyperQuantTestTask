namespace HyperQuantTestTask.WebApi
{
    public class Balance
    {
        public decimal BtcAmount { get; set; }
        public decimal XrpAmount { get; set; }
        public decimal XmrAmount { get; set; }
        public decimal DashAmount { get; set; }

        public decimal TotalUsd { get; set; }
        public decimal TotalBtc {  get; set; }
        public decimal TotalXrp { get; set; }
        public decimal TotalXmr {  get; set; }
        public decimal TotalDash {  get; set; }


    }
}
