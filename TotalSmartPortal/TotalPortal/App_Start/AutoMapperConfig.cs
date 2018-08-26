using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;


using TotalModel.Models;

using TotalDTO.Sales;
using TotalDTO.Purchases;
using TotalDTO.Productions;
using TotalDTO.Inventories;
using TotalDTO.Commons;
using TotalDTO.Accounts;

using TotalPortal.Areas.Sales.ViewModels;
using TotalPortal.Areas.Purchases.ViewModels;
using TotalPortal.Areas.Productions.ViewModels;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Accounts.ViewModels;
using TotalPortal.Areas.Commons.ViewModels;


[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TotalPortal.App_Start.AutoMapperConfig), "SetupMappings")]
namespace TotalPortal.App_Start
{
    public static class AutoMapperConfig
    {
        public static void SetupMappings()
        {
            ////////https://github.com/AutoMapper/AutoMapper/wiki/Static-and-Instance-API

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<PurchaseRequisition, PurchaseRequisitionViewModel>();
                cfg.CreateMap<PurchaseRequisition, PurchaseRequisitionDTO>();
                cfg.CreateMap<PurchaseRequisitionPrimitiveDTO, PurchaseRequisition>();
                cfg.CreateMap<PurchaseRequisitionViewDetail, PurchaseRequisitionDetailDTO>();
                cfg.CreateMap<PurchaseRequisitionDetailDTO, PurchaseRequisitionDetail>();

                cfg.CreateMap<GoodsReceipt, GoodsReceiptViewModel>();
                cfg.CreateMap<GoodsReceipt, GoodsReceiptDTO>();
                cfg.CreateMap<GoodsReceiptPrimitiveDTO, GoodsReceipt>();
                cfg.CreateMap<GoodsReceiptViewDetail, GoodsReceiptDetailDTO>();
                cfg.CreateMap<GoodsReceiptDetailDTO, GoodsReceiptDetail>();







                cfg.CreateMap<MaterialIssue, MaterialIssueViewModel>();
                cfg.CreateMap<MaterialIssue, MaterialIssueDTO>();
                cfg.CreateMap<MaterialIssuePrimitiveDTO, MaterialIssue>();
                cfg.CreateMap<MaterialIssueViewDetail, MaterialIssueDetailDTO>();
                cfg.CreateMap<MaterialIssueDetailDTO, MaterialIssueDetail>();

                cfg.CreateMap<WarehouseAdjustment, WarehouseAdjustmentDTO<WAOptionMtlRct>>();
                cfg.CreateMap<WarehouseAdjustment, WarehouseAdjustmentDTO<WAOptionMtlIss>>();
                cfg.CreateMap<WarehouseAdjustment, WarehouseAdjustmentDTO<WAOptionMtlAdj>>();

                cfg.CreateMap<WarehouseAdjustment, WarehouseAdjustmentDTO<WAOptionItmRct>>();
                cfg.CreateMap<WarehouseAdjustment, WarehouseAdjustmentDTO<WAOptionItmIss>>();
                cfg.CreateMap<WarehouseAdjustment, WarehouseAdjustmentDTO<WAOptionItmAdj>>();

                cfg.CreateMap<WarehouseAdjustment, WarehouseAdjustmentDTO<WAOptionPrdRct>>();
                cfg.CreateMap<WarehouseAdjustment, WarehouseAdjustmentDTO<WAOptionPrdIss>>();
                cfg.CreateMap<WarehouseAdjustment, WarehouseAdjustmentDTO<WAOptionPrdAdj>>();

                cfg.CreateMap<WarehouseAdjustmentPrimitiveDTO<WAOptionMtlRct>, WarehouseAdjustment>();
                cfg.CreateMap<WarehouseAdjustmentPrimitiveDTO<WAOptionMtlIss>, WarehouseAdjustment>();
                cfg.CreateMap<WarehouseAdjustmentPrimitiveDTO<WAOptionMtlAdj>, WarehouseAdjustment>();

                cfg.CreateMap<WarehouseAdjustmentPrimitiveDTO<WAOptionItmRct>, WarehouseAdjustment>();
                cfg.CreateMap<WarehouseAdjustmentPrimitiveDTO<WAOptionItmIss>, WarehouseAdjustment>();
                cfg.CreateMap<WarehouseAdjustmentPrimitiveDTO<WAOptionItmAdj>, WarehouseAdjustment>();

                cfg.CreateMap<WarehouseAdjustmentPrimitiveDTO<WAOptionPrdRct>, WarehouseAdjustment>();
                cfg.CreateMap<WarehouseAdjustmentPrimitiveDTO<WAOptionPrdIss>, WarehouseAdjustment>();
                cfg.CreateMap<WarehouseAdjustmentPrimitiveDTO<WAOptionPrdAdj>, WarehouseAdjustment>();

                cfg.CreateMap<WarehouseAdjustmentViewDetail, WarehouseAdjustmentDetailDTO>();
                cfg.CreateMap<WarehouseAdjustmentDetailDTO, WarehouseAdjustmentDetail>();

                cfg.CreateMap<WarehouseAdjustment, OtherMaterialReceiptViewModel>();
                cfg.CreateMap<WarehouseAdjustment, OtherMaterialIssueViewModel>();
                cfg.CreateMap<WarehouseAdjustment, MaterialAdjustmentViewModel>();

                cfg.CreateMap<WarehouseAdjustment, OtherItemReceiptViewModel>();
                cfg.CreateMap<WarehouseAdjustment, OtherItemIssueViewModel>();
                cfg.CreateMap<WarehouseAdjustment, ItemAdjustmentViewModel>();

                cfg.CreateMap<WarehouseAdjustment, OtherProductReceiptViewModel>();
                cfg.CreateMap<WarehouseAdjustment, OtherProductIssueViewModel>();
                cfg.CreateMap<WarehouseAdjustment, ProductAdjustmentViewModel>();









                cfg.CreateMap<PlannedOrder, PlannedOrderViewModel>();
                cfg.CreateMap<PlannedOrder, PlannedOrderDTO>();
                cfg.CreateMap<PlannedOrderPrimitiveDTO, PlannedOrder>();
                cfg.CreateMap<PlannedOrderViewDetail, PlannedOrderDetailDTO>();
                cfg.CreateMap<PlannedOrderDetailDTO, PlannedOrderDetail>();

                cfg.CreateMap<ProductionOrder, ProductionOrderViewModel>();
                cfg.CreateMap<ProductionOrder, ProductionOrderDTO>();
                cfg.CreateMap<ProductionOrderPrimitiveDTO, ProductionOrder>();
                cfg.CreateMap<ProductionOrderViewDetail, ProductionOrderDetailDTO>();
                cfg.CreateMap<ProductionOrderDetailDTO, ProductionOrderDetail>();




                cfg.CreateMap<SalesOrder, SalesOrderViewModel>();
                cfg.CreateMap<SalesOrder, SalesOrderDTO>();
                cfg.CreateMap<SalesOrderPrimitiveDTO, SalesOrder>();
                cfg.CreateMap<SalesOrderViewDetail, SalesOrderDetailDTO>();
                cfg.CreateMap<SalesOrderDetailDTO, SalesOrderDetail>();

                cfg.CreateMap<SalesOrder, SalesOrderBoxDTO>();


                cfg.CreateMap<DeliveryAdvice, DeliveryAdviceViewModel>();
                cfg.CreateMap<DeliveryAdvice, DeliveryAdviceDTO>();
                cfg.CreateMap<DeliveryAdvicePrimitiveDTO, DeliveryAdvice>();
                cfg.CreateMap<DeliveryAdviceViewDetail, DeliveryAdviceDetailDTO>();
                cfg.CreateMap<DeliveryAdviceDetailDTO, DeliveryAdviceDetail>();

                cfg.CreateMap<DeliveryAdvice, DeliveryAdviceBoxDTO>();


                cfg.CreateMap<SalesReturn, SalesReturnViewModel>();
                cfg.CreateMap<SalesReturn, SalesReturnDTO>();
                cfg.CreateMap<SalesReturnPrimitiveDTO, SalesReturn>();
                cfg.CreateMap<SalesReturnViewDetail, SalesReturnDetailDTO>();
                cfg.CreateMap<SalesReturnDetailDTO, SalesReturnDetail>();

                cfg.CreateMap<SalesReturn, SalesReturnBoxDTO>();

                cfg.CreateMap<GoodsIssue, GoodsIssueViewModel>();
                cfg.CreateMap<GoodsIssue, GoodsIssueDTO>();
                cfg.CreateMap<GoodsIssuePrimitiveDTO, GoodsIssue>();
                cfg.CreateMap<GoodsIssueViewDetail, GoodsIssueDetailDTO>();
                cfg.CreateMap<GoodsIssueDetailDTO, GoodsIssueDetail>();

                cfg.CreateMap<GoodsIssue, GoodsIssueBoxDTO>();

                cfg.CreateMap<HandlingUnit, HandlingUnitViewModel>();
                cfg.CreateMap<HandlingUnit, HandlingUnitDTO>();
                cfg.CreateMap<HandlingUnitPrimitiveDTO, HandlingUnit>();
                cfg.CreateMap<HandlingUnitViewDetail, HandlingUnitDetailDTO>();
                cfg.CreateMap<HandlingUnitDetailDTO, HandlingUnitDetail>();

                cfg.CreateMap<GoodsDelivery, GoodsDeliveryViewModel>();
                cfg.CreateMap<GoodsDelivery, GoodsDeliveryDTO>();
                cfg.CreateMap<GoodsDeliveryPrimitiveDTO, GoodsDelivery>();
                cfg.CreateMap<GoodsDeliveryViewDetail, GoodsDeliveryDetailDTO>();
                cfg.CreateMap<GoodsDeliveryDetailDTO, GoodsDeliveryDetail>();

                cfg.CreateMap<AccountInvoice, AccountInvoiceViewModel>();
                cfg.CreateMap<AccountInvoice, AccountInvoiceDTO>();
                cfg.CreateMap<AccountInvoicePrimitiveDTO, AccountInvoice>();
                cfg.CreateMap<AccountInvoiceViewDetail, AccountInvoiceDetailDTO>();
                cfg.CreateMap<AccountInvoiceDetailDTO, AccountInvoiceDetail>();

                cfg.CreateMap<Receipt, ReceiptViewModel>();
                cfg.CreateMap<Receipt, ReceiptDTO>();
                cfg.CreateMap<ReceiptPrimitiveDTO, Receipt>();
                cfg.CreateMap<ReceiptViewDetail, ReceiptDetailDTO>();
                cfg.CreateMap<ReceiptDetailDTO, ReceiptDetail>();

                cfg.CreateMap<Receipt, ReceiptBoxDTO>();

                cfg.CreateMap<CreditNote, CreditNoteViewModel>();
                cfg.CreateMap<CreditNote, CreditNoteDTO>();
                cfg.CreateMap<CreditNotePrimitiveDTO, CreditNote>();
                cfg.CreateMap<CreditNoteDetail, CreditNoteDetailDTO>();
                cfg.CreateMap<CreditNoteDetailDTO, CreditNoteDetail>();

                cfg.CreateMap<CreditNote, CreditNoteBoxDTO>();

                cfg.CreateMap<Customer, CustomerViewModel>();
                cfg.CreateMap<Customer, CustomerDTO>();
                cfg.CreateMap<Customer, CustomerBaseDTO>();
                cfg.CreateMap<CustomerPrimitiveDTO, Customer>();

                cfg.CreateMap<Customer, CustomerBaseDTO>();

                cfg.CreateMap<Employee, EmployeeViewModel>();
                cfg.CreateMap<Employee, EmployeeDTO>();
                cfg.CreateMap<Employee, EmployeeBaseDTO>();
                cfg.CreateMap<EmployeePrimitiveDTO, Employee>();

                cfg.CreateMap<Employee, EmployeeBaseDTO>();

                cfg.CreateMap<CommodityPrice, CommodityPriceViewModel>();
                cfg.CreateMap<CommodityPrice, CommodityPriceDTO>();
                cfg.CreateMap<CommodityPricePrimitiveDTO, CommodityPrice>();

                cfg.CreateMap<Commodity, MaterialViewModel>();
                cfg.CreateMap<Commodity, ItemViewModel>();
                cfg.CreateMap<Commodity, ProductViewModel>();
                cfg.CreateMap<Commodity, CommodityDTO<CMDMaterial>>();
                cfg.CreateMap<Commodity, CommodityDTO<CMDItem>>();
                cfg.CreateMap<Commodity, CommodityDTO<CMDProduct>>();
                cfg.CreateMap<CommodityPrimitiveDTO<CMDMaterial>, Commodity>();
                cfg.CreateMap<CommodityPrimitiveDTO<CMDItem>, Commodity>();
                cfg.CreateMap<CommodityPrimitiveDTO<CMDProduct>, Commodity>();


                cfg.CreateMap<Promotion, PromotionViewModel>();
                cfg.CreateMap<Promotion, PromotionDTO>();
                cfg.CreateMap<PromotionPrimitiveDTO, Promotion>();
                cfg.CreateMap<PromotionCommodityCodePart, PromotionCommodityCodePartDTO>();
                cfg.CreateMap<PromotionCommodityCodePartDTO, PromotionCommodityCodePart>();


                cfg.CreateMap<Promotion, PromotionBaseDTO>();

                cfg.CreateMap<Warehouse, WarehouseBaseDTO>();
                cfg.CreateMap<VoidType, VoidTypeBaseDTO>();

                //cfg.CreateMap<Module, ModuleViewModel>();
                //cfg.CreateMap<ModuleDetail, ModuleDetailViewModel>();
            });



            //Mapper.CreateMap<Module, ModuleViewModel>();
            //Mapper.CreateMap<ModuleDetail, ModuleDetailViewModel>();
        }
    }
}