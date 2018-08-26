using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Commons
{
    public interface ICMDOption { GlobalEnums.NmvnTaskID NMVNTaskID { get; } }

    public class CMDMaterial : ICMDOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Material; } } }
    public class CMDItem : ICMDOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Item; } } }
    public class CMDProduct : ICMDOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Product; } } }


    public interface ICommodityPrimitiveDTO : IPrimitiveEntity, IPrimitiveDTO, IBaseDTO
    {
        int CommodityID { get; set; }
        string Code { get; }
        string OfficialCode { get; }
        
        string CodePartA { get; set; }        
        string CodePartB { get; }        
        string CodePartC { get; }        
        string CodePartD { get; }
        string CodePartE { get; set; }
        string CodePartF { get; set; }

        string Name { get; set; }
        string OfficialName { get; set; }
        string OriginalName { get; set; }

        int CommodityBrandID { get; set; }
        string CommodityBrandName { get; set; }

        int CommodityCategoryID { get; set; }
        string CommodityCategoryName { get; set; }

        int CommodityClassID { get; set; }
        string CommodityClassName { get; set; }

        int CommodityLineID { get; set; }
        string CommodityLineName { get; set; }

        int CommodityTypeID { get; }
        string CommodityTypeName { get; set; }

        [Display(Name = "Nhà cung cấp")]
        int SupplierID { get; }

        int PiecePerPack { get; set; }
        int QuantityAlert { get; set; }
        decimal ListedPrice { get; set; }
        decimal GrossPrice { get; set; }
        string PurchaseUnit { get; set; }
        string SalesUnit { get; set; }
        string Packing { get; set; }
        string Origin { get; set; }

        double Weight { get; set; }
        double LeadTime { get; set; }

        bool IsRegularCheckUps { get; set; }
        bool Discontinue { get; set; }

        string HSCode { get; set; }

        bool IsMaterial { get; }
        bool IsItem { get; }
        bool IsProduct { get; }
    }

    public class CommodityPrimitiveDTO<TCommodityOption> : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
        where TCommodityOption : ICMDOption, new()
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return new TCommodityOption().NMVNTaskID; } }

        public int GetID() { return this.CommodityID; }
        public void SetID(int id) { this.CommodityID = id; }

        public int CommodityID { get; set; }
        public string Code { get { return !String.IsNullOrWhiteSpace(this.CodePartA) ?  this.CodePartA + " " : !String.IsNullOrWhiteSpace(this.CodePartB) ? this.CodePartB + " " : !String.IsNullOrWhiteSpace(this.CodePartC) ? this.CodePartC + " " : !String.IsNullOrWhiteSpace(this.CodePartD) ? this.CodePartD + " " : !String.IsNullOrWhiteSpace(this.CodePartE) ? this.CodePartE + " " : !String.IsNullOrWhiteSpace(this.CodePartF) ? this.CodePartF : ""; } }
        public string OfficialCode { get { return TotalBase.CommonExpressions.AlphaNumericString(this.Code); } }
        public string CodePartA { get; set; }
        public string CodePartB { get { return this.CommodityCategoryName; } }
        public string CodePartC { get { return  this.CommodityLineName; } }
        public string CodePartD { get { return this.CommodityClassName; } }
        public string CodePartE { get; set; }
        public string CodePartF { get; set; }

        public string Name { get; set; }
        public string OfficialName { get; set; }
        public string OriginalName { get; set; }

        public int CommodityBrandID { get; set; }
        public string CommodityBrandName { get; set; }

        public int CommodityCategoryID { get; set; }
        public string CommodityCategoryName { get; set; }

        public int CommodityClassID { get; set; }
        public string CommodityClassName { get; set; }

        public int CommodityLineID { get; set; }
        public string CommodityLineName { get; set; }

        public int CommodityTypeID { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.Material ? 3 : this.NMVNTaskID == GlobalEnums.NmvnTaskID.Item ? 2 : this.NMVNTaskID == GlobalEnums.NmvnTaskID.Product ? 1 : 0; } }
        public string CommodityTypeName { get; set; }

        public int SupplierID { get { return 1; } }

        public int PiecePerPack { get; set; }
        public int QuantityAlert { get; set; }
        public decimal ListedPrice { get; set; }
        public decimal GrossPrice { get; set; }
        public string PurchaseUnit { get; set; }
        public string SalesUnit { get; set; }
        public string Packing { get; set; }
        public string Origin { get; set; }

        public double Weight { get; set; }
        public double LeadTime { get; set; }

        public bool IsRegularCheckUps { get; set; }
        public bool Discontinue { get; set; }

        public string HSCode { get; set; }

        public override int PreparedPersonID { get { return 1; } }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            DecimalRequired checkDeciamal = new DecimalRequired();
            if (this.NMVNTaskID == GlobalEnums.NmvnTaskID.Item && !checkDeciamal.IsValid(this.CodePartE) && !checkDeciamal.IsValid(CodePartF)) yield return new ValidationResult("Lỗi tổng số lượng [TotalQuantityPositive]", new[] { "TotalQuantityPositive" });            
        }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule(); 
        }
    }

    public interface ICommodityDTO : ICommodityPrimitiveDTO
    {
        string ControllerName { get; }
    }

    public class CommodityDTO<TCommodityOption> : CommodityPrimitiveDTO<TCommodityOption>, ICommodityDTO
        where TCommodityOption : ICMDOption, new()
    {
        public string ControllerName { get { return this.NMVNTaskID.ToString() + "s"; } }

        public bool IsMaterial { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.Material; } }
        public bool IsItem { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.Item; } }
        public bool IsProduct { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.Product; } }
    }
}
