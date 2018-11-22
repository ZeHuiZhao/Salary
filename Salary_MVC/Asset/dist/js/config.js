/**
 * @file 配置文件
 */

// 所有接口
var APIS = {
    //获取菜单
    //get 
    GetMenu: service + "/api/Function/GetMenu", //  获取菜单
    GetFunctionList: service + "/api/Function/GetFunctionList", //  获取菜单列表
    GetFunctionById: service + "/api/Function/GetFunctionById",  //查询单个菜单
    GetParentFunc: service + "/api/Function/GetParentFunc",  //查询父级菜单
    //post
    AddFunction: service + "/api/Function/AddFunction", //  添加菜单
    UpdateFunc: service + "/api/Function/UpdateFunc", //  编辑菜单

    //配置权限
    //GET 
    GetAllRole: service + "/api/Home/GetAllRole", //  获取所有角色包括超级管理员（用来角色权限下拉）
    GetPrivilegeList: service + "/api/Function/GetPrivilegeList",//获取权限列表
    //POST 
    UpdateRolePrivilege: service + "/api/Home/UpdateRolePrivilege", //  保存角色权限


    //首页
    //get
    Logout: service + "/api/Home/Logout", //  注销
    GetRoleList: service + "/api/Home/GetRoleList", //  获得角色列表
    GetAllRole: service + "/api/Home/GetAllRole", //  获得所有角色

    //post
    UpdatePassword: service + "/api/Home/UpdatePassword/",//  修改密码
    ToggleRole: service + "/api/Home/ToggleRole",//  切换角色

    //用户
    //get
    GetUserListByPaging: service + "/api/User/GetUserListByPaging", //  分页查询用户 
    GetOneUser: service + "/api/User/GetOneUser", //  查询单个用户 
    //post
    AddUser: service + "/api/User/AddUser", //  增加用户 
    DeleteUser: service + "/api/User/DeleteUser", //  删除用户
    UpdateChannelUser: service + "/api/User/UpdateChannelUser", //  编辑渠道用户
    ResetPassword: service + "/api/User/ResetPassword", //  重置密码
    ToggleChannelUser: service + "/api/User/ToggleChannelUser", //  切换用户状态
    ApprovalUser: service + "/api/User/ApprovalUser", //  销售员开通账号状态
    CustomPwd: service + "/api/Home/CustomPwd", //  修改密码
    //中力 - 素材管理
    //get
    GetZLMaterialTypeList: service + "/api/MaterialType/GetZLMaterialTypeList", //  获取素材类别列表 
    GetOneMaterialTypeById: service + "/api/MaterialType/GetOneMaterialTypeById", //  获取单个素材类别 
    GetMaterialListByZL: service + "/api/Material/GetMaterialListByZL", //  获取中力渠道列表
    GetMaterialById: service + "/api/Material/GetMaterialById", //  获取单条素材
    
    GetChannelList: service + "/api/Channel/GetChannelList", //  获取渠道列表 
    GetMaterialTypeList: service + "/api/MaterialType/GetMaterialTypeList", //  获取渠道类别 

    GetChannelTypeList: service + "/api/Channel/GetChannelTypeList", //  获取筛选条件
    //post
    AddMaterialType: service + "/api/MaterialType/AddMaterialType", //  添加素材类别 
    UpdateMaterialType: service + "/api/MaterialType/UpdateMaterialType", //  更新素材类别
    UpdateMaterialByZL: service + "/api/Material/UpdateMaterialByZL", //  更新中力素材
    ToggleStatus: service + "/api/MaterialType/ToggleStatus", //  切换可用状态
    GetQRcodeUrl: service + "/api/MaterialType/GetQRcodeUrl", //  预览素材类别二维码

    AddMaterialByZL: service + "/api/Material/AddMaterialByZL", //  添加素材
    ToggleStatusByZL: service + "/api/Material/ToggleStatusByZL", //  切换素材状态
    GetQRcodeUrlByZL: service + "/api/Material/GetQRcodeUrlByZL", //  素材二维码
    DeleteChannelMaterial: service + "/api/Material/DeleteChannelMaterial", //  删除素材

    UploadFile: service + "/api/File/UploadFile", //  上传图片

    //报名管理
    //get
    GetEnrollList: service + "/api/Enroll/GetEnrollList", //  获取报名列表
    GetClassTime: service + "/api/Enroll/GetClassTime", //  获取课程时间和课程类别封面
    GetEnrollDetail: service + "/api/Enroll/GetEnrollDetail", //  获得单条报名
    GetSaleList: service + "/api/User/GetSaleList", //  获取渠道销售员列表
    //post
    Reapproval: service + "/api/Enroll/Reapproval", //  退回重审
    ApprovalEnroll: service + "/api/Enroll/ApprovalEnroll", //  销售员审核
    UpdateEnroll: service + "/api/Enroll/UpdateEnroll", //  更新审核

    //WX
    //get
    GetRQcodeUrl: service + "/api/Wechat/GetRQcodeUrl", //  销售员二维码

    //用户管理
    //get
    GetCompanyList: service + "/api/Company/GetCompanyList", //  获取客户列表
    GetCompanyById: service + "/api/Company/GetCompanyById", //  根据id获取客户
    //post
    AddCompany: service + "/api/Company/AddCompany", //  添加客户
    ToggelInPublic: service + "/api/Company/ToggelInPublic", //  移动到公海
    UpdateCompany: service + "/api/Company/UpdateCompany", //  更新公司
    ToggleOutPublic: service + "/api/Company/ToggleOutPublic", //  移出公海，指派销售员
    ToggleInRecycle: service + "/api/Company/ToggleInRecycle", //  移动到回收站
    ToggleOutRecycle: service + "/api/Company/ToggleOutRecycle", //  客户移出回收站

    //客户详情
    //GET 
    GetCompanyContactList: service + "/api/CompanyContact/GetCompanyContactList", //  获取联系人列表
    GetCompanyContactListNotPager: service + "/api/CompanyContact/GetCompanyContactListNotPager", //  获取联系人列表 不分页
    GetCompanyContactById: service + "/api/CompanyContact/GetCompanyContactById", //  根据id查询联系人
    GetCompanyRecordList: service + "/api/ContactRecord/GetCompanyRecordList", //  获取联系记录列
    GetCompanyRecordById: service + "/api/ContactRecord/GetCompanyRecordById", //  根据id查询记录列
    //POST 
    AddCompanyContact: service + "/api/CompanyContact/AddCompanyContact", //  添加客户联系人
    UpdateCompanyContact: service + "/api/CompanyContact/UpdateCompanyContact", //  更新联系人
    SetFirstContact: service + "/api/CompanyContact/SetFirstContact", // 设置联系人为第一联系人
    DeleteCompanyContact: service + "/api/CompanyContact/DeleteCompanyContact", // 根据id删除联系人

    AddComapnyRecord: service + "/api/ContactRecord/AddComapnyRecord", // 添加联系记录
    DeleteCompanyRecord: service + "/api/ContactRecord/DeleteCompanyRecord", // 删除一条联系记录
    UpdateComapnyRecord: service + "/api/ContactRecord/UpdateComapnyRecord", // 更新联系记录

    //报表分析
    //GET 
    GetMaterialDataById: service + "/api/Report/GetMaterialDataById", // 获取素材分析详情
    GetMaterialSalesDataById: service + "/api/Report/GetMaterialSalesDataById", // 获取素材分析详情的表格
    GetChannelExportList: service + "/api/Report/GetChannelExportList", // 获取渠道分析报表
    GetChannelSalesExportList: service + "/api/Report/GetChannelSalesExportList", // 获取渠道分析报表详情的表格
    GetChannelYYList: service + "/api/Report/GetChannelYYList", // 获取渠道分销报表
    GetSalesDataById: service + "/api/Report/GetSalesDataById", // 渠道分析报表（第二级）根据销售员id查询（漏斗图）数据
    GetSalesMaterialList: service + "/api/Report/GetSalesMaterialList", // 渠道分析报表（第二级）根据销售员id查询素材列表
    
    //POST 
    ExportMaterialList: service + "/api/Report/ExportMaterialList", // 导出素材分析报表
    ExportMateriaSaleslList: service + "/api/Report/ExportMateriaSaleslList", // 导出素材详情的分析报表
    ExportSalesList: service + "/api/Report/ExportSalesList", // 导出渠道分析报表
    ExportChannelYYList: service + "/api/Report/ExportChannelYYList", // 导出渠道分销报表
    ExportSalesDataById: service + "/api/Report/ExportSalesDataById", // 根据销售员id导出数据

    //活动
    //GET 
    GetActivityList: service + "/api/Activity/GetActivityList", // 获取获得列表
    GetActivityById: service + "/api/Activity/GetActivityById", // 获取活动详情
    GetActivityStatus: service + "/api/Activity/GetActivityStatus", // 获取活动状态
    //POST 
    AddActivity: service + "/api/Activity/AddActivity", // 添加活动
    UpdateActivity: service + "/api/Activity/UpdateActivity", // 更新活动
    CopyActivity: service + "/api/Activity/CopyActivity", // 复制活动
    DeleteActivity: service + "/api/Activity/DeleteActivity", // 删除一条活动


};

