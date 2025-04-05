//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Radin.Application.Interfaces.FacadPatterns;
//using Radin.Application.Services.Branch.Commands.BranchInfoSetService;
//using Radin.Application.Services.Branch.Queries.BranchInfoGetService;
//using Radin.Application.Services.Claims.Commands.ClaimCategorySetService;
//using Radin.Application.Services.Claims.Commands.ClaimSetService;
//using Radin.Application.Services.Claims.Queries.ClaimCategoryGetService;
//using Radin.Application.Services.Claims.Queries;
//using Radin.Application.Services.ContactUs.Commands.ContactMessageSet;
//using Radin.Application.Services.Contents.FacadPattern;
//using Radin.Application.Services.Contents.Queries.CommentInfoGet;
//using Radin.Application.Services.Contents.Queries.HomeContentGet;
//using Radin.Application.Services.Email.Commands;
//using Radin.Application.Services.Excelloading.FacadPattern;
//using Radin.Application.Services.Factors.Commands.Orders;
//using Radin.Application.Services.Factors.Queries.OrderGet;
//using Radin.Application.Services.HomePage.Commands.HomePageSliderEdit;
//using Radin.Application.Services.HomePage.Commands.HomePageSliderRemove;
//using Radin.Application.Services.HomePage.Commands.HomePageSliderSet;
//using Radin.Application.Services.HomePage.Queries.HomePageSlider;
//using Radin.Application.Services.Ideas.FacadPattern;
//using Radin.Application.Services.Product.Commands.ChallPrice;
//using Radin.Application.Services.Product.Commands.PowerCalculation;
//using Radin.Application.Services.Product.Commands;
//using Radin.Application.Services.Product.FacadPattern;
//using Radin.Application.Services.ProductItems.Commands.EdgeSizeEdit;
//using Radin.Application.Services.ProductItems.Commands.EdgeSizeRemove;
//using Radin.Application.Services.ProductItems.Commands.EdgeSizeSet;
//using Radin.Application.Services.ProductItems.Commands.TitleEdit;
//using Radin.Application.Services.ProductItems.Commands.TitleRemove;
//using Radin.Application.Services.ProductItems.Commands.TitleSet;
//using Radin.Application.Services.ProductItems.FacadPattern;
//using Radin.Application.Services.ProductItems.Queries.ChannelliumGet;
//using Radin.Application.Services.ProductItems.Queries.PlasticGet;
//using Radin.Application.Services.ProductItems.Queries.SwediMaxGet;
//using Radin.Application.Services.ProductItems.Queries.TablesGet.CrystalGet;
//using Radin.Application.Services.ProductItems.Queries.TablesGet.EdgeSizeGet;
//using Radin.Application.Services.ProductItems.Queries.TablesGet.MarginGet;
//using Radin.Application.Services.ProductItems.Queries.TablesGet.PowerGet;
//using Radin.Application.Services.ProductItems.Queries.TablesGet.SmdGet;
//using Radin.Application.Services.ProductItems.Queries.TitleGet;
//using Radin.Application.Services.Samples.FacadPattern;
//using Radin.Application.Services.SMS.Commands;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Radin.Application.Services.Factors.Commands.RecordProduct;
//using Radin.Application.Services.Factors.Commands.Orders.OrdersRemove;
//using Radin.Application.Services.Contents.Commands.CommentRemove;
//using Radin.Application.Services.Factors.Commands.Accessory.AccessorySet;
//using Radin.Application.Services.Factors.Commands.MountingFactorPrice;
//using Radin.Application.Services.StateInfoLoadingExcel;
//using Radin.Application.Services.GoesArea.Queries.StateGetService;
//using Radin.Application.Services.GoesArea.Queries.CityGetService;
//using Radin.Application.Services.Factors.Commands.UpdatePrice;
//using Radin.Application.Services.Factors.Queries.AccessoryGet;
//using Radin.Application.Services.Factors.CRM.Commands.EditWorkName;
//using Radin.Application.Services.CRM.Queries.CrmGet;
//using Radin.Infrastructure.IdentityConfigs;
//using Radin.Application.Services.CRM.Commands.ExitCrm;
//using Radin.Application.Services.Factors.Commands.StatusReason;
//using Radin.Application.Services.Factors.Queries.StatusReasonGet;
//using Radin.Application.Services.CRM.Commands.UpdateExpiration;
//using Radin.Application.Services.Factors.Queries.NegotiationGet;
//using Radin.Application.Services.Factors.Commands.Customer;
//using Radin.Application.Services.Factors.Queries.CustomerGet;
//using Radin.Application.Services.Branch.Commands.BranchRegisterService;
//using Radin.Application.Services.OtherExcelloading;
//using Radin.Application.Services.Factors.Commands.SetConnection;
//using Radin.Application.Services.Factors.Commands.NegotiationSet;
//using Radin.Application.Services.Factors.Queries.ConnectionsGet;
//using Radin.Application.Services.Factors.Commands.Orders.FinalizeOrder;
//using Radin.Application.Services.OKR.Queries.OkrGetService;
//using Radin.Application.Services.Factors.Commands.Pyment;
//using Radin.Application.Services.Factors.Queries.HesabfaBanksGet;
//using Radin.Application.Services.Factors.Queries.PymentPageInfoGet;
//using Radin.Application.Services.Operations.Check;
//using Radin.Application.Services.OKR.Queries.TargetDeterminationGet;
//using Radin.Application.Services.OKR.Commands.TargetDeterminationSet;
//using Radin.Application.Services.Factors.Commands.UndefinedProduct;
//using Radin.Application.Services.Factors.Commands.Service.ServiceProductSet;
//using Radin.Application.Services.Factors.Queries.PurchasedFactorGet;
//using Radin.Application.Services.FactorComplementation.Queries;
//using Radin.Application.Services.FactorComplementation.Commands;
//using Radin.Application.Services.Factors.Commands.ProductPriceDetailSet;
//using Radin.Application.Services.Factors.Queries.ProductPriceDetailGet;
//using Radin.Application.Services.Factors.Queries.FactorContractGet;
//using Radin.Application.Services.Factors.Commands.FactorContractSet;

//namespace Radin.Infrastructure.DependencyInjections
//{
//    public static class DependencyInjection
//    {
//        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
//        {
//            // Register infrastructure services, such as external APIs, file storage, etc.
//            services.AddScoped<ICommentInfoGetService, CommentInfoGetService>();
//            //_________________________________________________________________//
//            services.AddScoped<IChallPriceService, ChallPriceService>();
//            //_________________________________________________________________//
//            services.AddScoped<ITitleSetService, TitleSetService>();
//            services.AddScoped<ITitleGetService, TitleGetService>();
//            services.AddScoped<ITitleEditService, TitleEditService>();
//            services.AddScoped<ITitleRemoveService, TitleRemoveService>();
//            //_________________________________________________________________//
//            services.AddScoped<IEdgeSizeSetService, EdgeSizeSetService>();
//            services.AddScoped<IEdgeSizeGetService, EdgeSizeGetService>();
//            services.AddScoped<IEdgeSizeEditService, EdgeSizeEditService>();
//            services.AddScoped<IEdgeSizeRemoveService, EdgeSizeRemoveService>();
//            //_________________________________________________________________//
//            services.AddScoped<IPowerCalculationService, PowerCalculationService>();
//            //_________________________________________________________________//
//            services.AddScoped<ICrystalGetService, CrystalGetService>();
//            //_________________________________________________________________//
//            services.AddScoped<IMarginGetService, MarginGetService>();
//            //_________________________________________________________________//
//            services.AddScoped<IPowerGetService, PowerGetService>();
//            //_________________________________________________________________//
//            services.AddScoped<ISmdGetService, SmdGetService>();
//            services.AddScoped<IChannelliumGet, ChannelliumGet>();
//            services.AddScoped<IPlasticGetService, PlasticGetService>();
//            services.AddScoped<ISwediMaxGetService, SwediMaxGetService>();

//            //_________________________________________________________________//
//            services.AddScoped<IExcelLoadingFacad, ExcelLoadingFacad>();
//            services.AddScoped<IProductPriceFacad, ProductPriceFacad>();
//            services.AddScoped<IProductItemsFacad, ProductItemsFacad>();

//            //_________________________________________________________________//

//            services.AddScoped<CombinedResult>();
            
//            services.AddScoped<IHomeContentGetService, HomeContentGetService>();
//            services.AddScoped<IHomePageSliderGet, HomePageSliderGet>();
//            services.AddScoped<IHomePageSliderEditService, HomePageSliderEditService>();
//            services.AddScoped<IHomePageSliderRemoveService, HomePageSliderRemoveService>();
//            services.AddScoped<IHomePageSliderSetService, HomePageSliderSetService>();
//            services.AddScoped<IContactMessageSet, ContactMessageSet>();
//            //_________________________________________________________________//
//            services.AddScoped<IClaimSetService, ClaimSetService>();
//            services.AddScoped<IClaimGetService, ClaimGetService>();
//            services.AddScoped<IClaimInfoGetService, ClaimInfoGetService>();
//            services.AddScoped<IClaimCategorySetService, ClaimCategorySetService>();
//            services.AddScoped<IClaimCategoryGetService, ClaimCategoryGetService>();
//            //_________________________________________________________________//
//            services.AddScoped<IEmailSendService, EmailSendService>();
//            services.AddScoped<ISMSSendService, SMSSendService>();
//            services.AddScoped<ISMSCheckService, SMSCheckService>();
//            //_________________________________________________________________//

//            services.AddScoped<IIdeaFacad, IdeaFacad>();
//            services.AddScoped<ISampleFacad, SampleFacad>();

//            services.AddScoped<IContentFacad, ContentFacad>();
//            //_________________________________________________________________//

//            services.AddScoped<IBranchInfoSetService, BranchInfoSetService>();
//            services.AddScoped<IBranchInfoEditService, BranchInfoEditService>();
//            services.AddScoped<IBranchInfoGetService, BranchInfoGetService>();
//            services.AddScoped<IBranchUniqeGetService, BranchUniqeGetService>();
//            services.AddScoped<IBranchGetCodeService, BranchGetCodeService>();
//            services.AddScoped<IBranchRegisterService, BranchRegisterService>();
//            services.AddScoped<IPurchasedFactorGet, PurchasedFactorGet>();
//            //_________________________________________________________________//
//            services.AddScoped<IInitialOrderService, InitialOrderService>();
//            services.AddScoped<IEditWorkNameService, EditWorkNameService>();
//            services.AddScoped<IRecordProductService, RecordProductService>();
//            services.AddScoped<IOrderGetService, OrderGetService>();
//            services.AddScoped<ISubFactorGetService, SubFactorGetService>();
//            services.AddScoped<IGetProductFactors, GetProductFactors>();
//            services.AddScoped<IGetProductFactorDetiles, GetProductFactorDetiles>();
//            services.AddScoped<IFactorRemoveService, FactorRemoveService>();
//            services.AddScoped<ISubFactorRemoveService, SubFactorRemoveService>();
//            services.AddScoped<IProductFactorRemove, ProductFactorRemove>();
//            services.AddScoped<IAccessorySetService, AccessorySetService>();
//            services.AddScoped<IUndefinedProductSetService, UndefinedProductSetService>();
//            services.AddScoped<IFactorComplementationFieldsGetService, FactorComplementationFieldsGetService>();
//            services.AddScoped<IFactorComplementarySetService, FactorComplementarySetService>();
//            services.AddScoped<IProductPriceDetailSetService, ProductPriceDetailSetService>();
//            services.AddScoped<IProductPriceDetailGetSevice, ProductPriceDetailGetSevice>();
//            services.AddScoped<IFactorContractGet, FactorContractGet>();
//            services.AddScoped<IFactorContractSetService, FactorContractSetService>();

//            services.AddScoped<IMountFactorPriceService,MountFactorPriceService>();
//            services.AddScoped<IUpdatePrice, UpdatePrice>();
//            services.AddScoped<IAccessoryGetService, AccessoryGetService>();
//            services.AddScoped<ICountDiscountChangingService,CountDiscountChangingService>();
//            services.AddScoped<ICrmGetService, CrmGetService>();
//            services.AddScoped<IExitCrmService, ExitCrmService>();
//            services.AddScoped<IExpirationService, ExpirationService>();
//            services.AddScoped<INegotiationService, NegotiationService>();
//            services.AddScoped<ICustomerService, CustomerService>();
//            services.AddScoped<ICustomerGetService, CustomerGetService>();
//            services.AddScoped<IConnectionService, ConnectionService>();
//            services.AddScoped<INegotiationSetService, NegotiationSetService>();
//            services.AddScoped<IConnectionsGetService, ConnectionsGetService>();
//            services.AddScoped<IFinalizeOrderService, FinalizeOrderService>();
//            services.AddScoped<IOkrGetService, OkrGetService>();
//            services.AddScoped<IBanckCheckService, BanckCheckService>();
//            services.AddScoped<ITargetDeterminationGetService, TargetDeterminationGetService>();
//            services.AddScoped<ITargetDeterminationSetService, TargetDeterminationSetService>();
//            services.AddScoped<IServiceProductSet, ServiceProductSet>();
//            // Add more infrastructure-related services here
//            // 
//            services.AddScoped<ExcelloadingForCities, ExcelloadingForCities>();
//            services.AddScoped<CustomerExcelLoading, CustomerExcelLoading>();
//            services.AddScoped<MainFactorExcelLoading, MainFactorExcelLoading>();
//            services.AddScoped<JobCategoryExcelLoading, JobCategoryExcelLoading>();
//            services.AddScoped<AcessoryExcelloading, AcessoryExcelloading>();

//            services.AddScoped<QuestionService, QuestionService>();


//            services.AddScoped<IStateGetService , StateGetService>();
//            services.AddScoped<ICityGetService , CityGetService>();
//            //_______________________________________________________________________//
//            services.AddScoped<IStatusReasonSetService , StatusReasonSetService>();
//            services.AddScoped<IStatusRasonGetService , StatusReasonGetService>();
//            services.AddScoped<IStatusReasonRecoveryService , StatusReasonRecoveryService>();

//            //services.AddTransient<CookieConfigurationMiddleware, CookieConfigurationMiddleware>();

//            services.AddScoped<ICashPymentSaveService, CashPymentSaveService>();
//            services.AddScoped<IGetBanksService, GetBanksService>();
//            services.AddScoped<IPymentPageInfoGetService  , PymentPageInfoGetService>();
//            services.AddScoped<IPaymentService, PaymentService>();




//            return services;
//        }
//    }
//}
