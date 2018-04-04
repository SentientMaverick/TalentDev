using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class Requisition
    {
    [Key]
    public int ID { get; set; }
    public string RequisitionNo{ get; set; }
    public string Requestor { get; set; }
    public string NoSeries{ get; set; } 
    public string ResponsibilityCenter{ get; set; }
    public string Stage { get; set; }
    public string Score{ get; set; }
    public string StageCode{ get; set; }
    public bool Qualified  { get; set; }
        //setled
    public string JobSupervisorManager  { get; set; }
    public string GlobalDimension { get; set; }
    public int TurnAroundTime{ get; set; }
    public int GracePeriod { get; set; }
    public bool Closed{ get; set; }
    public string RequisitionType{ get; set; }
    public DateTime ClosingDate{ get; set; }
    public string Status{ get; set; }
    public int RequiredPositions{ get; set; }
    public int VacantPositions { get; set; }   
    public string ReasonForRequest { get; set; }
    public string AnyAdditionalInformation{ get; set; }
    public string JobGrade { get; set; }
    public string TypeOfContractRequired { get; set; }        

    }
}