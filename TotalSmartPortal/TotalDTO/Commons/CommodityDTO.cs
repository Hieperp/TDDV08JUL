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
        string CodePartB { get; set; }
        string CodePartC { get; set; }
        string CodePartD { get; set; }

        string Name { get; set; }
        string OfficialName { get; set; }
        string OriginalName { get; set; }

        int CommodityBrandID { get; set; }
        string CommodityBrandName { get; set; }

        int CommodityCategoryID { get; set; }
        string CommodityCategoryName { get; set; }

        int CommodityTypeID { get; set; }
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
    }

    public class CommodityPrimitiveDTO<TCommodityOption> : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
        where TCommodityOption : ICMDOption, new()
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return new TCommodityOption().NMVNTaskID; } }

        public int GetID() { return this.CommodityID; }
        public void SetID(int id) { this.CommodityID = id; }

        public int CommodityID { get; set; }
        public string Code { get { return this.CodePartA + " " + this.CodePartB + " " + this.CodePartC + " " + this.CodePartD; } }
        public string OfficialCode { get { return TotalBase.CommonExpressions.AlphaNumericString(this.Code); } }
        public string CodePartA { get; set; }
        public string CodePartB { get; set; }
        public string CodePartC { get; set; }
        public string CodePartD { get; set; }

        public string Name { get; set; }
        public string OfficialName { get; set; }
        public string OriginalName { get; set; }

        public int CommodityBrandID { get; set; }
        public string CommodityBrandName { get; set; }

        public int CommodityCategoryID { get; set; }
        public string CommodityCategoryName { get; set; }

        public int CommodityTypeID { get; set; }
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
    }

    public interface ICommodityDTO : ICommodityPrimitiveDTO
    {
        string ControllerName { get; }
    }

    public class CommodityDTO<TCommodityOption> : CommodityPrimitiveDTO<TCommodityOption>, ICommodityDTO
        where TCommodityOption : ICMDOption, new()
    {
        public string ControllerName { get { return this.NMVNTaskID.ToString() + "s"; } }
    }
}
