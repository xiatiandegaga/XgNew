using Cloud.Extensions;
using Cloud.Repositories;
using Cloud.Snowflake;
using Cloud.Utilities;
using Domain.Entity.Identity;
using Grpc.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApi
{
    public class FakeDataSeeder
    {
        public static async Task SeedAsync(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var _logger = scope.ServiceProvider.GetRequiredService<ILogger<FakeDataSeeder>>();
            try
            {
                var unitWork = scope.ServiceProvider.GetRequiredService<ICloudUnitOfWork>();
                var snowflakeIdWorker = scope.ServiceProvider.GetRequiredService<ISnowflakeIdWorker>();
                // 检查用户表是否存在数据
                var exists = await unitWork.ExistsAsync<User>();
                if (!exists)
                {
                    var account1 = configuration["DefaultSetting:DefaultAdmin"];
                    var pwd1 = EncryptionUtility.MD5(configuration["DefaultSetting:DefaultAdminPwd"]);
                    var userId1 = snowflakeIdWorker.NextId();
                    var encryPwd1 = EncryptionUtility.MD5(account1.StrReverse() + pwd1);
                    var users = new List<User>
               {
                   new() { Id=userId1,Account=account1,Password=encryPwd1  }
               };
                    unitWork.AddRange(users);
                    var roleId1 = snowflakeIdWorker.NextId();
                    var roles = new List<Role>
               {
                   new() { Id=roleId1,Name="admin",Level=0,Description="管理员"  }
               };
                    unitWork.AddRange(roles);

                    var userRole = new List<UserRole>
                {
                new() { RoleId=roleId1,UserId=userId1}
                };
                    unitWork.AddRange(userRole);


                    var menus = new List<Menu>
{
    // -------------------- 根菜单 (Pid=0) --------------------
    new Menu { Id = 1, Name = "System", Category = 1, MetaTitle = "系统管理", MetaIcon = "user-circle", Pid = 0, Path = "/system", Component = "LAYOUT", Redirect = "/system/menu/index", SortNo = 1 },
    new Menu { Id = 443344236394315776, Name = "Mall", Category = 1, MetaTitle = "商品管理", MetaIcon = "shop", Pid = 0, Path = "/mall", Component = "LAYOUT", Permission = "mall", SortNo = 2 },
    new Menu { Id = 600708474099204096, Name = "Order", Category = 1, MetaTitle = "订单管理", MetaIcon = "shop", Pid = 0, Path = "/order", Component = "LAYOUT", Permission = "order", SortNo = 3 },

    // -------------------- 系统管理子菜单 (Pid=1) --------------------
    new Menu { Id = 2, Name = "RoleManagement", Category = 1, MetaTitle = "角色管理", Pid = 1, Path = "role", Component = "/system/role/index", Permission = "role", SortNo = 6 },
    new Menu { Id = 5, Name = "SystemMenu", Category = 1, MetaTitle = "菜单管理", Pid = 1, Path = "menu", Component = "/system/menu/index", Permission = "menu", SortNo = 5 },
    new Menu { Id = 6, Name = "globalData", Category = 1, MetaTitle = "数据字典", Pid = 1, Path = "globalData", Component = "/system/globalData/index", Permission = "globalData", SortNo = 8 },
    new Menu { Id = 8, Name = "UserListManagement", Category = 1, MetaTitle = "新增", Pid = 4, Path = "userList", Component = "/system/userList/index", Permission = "user:add", SortNo = 7 },
    new Menu { Id = 426430514149195776, Name = "UserManagement", Category = 1, MetaTitle = "用户管理", Pid = 1, Path = "user", Component = "/system/user/index", Permission = "user", SortNo = 10 },
    new Menu { Id = 429301087644680192, Name = "AdminUserManagement", Category = 1, MetaTitle = "账户管理", Pid = 1, Path = "adminUser", Component = "/system/adminUser/index", Permission = "adminUser", SortNo = 4 },
    new Menu { Id = 430686572183027712, Name = "ImgRule", Category = 1, MetaTitle = "自定义图片", Pid = 1, Path = "imgRule", Component = "/system/imgRule/index", Permission = "imgRule", SortNo = 11 },

    // -------------------- 商品管理模块 (Pid=443344236394315776) --------------------
    new Menu { Id = 443344619623677952, Name = "MallProduct", Category = 1, MetaTitle = "商品列表", Pid = 443344236394315776, Path = "/mallProduct", Component = "/mall/mallProduct/index", Permission = "mallProduct", SortNo = 3 },
    new Menu { Id = 443345557184839680, Name = "MallBrand", Category = 1, MetaTitle = "品牌管理", Pid = 443344236394315776, Path = "/mallBrand", Component = "/mall/mallBrand/index", Permission = "mallBrand", SortNo = 2 },
    new Menu { Id = 443346195012648960, Name = "MallProductType", Category = 1, MetaTitle = "商品类型", Pid = 443344236394315776, Path = "/mallProductType", Component = "/mall/mallProductType/index", Permission = "mallProductType", SortNo = 3 },
    new Menu { Id = 443346438164840448, Name = "MallProductCategory", Category = 1, MetaTitle = "商品目录", Pid = 443344236394315776, Path = "/mallProductCategory", Component = "/mall/mallProductCategory/index", Permission = "mallProductCategory", SortNo = 4 },
    new Menu { Id = 443776354844934144, Name = "mallProductAttr", Category = 1, MetaTitle = "属性列表", Pid = 443344236394315776, Path = "/mallProductAttr", Component = "/mall/mallProductAttr/index", Permission = "mallProductAttr", SortNo = 5 },
    new Menu { Id = 601422921574907904, Name = "mallProductSkuStockHistoryIndex", Category = 1, MetaTitle = "商品出入库记录", Pid = 443344236394315776, Path = "/mallProductSkuStockHistory", Component = "/mall/mallProductSkuStockHistory/index", Permission = "mallProductSkuStockHistory", SortNo = 9 },

    // -------------------- 订单管理模块 (Pid=600708474099204096) --------------------
    new Menu { Id = 600709052309176320, Name = "mallOrderIndex", Category = 1, MetaTitle = "订单信息", Pid = 600708474099204096, Path = "mallOrder", Component = "/order/mallOrder/index", Permission = "mallOrder", SortNo = 1 },
    new Menu { Id = 600709369641828352, Name = "mallReturnOrderIndex", Category = 1, MetaTitle = "退单申请", Pid = 600708474099204096, Path = "mallReturnOrder", Component = "/order/mallReturnOrder/index", Permission = "mallReturnOrder", SortNo = 2 },
    new Menu { Id = 627159682728853504, Name = "mallOrderPayCashFlowIndex", Category = 1, MetaTitle = "订单支付流水", Pid = 600708474099204096, Path = "mallOrderPayCashFlow", Component = "/order/mallOrderPayCashFlow/index", Permission = "mallOrderPayCashFlow", SortNo = 3 },
    new Menu { Id = 627174351417901056, Name = "mallOrderRefundCashFlowIndex", Category = 1, MetaTitle = "订单退款流水", Pid = 600708474099204096, Path = "mallOrderRefundCashFlow", Component = "/order/mallOrderRefundCashFlow/index", Permission = "mallOrderRefundCashFlow", SortNo = 4 },

    // -------------------- 按钮操作 (Category=2) --------------------
    new Menu { Id = 439432298392190976, Category = 2, MetaTitle = "新增", Pid = 426430514149195776, Permission = "user:add", SortNo = 12 },
    new Menu { Id = 439460656794566656, Category = 2, MetaTitle = "删除", Pid = 426430514149195776, Permission = "user:delete", SortNo = 0 },
    new Menu { Id = 439478136078336000, Category = 2, MetaTitle = "编辑", Pid = 426430514149195776, Permission = "user:edit", SortNo = 13 },
    new Menu { Id = 442336902683557888, Category = 2, MetaTitle = "新建", Pid = 429301087644680192, Permission = "adminUser:add", SortNo = 13 },
    new Menu { Id = 442336967372308480, Category = 2, MetaTitle = "编辑", Pid = 429301087644680192, Permission = "adminUser:edit", SortNo = 14 },
    new Menu { Id = 442337016667963392, Category = 2, MetaTitle = "删除", Pid = 429301087644680192, Permission = "adminUser:delete", SortNo = 15 },
    new Menu { Id = 442337074297700352, Category = 2, MetaTitle = "重置密码", Pid = 429301087644680192, Permission = "adminUser:reset", SortNo = 16 },
    new Menu { Id = 442365698631532544, Category = 2, MetaTitle = "新增", Pid = 5, Permission = "menu:add", SortNo = 17 },
    new Menu { Id = 442365853464264704, Category = 2, MetaTitle = "编辑", Pid = 5, Permission = "menu:edit", SortNo = 18 },
    new Menu { Id = 442365891540156416, Category = 2, MetaTitle = "删除", Pid = 5, Permission = "menu:delete", SortNo = 18 },
    new Menu { Id = 442366117751554048, Category = 2, MetaTitle = "新增", Pid = 2, Permission = "role:add", SortNo = 19 },
    new Menu { Id = 442366170956300288, Category = 2, MetaTitle = "编辑", Pid = 2, Permission = "role:edit", SortNo = 20 },
    new Menu { Id = 442366215411728384, Category = 2, MetaTitle = "删除", Pid = 2, Permission = "role:delete", SortNo = 21 },
    new Menu { Id = 442366434861907968, Category = 2, MetaTitle = "新增", Pid = 6, Permission = "globalData:add", SortNo = 22 },
    new Menu { Id = 442366489245253632, Category = 2, MetaTitle = "修改", Pid = 6, Permission = "globalData:edit", SortNo = 23 },
    new Menu { Id = 442366707558776832, Category = 2, MetaTitle = "字典详情新增", Pid = 6, Permission = "globalData:detailAdd", SortNo = 24 },
    new Menu { Id = 442366757076729856, Category = 2, MetaTitle = "字典详情编辑", Pid = 6, Permission = "globalData:detailEdit", SortNo = 25 },
    new Menu { Id = 442366811921448960, Category = 2, MetaTitle = "字典详情删除", Pid = 6, Permission = "globalData:detailDelete", SortNo = 26 },
    new Menu { Id = 442367607614472192, Category = 2, MetaTitle = "新增", Pid = 430686572183027712, Permission = "imgRule:add", SortNo = 26 },
    new Menu { Id = 442367703479484416, Category = 2, MetaTitle = "编辑", Pid = 430686572183027712, Permission = "imgRule:edit", SortNo = 27 },
    new Menu { Id = 442368430067154944, Category = 2, MetaTitle = "删除", Pid = 430686572183027712, Permission = "imgRule:delete", SortNo = 28 },
    new Menu { Id = 443357988510498816, Category = 2, MetaTitle = "新建", Pid = 443345557184839680, Permission = "mallBrand:add", SortNo = 1 },
    new Menu { Id = 443358042449248256, Category = 2, MetaTitle = "编辑", Pid = 443345557184839680, Permission = "mallBrand:edit", SortNo = 2 },
    new Menu { Id = 443358087542210560, Category = 2, MetaTitle = "删除", Pid = 443345557184839680, Permission = "mallBrand:delete", SortNo = 3 },
    new Menu { Id = 595934484105265152, Category = 2, MetaTitle = "新建", Pid = 443344619623677952, Permission = "mallProduct:add", SortNo = 1 },
    new Menu { Id = 595934571678138368, Category = 2, MetaTitle = "编辑", Pid = 443344619623677952, Permission = "mallProduct:edit", SortNo = 2 },
    new Menu { Id = 595934631962869760, Category = 2, MetaTitle = "删除", Pid = 443344619623677952, Permission = "mallProduct:delete", SortNo = 3 },
    new Menu { Id = 595934726871580672, Category = 2, MetaTitle = "复制", Pid = 443344619623677952, Permission = "mallProduct:copy", SortNo = 4 },
    new Menu { Id = 595934803593789440, Category = 2, MetaTitle = "上架", Pid = 443344619623677952, Permission = "mallProduct:takeOn", SortNo = 5 },
    new Menu { Id = 595934865157783552, Category = 2, MetaTitle = "下架", Pid = 443344619623677952, Permission = "mallProduct:takeOff", SortNo = 6 },
    new Menu { Id = 595934964298547200, Category = 2, MetaTitle = "批量上架", Pid = 443344619623677952, Permission = "mallProduct:batchTakeOn", SortNo = 7 },
    new Menu { Id = 595935041842839552, Category = 2, MetaTitle = "批量下架", Pid = 443344619623677952, Permission = "mallProduct:batchTakeOff", SortNo = 8 },
    new Menu { Id = 596759534391787520, Category = 2, MetaTitle = "新建", Pid = 443346195012648960, Permission = "mallProductType:add", SortNo = 1 },
    new Menu { Id = 600709640640004096, Category = 2, MetaTitle = "发货", Pid = 600709052309176320, Permission = "mallOrder:send", SortNo = 1 },
    new Menu { Id = 600709798706544640, Category = 2, MetaTitle = "审核", Pid = 600709369641828352, Permission = "mallReturnOrder:check", SortNo = 1 },
    new Menu { Id = 600709940985724928, Category = 2, MetaTitle = "退货入库", Pid = 600709369641828352, Permission = "mallReturnOrder:returnInv", SortNo = 2 },
    new Menu { Id = 600710082887417856, Category = 2, MetaTitle = "退款", Pid = 600709369641828352, Permission = "mallReturnOrder:refund", SortNo = 3 },
    new Menu { Id = 635070661345148928, Category = 2, MetaTitle = "退货", Pid = 600709369641828352, Permission = "mallReturnOrder:returned", SortNo = 4 }
};
                    menus = menus.OrderBy(m => m.Id).ToList();
                    unitWork.AddRange(menus);


                    var roleMenus = new List<RoleMenu>();
                    foreach (var menu in menus)
                    {
                        roleMenus.Add(new RoleMenu { RoleId = roleId1, MenuId = menu.Id });
                    }

                    unitWork.AddRange(roleMenus);

                    await unitWork.CommitAsync();
                    _logger.LogInformation("FakeDataSeeder insert success");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"FakeDataSeeder insert fail -{ex.Message}");
            }

        }
    }
}
