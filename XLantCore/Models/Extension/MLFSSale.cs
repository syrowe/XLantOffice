﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace XLantCore.Models
{
    public partial class MLFSSale
    {
        /// <summary>
        /// Read Only - Provides the total of the net and vat amounts
        /// </summary>
        [Display(Name="Gross Amount")]
        public decimal GrossAmount
        {
            get
            {
                return Tools.HandleNull(NetAmount) + Tools.HandleNull(VAT);
            }
        }

        /// <summary>
        /// Read Only - Tests whether the initial fee criteria is met is it greater that £450 if not is it >= 3%
        /// </summary>
        [Display(Name="Initial Fee Passed")]
        public bool InitialFeePass
        {
            get
            {
                bool pass = false;
                if (Investment != 0)
                {
                    if (NetAmount >= 450)
                    {
                        pass = true;
                    }
                    else
                    {
                        if (NetAmount / Investment >= (decimal)0.03)
                        {
                            pass = true;
                        }
                    }
                    return pass;
                }
                else
                {
                    return pass;
                }
            }
        }

        /// <summary>
        /// Read Only - Tests whether the ongoing fee criteria is met is it greater that £500 for 12 months if not is it >= 1%
        /// </summary>
        [Display(Name="Ongoing Fee Pass")]
        public bool OngoingFeePass
        {
            get
            {
                bool pass = false;
                decimal twelveMonthsIncome = Investment * OnGoingPercentage / 100;
                if (twelveMonthsIncome >= 500)
                {
                    pass = true;
                }
                else
                {
                    if (OnGoingPercentage >= (decimal)0.01)
                    {
                        pass = true;
                    }
                }
                return pass;
            }
        }

        /// <summary>
        /// Read Only - After all adjustments how much is left to be collected
        /// </summary>
        public decimal Outstanding
        {
            get
            {
                if (Adjustments != null && Adjustments.Count>0)
                {
                    return GrossAmount + Adjustments.DefaultIfEmpty().Sum(z => z.Amount); 
                }
                else
                {
                    return GrossAmount;
                }
            }
        }

        /// <summary>
        /// Read Only - How much has been received
        /// </summary>
        public decimal Receipt
        {
            get
            {
                if (Adjustments != null && Adjustments.Where(x => x.ReceiptId != null).ToList().Count > 0)
                {
                    return Adjustments.Where(x => x.ReceiptId != null).DefaultIfEmpty().Sum(y => y.Amount);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Read Only - What is the amount of the NTU adjustment - This should always be the total of the debtor amount
        /// </summary>
        [Display(Name="NTU")]
        public decimal NotTakenUp
        {
            get
            {
                if (Adjustments != null && Adjustments.Where(x => x.NotTakenUp).ToList().Count > 0)
                {
                    return Adjustments.Where(x => x.NotTakenUp).DefaultIfEmpty().Sum(y => y.Amount);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Read Only - The total of the Debtor Adjustments which are not Variances or NTUs or Reciepts
        /// </summary>
        public decimal Adjustment
        {
            get
            {
                if (Adjustments != null && Adjustments.Where(x => x.ReceiptId == null && !x.IsVariance && !x.NotTakenUp).ToList().Count > 0)
                {
                    return Adjustments.Where(x => x.ReceiptId == null && !x.IsVariance && !x.NotTakenUp).DefaultIfEmpty().Sum(y => y.Amount);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Read Only - The total of the adjustments which are variances
        /// </summary>
        public decimal Variance
        {
            get
            {
                if (Adjustments != null && Adjustments.Where(x => x.IsVariance).ToList().Count > 0)
                {
                    return Adjustments.Where(x => x.IsVariance).DefaultIfEmpty().Sum(y => y.Amount);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Updates the object for info collected from the plan CSV
        /// </summary>
        /// <param name="row">The data from the plan.csv in a datatable row</param>
        public void AddPlanData(DataRow row)
        {
            ProviderName = row["Provider.Name"].ToString();
            DateTime creationDate = DateTime.Parse(row["Owner 1.Creation Date"].ToString());
            if (creationDate > ReportingPeriod.StartDate.AddMonths(-9))
            {
                IsNew = true;
            }
            Investment = Tools.HandleNull(row["Total Premiums to Date"].ToString());
            OnGoingPercentage = Tools.HandleNull(row["On-going Fee Percentage"].ToString());
            Organisation = row["Selling Adviser.Group.Name"].ToString();
        }

        /// <summary>
        /// Updates the object from plan and fee data gained from IO Api
        /// </summary>
        /// <param name="plan">The plan</param>
        /// <param name="fee">the Fee</param>
        public void AddPlanData(MLFSPlan plan, MLFSFee fee)
        {
            ProviderName = plan.Provider;
            if (!plan.IsPreExistingClient)
            {
                IsNew = true;
            }
            Investment = plan.ContributionsToDate;
            if (fee != null)
            {
                OnGoingPercentage = fee.FeePercentage;
                Organisation = "";
            }
        }

        /// <summary>
        /// Takes IO client data and uses it to update information about the sale entry
        /// </summary>
        /// <param name="client">the client to which the sale relates</param>
        public void AddClientData(MLFSClient client)
        {
            MLFSPlan plan = client.Plans.Where(x => x.Reference == this.PlanReference).FirstOrDefault();
            if (plan != null)
            {
                ProviderName = plan.Provider;
                if (!plan.IsPreExistingClient)
                {
                    IsNew = true;
                }
                if (plan.CurrentValuation == 0)
                {
                    this.Investment = plan.ContributionsToDate;
                }
                else
                {
                    this.Investment = plan.CurrentValuation;
                }
                MLFSFee fee = client.Fees.Where(x => x.Plan != null && x.Plan.PrimaryID == plan.PrimaryID && x.IsRecurring).FirstOrDefault();
                if (fee != null)
                {
                    OnGoingPercentage = fee.FeePercentage;
                }

            }

            if (client.Plans != null && client.Plans.Count > 0)
            {
                this.EstimatedOtherIncome = 0;
                foreach (MLFSPlan p in client.Plans.Where(x => plan == null || x.PrimaryID != plan.PrimaryID))
                {
                    MLFSFee fee = client.Fees.Where(x => x.Plan != null && x.Plan.PrimaryID.Contains(p.PrimaryID) && x.IsRecurring).FirstOrDefault();
                    if (fee != null)
                    {
                        decimal value = 0;
                        if (p.CurrentValuation == 0)
                        {
                            value = p.ContributionsToDate;
                        }
                        else
                        {
                            value = p.CurrentValuation;
                        }
                        decimal d = value * fee.FeePercentage /100;
                        this.EstimatedOtherIncome += d;
                    }
                } 
            }
        }

        /// <summary>
        /// Creates an NTU adjustment to reverse out a debtor when the transaction never happens
        /// </summary>
        /// <param name="period">the period in which the plan is marked as NTU</param>
        /// <returns></returns>
        public MLFSDebtorAdjustment CreateNTU(MLFSReportingPeriod period)
        {
            MLFSDebtorAdjustment adj = new MLFSDebtorAdjustment
            {
                DebtorId = (int)Id,
                Amount = GrossAmount * -1,
                Debtor = this,
                IsVariance = false,
                NotTakenUp = true,
                ReportingPeriod = period,
                ReportingPeriodId = period.Id
            };
            Adjustments.Add(adj);
            return adj;
        }

        /// <summary>
        /// Runs through a a bathc of receipts and compares it to the debtors using IORefernece and adds a receipt entry where a match is found
        /// </summary>
        /// <param name="debtors">The current debtors</param>
        /// <param name="receipts">The batch of receipts</param>
        /// <returns>A list of the adjustments (receipts) to be added</returns>
        public static List<MLFSDebtorAdjustment> CheckForReceipts(List<MLFSSale> debtors, List<MLFSIncome> receipts)
        {
            List<MLFSDebtorAdjustment> adjs = new List<MLFSDebtorAdjustment>();
            for (int i = 0; i < debtors.Count; i++)
            {
                MLFSSale debtor = debtors[i];
                foreach (MLFSIncome receipt in receipts)
                {
                    if (receipt.IOReference == debtor.IOReference || (receipt.IOReference == debtor.PlanReference && receipt.Amount == debtor.GrossAmount))
                    {
                        MLFSDebtorAdjustment adj = new MLFSDebtorAdjustment(debtor, receipt);
                        debtor.Adjustments.Add(adj);
                        adjs.Add(adj);
                    }
                }
            }
            return adjs;
        }
    }
}
