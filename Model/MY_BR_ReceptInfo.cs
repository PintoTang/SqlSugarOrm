using SqlSugar;
using System;

namespace Model
{
    [SugarTable("MY_BR_ReceptInfo")]
	public class MY_BR_ReceptInfo
	{
		[SugarColumn(IsPrimaryKey = true)]
		public string RID { get; set; }
		public string SubCom { get; set; }
		public string RecHierachies { get; set; }
		public string VisitLocation { get; set; }
		public string VisitCom { get; set; }
		public string Visitors { get; set; }
		public string VisitStartTime { get; set; }
		public string SurveyTopic { get; set; }
		public string RecChargePerson { get; set; }
		public string VisitObjectDesc { get; set; }
		public string SubComSituation { get; set; }
		public string ExistProblem { get; set; }
		public string Expectation { get; set; }
		public string RecPurpose { get; set; }
		public string VisitEndTime { get; set; }
		public string RecTypes { get; set; }
		public decimal RecScore { get; set; }
		public string Attachment { get; set; }
		public string AttachmentUrl { get; set; }
		public string RecSummary { get; set; }
		public string RecLevel { get; set; }
		public string Status { get; set; }
		public string WritePeople { get; set; }
		public DateTime WriteTime { get; set; }
		public string Highlights { get; set; }
		public string Shortage { get; set; }
		public string Improvment { get; set; }
		public string FolderId { get; set; }
	}
}
