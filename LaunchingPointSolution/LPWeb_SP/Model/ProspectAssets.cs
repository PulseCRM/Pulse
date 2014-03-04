using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ProspectAssets:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ProspectAssets
	{
		public ProspectAssets()
		{}
		#region Model
		private int _prospectassetid;
		private int _contactid;
		private string _name;
		private string _account;
		private decimal? _amount;
		private string _type;
		/// <summary>
		/// 
		/// </summary>
		public int ProspectAssetId
		{
			set{ _prospectassetid=value;}
			get{return _prospectassetid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ContactId
		{
			set{ _contactid=value;}
			get{return _contactid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Account
		{
			set{ _account=value;}
			get{return _account;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Amount
		{
			set{ _amount=value;}
			get{return _amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		#endregion Model

	}
}

